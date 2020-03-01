using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TKS.FAS.Entity;
using Dapper;
using DapperExtensions;
using System.IO;
using System.Net;

namespace TKS.FAS.Common
{
    public abstract class CommonBase
    {
        protected string CnnString { get; set; }

        protected bool IsDebug { get; set; }

        private IDbConnection _cnn;
        protected IDbConnection cnn
        {
            get { return _cnn; }
            set { _cnn = value; }
        }



        public CommonBase()
        {
            CnnString = ConfigHelper.Read("ConnString");
            IsDebug = ConfigHelper.Read("debug") == "1" ? true : false;
        }

        protected IDbConnection GetConnection()
        {

            //暂支持MsSql
            //此处预留对其它DB的支持
            try
            {
                _cnn = new SqlConnection(CnnString);
                _cnn.Open();
            }
            catch (Exception ex)
            {

            }
            return _cnn;
        }

        /// <summary>
        /// 简单拼装分页sql for MsSql
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="table">表</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="currentIndex">当前页</param>
        /// <returns></returns>
        protected string GetPageSql(string fields, string table, string order, int pageSize, int currentIndex)
        {

            string pgSql = @"SELECT   * FROM ( SELECT TOP " + currentIndex * pageSize + @"
            row_number() OVER (ORDER BY " + order + @") n," + fields + @" FROM " + table + @") w2 
                WHERE   w2.n > " + (currentIndex - 1) * pageSize + " ORDER BY w2.n ASC ";

            return pgSql;
        }
        protected string GetPageSql(string fields, string table, string order)
        {

            string pgSql = @"SELECT   * FROM ( SELECT 
            row_number() OVER (ORDER BY " + order + @") n," + fields + @" FROM " + table + @") w2 
                 ORDER BY w2.n ASC ";

