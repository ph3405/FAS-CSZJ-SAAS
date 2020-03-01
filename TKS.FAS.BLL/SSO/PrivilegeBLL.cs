using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Entity.SSO;
using TKS.FAS.Entity;
using TKS.FAS.Common;
using Dapper;
using DapperExtensions;
using System.Configuration;
using TKS.FAS.Entity.FAS;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Sms.Model.V20160927;



namespace TKS.FAS.BLL
{
    public partial class UserBLL
    {
        private char[] constant =
      {
        '0','1','2','3','4','5','6','7','8','9'
      };
        string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
        public ResponseUserLogin UserLogin(RequestUserLogin request)
        {
            ResponseUserLogin response = new ResponseUserLogin();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(request.Password, "MD5");

                    var users = cnn.Query<TKS_FAS_UserExt>(@"select A.*,B.NodeId ,C.Name as NodeName from tks_fas_user 
                        A left join TKS_FAS_User2Node B on A.id=b.UserId
                        left join TKS_FAS_Node C on B.NodeId=c.id where 
                    A.userName=@UserName and A.password=@Password",
                     new { UserName = request.UserName, Password = pass }, ts).ToList();
                    if (users.Count() == 0)
                    {
                        users = cnn.Query<TKS_FAS_UserExt>(@"select A.*,B.NodeId ,C.Name as NodeName from tks_fas_user 
                        A left join TKS_FAS_User2Node B on A.id=b.UserId
                        left join TKS_FAS_Node C on B.NodeId=c.id where 
                    A.Mobile=@Mobile and A.password=@Password",
                     new { Mobile = request.Mobile, Password = pass }, ts).ToList();
                        if (users.Count() == 0)
                        {
                            throw new NormalException("用户名或者密码错误");
                        }

                    }

                    TKS_FAS_UserExt user = users[0];
                    if (user.Status == "0")
                        throw new NormalException("您的账号已被停用");

                    //token

                    cnn.Execute("delete from tks_fas_token where userid=@UserId and source='WEB'", new { UserId = user.Id }, ts);
                    TKS_FAS_Token token = new TKS_FAS_Token();
                    token.Id = Guid.NewGuid().ToString("N");
                    token.UserId = user.Id;
                    token.Token = Guid.NewGuid().ToString("N");
                    token.ActiveTime = DateTime.Now;
                    token.Source = "WEB";
                    cnn.Insert<TKS_FAS_Token>(token, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Id = user.Id;
                    response.Sex = user.Sex;
                    response.Token = token.Token;
                    response.UserName = user.UserName;
                    response.TrueName = user.TrueName;
                    response.NodeName = user.NodeName;
                    response.Message = "登陆成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserLogin;
                }
            }
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseAccountInvitationSend SendMessage(RequestAccountInvitationSend request)
        {
            var res = new ResponseAccountInvitationSend();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    if (string.IsNullOrEmpty(request.MobilePhone))
                    {
                        throw new Exception("手机号不能为空");
                    }
                    // string code = GenerateRandomNumber(5);

                    string vcode = GenerateRandomNumber(4);
                    IClientProfile profile = DefaultProfile.GetProfile("", "", "");
                    IAcsClient client = new DefaultAcsClient(profile);
                    SingleSendSmsRequest req = new SingleSendSmsRequest();
                    req.SignName = "章小算";//"管理控制台中配置的短信签名（状态必须是验证通过）"
                    req.TemplateCode = "SMS_115165077";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
                    req.RecNum = request.MobilePhone;//"接收号码，多个号码可以逗号分隔"
                    req.ParamString = "{\"code\":\"" + vcode + "\"}";//切记前面的code字段，只能是英文，不能是中文！！短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
                    SingleSendSmsResponse httpResponse = client.GetAcsResponse(req);

                    TKS_FAS_MobileVerification context = new TKS_FAS_MobileVerification();
                    context.Id = Guid.NewGuid().ToString("N");
                    context.Mobile = request.MobilePhone;
                    context.VerCode = vcode;
                    context.CodeType = VerificationType.WX_Regist.ToString();
                    context.Status = "OP";
                    context.CreateDate= DateTime.Now;
                    cnn.Insert<TKS_FAS_MobileVerification>(context, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "";

                }
                catch (Exception ex)
                {
                    res.IsSuccess = false;
                    res.Message = ex.Message;
                }
            }
            return res;
        }
        public ResponseUserLogin_WX WXLogin(RequestUserLogin request)
        {
            ResponseUserLogin_WX response = new ResponseUserLogin_WX();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var users = cnn.Query<TKS_FAS_MobileVerification>(@"select * from TKS_FAS_MobileVerification where Status='OP' and CodeType='WX_Regist' and Mobile=@Mobile and VerCode=@VerCode",
                     new { Mobile = request.Mobile, VerCode = request.VerCode }, ts).FirstOrDefault();
                    if (users == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "验证码失效";
                        return response;
                    }
                    else
                    {
                        string userid = "";
                    
                        string role = ConfigurationManager.AppSettings["GZQY-ADMIN"];
                        cnn.Execute("update TKS_FAS_MobileVerification set Status='SE' where Id=@Id", new { Id = users.Id }, ts);
                        UserBLL bll = new UserBLL();
                        string sql = @"select * from TKS_FAS_User where mobile=@Mobile";
                        var user = cnn.Query<TKS_FAS_User>(sql, new { Mobile = request.Mobile }, ts).FirstOrDefault();
                        if (user != null)
                        {
                            userid = user.Id;
                            response.user = user;

                            //判断是否有企业主角色，没有则添加企业主角色权限
                            var QY = cnn.Query<TKS_FAS_User2Role>(@"select * from TKS_FAS_User2Role where UserId=@UserId and RoleId=@RoleId", new { UserId = user.Id, RoleId = role }, ts).FirstOrDefault();
                            if (QY == null)
                            {
                                TKS_FAS_User2Role u2r = new TKS_FAS_User2Role();
                                u2r.Id = Guid.NewGuid().ToString();
                                u2r.UserId = user.Id;
                                u2r.RoleId = role;//雇主企业管理员角色ID，注册默认;
                                cnn.Insert<TKS_FAS_User2Role>(u2r, ts);
                            }
                            response.IsSuccess = true;
                            //response.UserType = "0";//有关联的企业账套，直接登录进入主页面
                            response.Message = "登陆成功";
                        }
                        else
                        {
                            //创建企业主账号,赋予企业主角色权限
                            TKS_FAS_User newuser = new TKS_FAS_User();
                            newuser.Id = Guid.NewGuid().ToString("N");
                            newuser.UserName = request.Mobile;
                            newuser.TrueName = request.Mobile;
                            //newuser.Sex = request.User.Sex;
                            newuser.Mobile = request.Mobile;
                            string pass = System.Web.Security.FormsAuthentication.
                                HashPasswordForStoringInConfigFile("123456", "MD5");


                            newuser.Password = pass;
                            newuser.Status = "1";//启用
                            newuser.CreateUser = "系统注册";
                            newuser.CreateDate = DateTime.Now;
                            cnn.Insert<TKS_FAS_User>(newuser, ts);
                            TKS_FAS_User2Role u2r = new TKS_FAS_User2Role();
                            u2r.Id = Guid.NewGuid().ToString();
                            u2r.UserId = newuser.Id;
                            u2r.RoleId = role;//雇主企业管理员角色ID，注册默认;
                            cnn.Insert<TKS_FAS_User2Role>(u2r, ts);
                            response.user = newuser;
                            response.IsSuccess = true;
                            response.Message = "登陆成功";

                            userid = newuser.Id;
                        }
                        //token

                        cnn.Execute("delete from tks_fas_token where userid=@UserId and source='WXAPP'", new { UserId = userid }, ts);
                        TKS_FAS_Token token = new TKS_FAS_Token();
                        token.Id = Guid.NewGuid().ToString("N");
                        token.UserId = userid;
                        token.Token = Guid.NewGuid().ToString("N");
                        token.ActiveTime = DateTime.Now;
                        token.Source = "WXAPP";
                        cnn.Insert<TKS_FAS_Token>(token, ts);
                        ts.Commit();
                        response.Token = token.Token;
                  
                       
        
                       
                        return response;
                    }

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserLogin_WX;
                }
            }
        }

        /// <summary>
        /// 微信小程序验证邀请码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBase WX_CheckInvitation(RequestMyAccountAdd request)
        {
            ResponseBase response = new ResponseBase();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //根据邀请码获取账套信息
                    string sql = @"select * from TKS_FAS_AccountInfo where InvitationCode=@Code";

                    var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { Code = request.InvitationCode }, ts);
                    if (account == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "验证码失效";
                        return response;
                    }
                    else
                    {
                        var Account2User = cnn.QueryFirstOrDefault<TKS_FAS_Account2User>(@"select * from TKS_FAS_Account2User where AccountId=@AccountId and UserId=@UserId", new { AccountId = account.Id, UserId= user.User.Id }, ts);
                        if (Account2User == null)
                        {
                            TKS_FAS_Account2User newAccount2User = new TKS_FAS_Account2User();
                            newAccount2User.Id = Guid.NewGuid().ToString("N");
                            newAccount2User.AccountId = account.Id;
                            newAccount2User.UserId = user.User.Id;
                            newAccount2User.CreateDate = DateTime.Now;
                            cnn.Insert<TKS_FAS_Account2User>(newAccount2User, ts);
                        }
                        //激活账套
                        string sql_active = @"delete from TKS_FAS_UserCurrentAccount where userId=@UserId";

                        cnn.Execute(sql_active, new { UserId = user.User.Id }, ts);


                        TKS_FAS_UserCurrentAccount cur = new TKS_FAS_UserCurrentAccount();
                        cur.Id = Guid.NewGuid().ToString("N");
                        cur.UserId = user.User.Id;
                        //cur.AccountId = request.Data.Id;
                        cur.AccountId = account.Id;
                        cnn.Insert<TKS_FAS_UserCurrentAccount>(cur, ts);
                        response.IsSuccess = true;
                        response.Message = "登陆成功";
                    }
                    ts.Commit();
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBase;
                }
            }
        }

        /// <summary>
        /// 获取可操作的账套列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseOPAccountListGet WX_GetAccountList(RequestOPAccountListGet request)
        {
            ResponseOPAccountListGet response = new ResponseOPAccountListGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    //var user = this.UserInfoGetButAccount(request.Token, ts);
                    var user = this.UserInfoGet(request.Token, ts);
                    List<TKS_FAS_AccountInfoExt> data = new List<TKS_FAS_AccountInfoExt>();
                    string sql = string.Empty;
                    sql = @"SELECT b.*
                            FROM TKS_FAS_Account2User a
	                            INNER JOIN TKS_FAS_AccountInfo b ON a.AccountId = b.Id
                            WHERE a.UserId = @UserId";
                    data = cnn.Query<TKS_FAS_AccountInfoExt>(sql, new { UserId = user.User.Id }, ts).ToList();

                    sql = @"select * from TKS_FAS_UserCurrentAccount where userid=@UserId";
                    var curAccount = cnn.Query<TKS_FAS_UserCurrentAccount>(sql, new { UserId = user.User.Id }, ts).ToList();
                    if (curAccount.Count() == 0 && data.Count() > 0)
                    {
                        data[0].Active = 0;
                        //TKS_FAS_MonthPeriodInfo period = GetActivePeriod(data[0].Id, ts);
                        response.IsSelected = false;
                        //response.Year = period.Year;
                        //response.Month = period.Month;
                    }
                    else if (curAccount.Count() > 0)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (data[i].Id == curAccount[0].AccountId)
                            {

                                data[i].Active = 1;
                                //var period = GetActivePeriod(data[i].Id, ts);
                                response.IsSelected = true;
                                //response.Year = period.Year;
                                //response.Month = period.Month;
                                break;
                            }
                        }
                    }

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Data = data;
                    //response.SelectAccount = data.Where(x => x.Active == 1).FirstOrDefault();
                    //response.UserCreditCode = user.Node.CreditCode;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseOPAccountListGet;
                }

            }
        }
        public ResponseUserLogin UserCheck(RequestUserLogin request)
        {
            ResponseUserLogin response = new ResponseUserLogin();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(request.Password, "MD5");

                    var users = cnn.Query<TKS_FAS_User>(@"select * from tks_fas_user where 
                    userName=@UserName and password=@Password",
                     new { UserName = request.UserName, Password = pass }, ts).ToList();
                    if (users.Count() == 0)
                    {
                        throw new NormalException("用户名或者密码错误");
                    }

                    TKS_FAS_User user = users[0];
                    if (user.Status == "0")
                        throw new NormalException("您的账号已被停用");


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Id = user.Id;

                    response.UserName = user.UserName;
                    response.TrueName = user.TrueName;
                    response.Message = "校验成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserLogin;
                }
            }
        }



        public bool IsMobileExist(string mobile)
        {
            using (cnn = GetConnection())
            {
                string sql = @"select * from TKS_FAS_User where mobile=@Mobile";
                var data = cnn.Query(sql, new { Mobile = mobile });
                if (data.Count() > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public ResponseUserRegister UserRegister(RequestUserRegister request)
        {
            ResponseUserRegister res = new ResponseUserRegister();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = @"select * from TKS_FAS_User where userName=@UserName ";
                    var data = cnn.Query(sql, new { UserName = request.User.UserName }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("用户名已存在");
                    }

                    sql = @"select * from TKS_FAS_User where mobile=@Mobile";
                    data = cnn.Query(sql, new { Mobile = request.User.Mobile }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("手机号已存在");
                    }
                    if (request.Node.Type == 1 && string.IsNullOrEmpty(request.Node.Name))
                    {
                        request.Node.Name = request.User.UserName;
                    }

                    sql = @"select * from TKS_FAS_Node where name=@Name ";
                    data = cnn.Query(sql, new { Name = request.Node.Name }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("企业名已存在");
                    }

                    string admin = string.Empty;

                    if (request.Node.Type == 0)
                    {
                        admin = ConfigHelper.Read("GZQY-ADMIN");//雇主企业管理员角色ID，注册默认
                    }
                    else
                    {
                        admin = ConfigHelper.Read("DZQY-ADMIN");//代帐企业管理员角色ID，注册默认
                    }

                    TKS_FAS_User user = new TKS_FAS_User();
                    user.Id = Guid.NewGuid().ToString("N");
                    user.UserName = request.User.UserName;
                    user.TrueName = request.User.UserName;
                    user.Sex = request.User.Sex;
                    user.Mobile = request.User.Mobile;
                    string pass = System.Web.Security.FormsAuthentication.
                        HashPasswordForStoringInConfigFile(request.User.Password, "MD5");
                    user.Province = request.User.Province;
                    user.City = request.User.City;
                    user.Town = request.User.Town;
                    user.ZCService = request.User.ZCService;
                    if (request.Node.Type == 0)
                    {
                        user.DZService = 1;
                    }
                    else
                    {
                        user.DZService = 0;
                    }

                    user.Password = pass;
                    user.Status = "1";//启用
                    user.CreateUser = "系统注册";
                    user.CreateDate = DateTime.Now;

                    TKS_FAS_Node node = new TKS_FAS_Node();
                    node.Id = Guid.NewGuid().ToString("N");
                    node.Name = string.IsNullOrEmpty(request.Node.Name) ? user.UserName : request.Node.Name;
                    node.CreditCode = Guid.NewGuid().ToString("N");//用作唯一key
                    node.IsOutSource = request.Node.IsOutSource;
                    node.Type = request.Node.Type;
                    node.UCode = request.Node.UCode;
                    node.CreateUser = user.UserName;
                    node.CreateDate = DateTime.Now;

                    TKS_FAS_User2Node u2n = new TKS_FAS_User2Node();
                    u2n.Id = Guid.NewGuid().ToString("N");
                    u2n.NodeId = node.Id;
                    u2n.UserId = user.Id;

                    TKS_FAS_User2Role u2r = new TKS_FAS_User2Role();
                    u2r.Id = Guid.NewGuid().ToString();
                    u2r.UserId = user.Id;
                    u2r.RoleId = admin;

                    cnn.Insert<TKS_FAS_User>(user, ts);
                    cnn.Insert<TKS_FAS_Node>(node, ts);
                    cnn.Insert<TKS_FAS_User2Node>(u2n, ts);
                    cnn.Insert<TKS_FAS_User2Role>(u2r, ts);

                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "注册成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseUserRegister;
                }
            }
        }


        public ResponseUserMenusGet UserMenuGet(RequestUserMenusGet request)
        {
            ResponseUserMenusGet response = new ResponseUserMenusGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string rootId = request.FuncId;
                    //rootId = ConfigurationManager.AppSettings[key];
                    var userInfo = this.UserInfoGetButAccount(request.Token, ts);
                    #region 权限控制 用户进入平台管理或者会计操作页面 add by Hero.Zhang
                    var Permission = this.GetPermission(request.Token, ts);
                    if (userInfo.User.UserName == "admin")
                    {
                        rootId = ConfigurationManager.AppSettings["funcId_admin"];
                    }
                    else
                    {
                        if (Permission == null)
                        {
                            response.Data = null;
                            response.Message = "没有角色权限";
                            response.IsSuccess = false;
                            return response;
                        }
                        if (Permission.PLevel == 1)
                        {
                            rootId = ConfigurationManager.AppSettings["funcId_admin"];
                        }
                        else if (Permission.PLevel > 1)
                        {
                            rootId = ConfigurationManager.AppSettings["funcId"];
                        }
                    }
                    #endregion
                    //role 
                    var roles = cnn.Query<string>("select roleid from TKS_FAS_User2Role where userid=@UserId",
                        new { UserId = userInfo.User.Id }, ts).ToList();
                    string inId = string.Empty;
                    if (roles.Count() > 0)
                    {
                        inId = "and entityId in ('" + string.Join("','", roles.ToArray()) + "')";
                    }
                    else
                    {
                        inId = " and 1=2";
                    }



                    //func by role
                    string sql = @"select functionId from TKS_FAS_Entity2Function where type=@Type  {0} group by functionId";
                    sql = string.Format(sql, inId);


                    var checkedFuncs = cnn.Query<string>(sql, new
                    {
                        Type = "R"
                    }, ts).ToList();
                    string inFuncs = string.Empty;
                    if (checkedFuncs.Count() > 0)
                    {
                        inFuncs = " and  id in ('" + string.Join("','", checkedFuncs.ToArray()) + "')";
                    }
                    else
                    {
                        inFuncs = " and 1=2";
                    }


                    sql = @"WITH CTE  AS
                         (SELECT *
                            FROM tks_fas_function
                           WHERE id = @Id
  
                          UNION ALL
                          SELECT B.*
                            FROM tks_fas_function B
                           INNER JOIN CTE
                              ON  B.PARENTID = CTE.id)
      
                     SELECT * FROM CTE  where 1=1 {0} order by seq";
                    sql = string.Format(sql, inFuncs);

                    var funcs = cnn.Query<TKS_FAS_Function>(sql, new { Id = rootId }, ts).ToList();


                    TKS_FAS_MenuItem root = new TKS_FAS_MenuItem();
                    var first = funcs.Where(p => p.Id == rootId).FirstOrDefault();
                    if (first == null)
                    {
                        response.Data = null;
                        response.Message = "没有权限";
                        response.IsSuccess = false;
                    }
                    else
                    {
                        root = Trans2MenuItem(first);
                        CreateTree(funcs, ref root);
                        response.Data = root.children;
                        response.Message = "加载完毕";
                        response.IsSuccess = true;
                        response.FuncId = rootId;
                    }

                    ts.Commit();

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserMenusGet;
                }
            }


        }

        public ResponseUserMenusGet GetMenu(RequestUserMenusGet request)
        {
            ResponseUserMenusGet response = new ResponseUserMenusGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string rootId = ConfigurationManager.AppSettings["funcId"];
                    //string lst_rootId = "('"+ rootId + "','27bdaf9d01bc4960b2be86c8923b4b56','6ca8509327ae49c49008f35595c997c7','9de61fd017954080888318fca45b023e')";
                    string lst_rootId = "('27bdaf9d01bc4960b2be86c8923b4b56','6ca8509327ae49c49008f35595c997c7','9de61fd017954080888318fca45b023e')";
                    var userInfo = this.UserInfoGetButAccount(request.Token, ts);

                    //role 
                    var roles = cnn.Query<string>("select roleid from TKS_FAS_User2Role where userid=@UserId",
                        new { UserId = userInfo.User.Id }, ts).ToList();
                    string inId = string.Empty;
                    if (roles.Count() > 0)
                    {
                        inId = "and entityId in ('" + string.Join("','", roles.ToArray()) + "')";
                    }
                    else
                    {
                        inId = " and 1=2";
                    }



                    //func by role
                    string sql = @"select functionId from TKS_FAS_Entity2Function where type=@Type  {0} group by functionId";
                    sql = string.Format(sql, inId);


                    var checkedFuncs = cnn.Query<string>(sql, new
                    {
                        Type = "R"
                    }, ts).ToList();
                    string inFuncs = string.Empty;
                    if (checkedFuncs.Count() > 0)
                    {
                        inFuncs = " and  id in ('" + string.Join("','", checkedFuncs.ToArray()) + "')";
                    }
                    else
                    {
                        inFuncs = " and 1=2";
                    }


                    sql = @"WITH CTE  AS
                         (SELECT *
                            FROM tks_fas_function
                           WHERE id in{0}
  
                          UNION ALL
                          SELECT B.*
                            FROM tks_fas_function B
                           INNER JOIN CTE
                              ON  B.PARENTID = CTE.id)
      
                     SELECT * FROM CTE  where 1=1 {1} union
	SELECT *
		FROM tks_fas_function
		WHERE id = @Id order by seq";
                    sql = string.Format(sql, lst_rootId, inFuncs);

                    var funcs = cnn.Query<TKS_FAS_Function>(sql, new { Id = rootId }, ts).ToList();


                    TKS_FAS_MenuItem root = new TKS_FAS_MenuItem();
                    var first = funcs.Where(p => p.Id == rootId).FirstOrDefault();
                    if (first == null)
                    {
                        response.Data = null;
                        response.Message = "没有权限";
                        response.IsSuccess = false;
                    }
                    else
                    {
                        root = Trans2MenuItem(first);
                        CreateTree(funcs, ref root);
                        response.Data = root.children;
                        response.Message = "加载完毕";
                        response.IsSuccess = true;
                    }

                    ts.Commit();

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserMenusGet;
                }
            }


        }
        void CreateTree(List<TKS_FAS_Function> data, ref TKS_FAS_MenuItem root)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var cur = data[i];
                if (cur.ParentId == root.id)
                {
                    var child = Trans2MenuItem(cur);

                    root.children.Add(child);
                    CreateTree(data, ref child);
                }
            }
        }

        TKS_FAS_MenuItem Trans2MenuItem(TKS_FAS_Function func)
        {
            TKS_FAS_MenuItem item = new TKS_FAS_MenuItem();
            item.id = func.Id;
            item.title = func.Name;
            item.icon = func.Img;
            item.href = func.URL;
            item.spread = false;
            item.children = new List<TKS_FAS_MenuItem>();
            item.children = new List<TKS_FAS_MenuItem>();
            return item;
        }

        /// <summary>
        /// 根据手机号，修改密码
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="pass"></param>
        public ResponseBase ModifyPass(string mobile, string pass)
        {
            var res = new ResponseBase();
            using (cnn = GetConnection())
            {
                try
                {
                    string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "MD5");

                    string sql = "update TKS_FAS_User set password=@password where mobile=@mobile";

                    cnn.Execute(sql, new { password = password, mobile = mobile });
                    res.IsSuccess = true;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    res.IsSuccess = false;
                    res.Message = "修改密码失败" + ex.Message;
                    return res;
                }
            }


        }

        public ResponseBase ModifyPassByUser(string token, string pass)
        {
            var res = new ResponseBase();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = UserInfoGet(token, ts);
                    string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pass, "MD5");

                    string sql = "update TKS_FAS_User set password=@password where id=@Id";

                    cnn.Execute(sql, new { password = password, Id = user.User.Id }, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    res.IsSuccess = false;
                    res.Message = "修改密码失败" + ex.Message;
                    return res;
                }
            }


        }
    }
}