            return pgSql;
        }
        protected string GetPageSql_KM(string fields, string table, string order, int pageSize, int currentIndex)
        {

            string pgSql = @"SELECT   * FROM ( SELECT TOP " + currentIndex * pageSize + @"
            row_number() OVER (ORDER BY " + order + @") n," + fields + @" FROM " + table + @") w2 
                WHERE   w2.n > " + (currentIndex - 1) * pageSize + " ORDER BY w2.n DESC ";

            return pgSql;
        }

        /// <summary>
        /// 统一异常处理
        /// </summary>
        /// <param name="response"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected ResponseBase DealException(ResponseBase response, Exception ex)
        {

            if (typeof(AppException) == ex.GetType())
            {
                var e = ex as AppException;

                response.Message = IsDebug ? e.Error : e.ShowError + ",错误代码[" + e.GUID + "]";
            }
            else if (typeof(NormalException) == ex.GetType())
            {
                response.Message = ex.Message;
            }
            else
            {

                response.Message = ex.Message;
            }
            response.IsSuccess = false;
            return response;
        }

        protected MM_UserInfo UserInfoGetButAccount(string token, IDbTransaction ts)
        {
            #region timeout
            string sql = "select * from tks_fas_token where token=@Token";

            var tokens = cnn.Query<TKS_FAS_Token>(sql, new { Token = token }, ts).ToList();

            if (tokens.Count() == 0)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "登陆超时，请重新登陆");
            }
            TKS_FAS_Token curToken = tokens[0];
            DateTime now = DateTime.Now;

            //var interval = now - DateTime.Parse(curToken.ActiveTime.ToString());
            //日期时间格式化 修改于2018-03-14 Hero.Zhang
            string strDate = (curToken.ActiveTime ?? DateTime.MinValue).ToString("yyyy/MM/dd HH:mm:ss");
            TimeSpan interval = now - DateTime.Parse(strDate);
            var sec = interval.TotalSeconds;
            var timeOut = double.Parse(ConfigHelper.Read("timeOut"));
            if (timeOut < sec)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "token 超时" + sec + "秒");
            }

            sql = @"update tks_fas_token set ActiveTime=@ActiveTime where token=@Token";
            cnn.Execute(sql, new { ActiveTime = DateTime.Now, Token = token }, ts);

            #endregion

            var user = cnn.Query<TKS_FAS_User>("select * from tks_fas_user where id=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();
            if (user.Count == 0)
            {
                throw new AppException(curToken.UserId, "UserInfoGet",
                   "用户信息异常，请联系管理员", "当前token没有对应的用户信息" + curToken.UserId);
            }
            var node = cnn.Query<TKS_FAS_Node>(@"select A.* from tks_fas_node A left 
                    join tks_fas_user2Node B on A.id=B.nodeId where B.userid=@UserId",
                    new { UserId = curToken.UserId }, ts).ToList();
            if (node.Count() > 1)
            {
                throw new AppException(curToken.UserId, "UserInfoGet",
                    "用户的机构信息异常，请联系管理员", "用户属于多个机构");
            }

            var roles = cnn.Query<TKS_FAS_Role>(@"select A.* from TKS_FAS_Role A left join 
                            TKS_FAS_User2Role B on A.id=B.roleId where  B.userid=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();


            MM_UserInfo res = new MM_UserInfo();


            res.User = user[0];
            res.Node = node.Count > 0 ? node[0] : null;
            res.Roles = roles;
            return res;

        }

        protected MM_UserInfo UserInfoGet(string token, IDbTransaction ts)
        {
            #region timeout
            string sql = "select * from tks_fas_token where token=@Token";

            var tokens = cnn.Query<TKS_FAS_Token>(sql, new { Token = token }, ts).ToList();

            if (tokens.Count() == 0)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "登陆超时，请重新登陆");
            }
            TKS_FAS_Token curToken = tokens[0];
            DateTime now = DateTime.Now;

            var interval = now - DateTime.Parse(curToken.ActiveTime.ToString());
            var sec = interval.TotalSeconds;
            var timeOut = double.Parse(ConfigHelper.Read("timeOut"));
            if (timeOut < sec)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "token 超时" + sec + "秒");
            }

            sql = @"update tks_fas_token set ActiveTime=@ActiveTime where token=@Token";
            cnn.Execute(sql, new { ActiveTime = DateTime.Now, Token = token }, ts);

            #endregion

            var user = cnn.Query<TKS_FAS_User>("select * from tks_fas_user where id=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();

            var node = cnn.Query<TKS_FAS_Node>(@"select A.* from tks_fas_node A left 
                    join tks_fas_user2Node B on A.id=B.nodeId where B.userid=@UserId",
                    new { UserId = curToken.UserId }, ts).ToList();
            if (node.Count() > 1)
            {
                throw new AppException(curToken.UserId, "UserInfoGet",
                    "用户的机构信息异常，请联系管理员", "用户属于多个机构");
            }

            var roles = cnn.Query<TKS_FAS_Role>(@"select A.* from TKS_FAS_Role A left join 
                            TKS_FAS_User2Role B on A.id=B.roleId where   B.userid=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();

            var currentAccount = cnn.Query<TKS_FAS_UserCurrentAccount>(
                            @"select * from TKS_FAS_UserCurrentAccount where userId=@UserId",
                            new { UserId = curToken.UserId }, ts).ToList();
            MM_UserInfo res = new MM_UserInfo();
            if (currentAccount.Count() == 0)
            {
                //update by Hero.Zhang
                //throw new AppException(curToken.UserId,"","请选择账套", "请选择账套");
            }
            else
            {

                res.AccountId = currentAccount[0].AccountId;

            }


            res.User = user[0];
            res.Node = node.Count > 0 ? node[0] : null;
            res.Roles = roles;
            return res;

        }

        /// <summary>
        /// Http方式下载文件
        /// </summary>
        /// <param name="uploadPath">文件夹目录</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public bool Download(string uploadPath)
        {
            bool flag = false;
            //打开上次下载的文件
            long SPosition = 0;
            //实例化流对象
            FileStream FStream;
            //判断要下载的文件是否存在
            if (File.Exists(uploadPath))
            {
                //打开要下载的文件
                FStream = File.OpenWrite(uploadPath);
                //获取已经下载的长度
                SPosition = FStream.Length;
                FStream.Seek(SPosition, SeekOrigin.Current);
            }
            else
            {
                //文件不保存创建一个文件
                FStream = new FileStream(uploadPath, FileMode.Create);
                SPosition = 0;
            }
            try
            {
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(uploadPath);
                if (SPosition > 0)
                    myRequest.AddRange((int)SPosition);             //设置Range值
                //向服务器请求,获得服务器的回应数据流
                Stream myStream = myRequest.GetResponse().GetResponseStream();
                //定义一个字节数据
                byte[] btContent = new byte[512];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 512);
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 512);
                }
                //关闭流
                FStream.Close();
                myStream.Close();
                flag = true;        //返回true下载成功
            }
            catch (Exception)
            {
                FStream.Close();
                flag = false;       //返回false下载失败
            }
            return flag;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="token"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        protected TKS_FAS_PermissionInfo GetPermission(string token, IDbTransaction ts)
        {
            #region timeout
            string sql = "select * from tks_fas_token where token=@Token";

            var tokens = cnn.Query<TKS_FAS_Token>(sql, new { Token = token }, ts).ToList();

            if (tokens.Count() == 0)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "登陆超时，请重新登陆");
            }
            TKS_FAS_Token curToken = tokens[0];
            DateTime now = DateTime.Now;

            var interval = now - DateTime.Parse(curToken.ActiveTime.ToString());
            var sec = interval.TotalSeconds;
            var timeOut = double.Parse(ConfigHelper.Read("timeOut"));
            if (timeOut < sec)
            {
                //System.Web.HttpContext.Current.Session.Clear();
                //System.Web.HttpContext.Current.Response.Write("~/Login.aspx");
                throw new AppException("", "UserInfoGet", "登陆超时，请重新登陆", "token 超时" + sec + "秒");
            }

            sql = @"update tks_fas_token set ActiveTime=@ActiveTime where token=@Token";
            cnn.Execute(sql, new { ActiveTime = DateTime.Now, Token = token }, ts);

            #endregion
            var user = cnn.Query<TKS_FAS_User>("select * from tks_fas_user where id=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();
            if (user.Count == 0)
            {
                throw new AppException(curToken.UserId, "UserInfoGet",
                   "用户信息异常，请联系管理员", "当前token没有对应的用户信息" + curToken.UserId);
            }
            var node = cnn.Query<TKS_FAS_Node>(@"select A.* from tks_fas_node A left 
                    join tks_fas_user2Node B on A.id=B.nodeId where B.userid=@UserId",
                    new { UserId = curToken.UserId }, ts).ToList();
            if (node.Count() > 1)
            {
                throw new AppException(curToken.UserId, "UserInfoGet",
                    "用户的机构信息异常，请联系管理员", "用户属于多个机构");
            }
            var roles = cnn.Query<TKS_FAS_Role>(@"select A.* from TKS_FAS_Role A left join 
                            TKS_FAS_User2Role B on A.id=B.roleId where   B.userid=@UserId",
                new { UserId = curToken.UserId }, ts).ToList();


            TKS_FAS_PermissionInfo res = new TKS_FAS_PermissionInfo();
            if (roles.Count > 0)
            {
                string where = "(";
                foreach (var item in roles)
                {
                    if (where == "(")
                    {
                        where += "'" + item.Id + "'";
                    }
                    else
                    {
                        where += ",'" + item.Id + "'";
                    }
                }
                where += ")";
                sql = string.Format(@"select Permission,PLevel from TKS_FAS_Role2Permission where RoleId in{0} order by PLevel", where);
                var Permission = cnn.Query<TKS_FAS_PermissionInfo>(sql,
                null, ts).Distinct().ToList();
                if (Permission.Count > 0)
                {
                    res = Permission[0];
                }
                else
                {
                    res = null;
                }

            }
            return res;
        }

    }
}
