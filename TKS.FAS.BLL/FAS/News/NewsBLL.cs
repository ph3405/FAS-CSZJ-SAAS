using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Common;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using Dapper;
using DapperExtensions;
using System.Text.RegularExpressions;

namespace TKS.FAS.BLL.FAS
{
    public class NewsBLL : CommonBase
    {
        public ResponseNewsSearch NewsSearch(RequestNewsSearch request)
        {

            ResponseNewsSearch response = new ResponseNewsSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_News where type=@Type and title like @Title ",
                        " published_at desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_News> data = cnn.Query<TKS_FAS_News>(sql,
                        new
                        {
                            Title="%"+request.Title+"%",
                            Type= request.Type
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_News where type=@Type and title like @Title  ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        Title = "%" + request.Title + "%",
                        Type = request.Type
                    }, ts).ToString());

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = data;
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseNewsSearch;
                }
            }
        }

        public ResponseNewsSearch BookSearch(RequestNewsSearch request)
        {

            ResponseNewsSearch response = new ResponseNewsSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"select id,title,content from TKS_FAS_News where type=@Type and status=1 order by Sort ";
                    List<TKS_FAS_News> data = cnn.Query<TKS_FAS_News>(sql,
                        new
                        {
                            Type = request.Type
                        }, ts).ToList();

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";

                    response.Data = data;
        
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseNewsSearch;
                }
            }
        }
        public ResponseNewsPublish Publish(RequestNewsPublish request)
        {
            var res = new ResponseNewsPublish();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = "update tks_fas_news set status=1 ,published_at=@published_at where id=@Id";

                    cnn.Execute(sql, new { published_at=DateTime.Now,Id=request.Id });

                    res.IsSuccess = true;
                    res.Message = "发布成功";
                    return res;
                }
                catch(Exception ex)
                {
                    return this.DealException(res, ex) as ResponseNewsPublish;
                }
            }
        }

        public ResponseNewsUnPublish UnPublish(RequestNewsUnPublish request)
        {
            var res = new ResponseNewsUnPublish();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = "update tks_fas_news set status=0   where id=@Id";

                    cnn.Execute(sql, new {   Id = request.Id });

                    res.IsSuccess = true;
                    res.Message = "取消发布成功";
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponseNewsUnPublish;
                }
            }
        }

        public ResponseNewsDel Del(RequestNewsDel request)
        {
            var res = new ResponseNewsDel();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = "delete from  tks_fas_news   where id=@Id";

                    cnn.Execute(sql, new {   Id = request.Id });

                    res.IsSuccess = true;
                    res.Message = "删除成功";
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponseNewsDel;
                }
            }
        }

        public List<TKS_FAS_News> Top10()
        {
            List<TKS_FAS_News> res = new List<TKS_FAS_News>();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = @"select top 10 * from tks_fas_news order by published_at desc";

                    var data = cnn.Query<TKS_FAS_News>(sql).ToList();
                   
                    foreach (var item in data)
                    {
                        string content = item.content;
                        Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
                        //新建一个matches的MatchCollection对象 保存 匹配对象个数(img标签)  
                        MatchCollection matches = regImg.Matches(content);
                        foreach (Match match in matches)
                        {
                            //获取所有Img的路径src,并保存到数组中  
                            item.cover = match.Groups["imgUrl"].Value;
                            break;
                        }
                    }
                    res = data;
                    return res;
                }
                catch(Exception ex)
                {
                    return res;
                }
            }
        }

        public TKS_FAS_News Get(string id)
        {
            var res = new TKS_FAS_News();
            using (this.cnn = GetConnection())
            {
                try
                {
                    string sql = @"select * from tks_fas_news where id=@Id";
                    var data = cnn.QueryFirstOrDefault<TKS_FAS_News>(sql, new { Id = id });

                    return data;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public TKS_FAS_News Save(TKS_FAS_News data, string token)
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(token, ts);

                    if (string.IsNullOrEmpty(data.id))
                    {
                        data.id= DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        data.post_id = DateTime.Now.ToString("yyyyMMddHHmmss");
                        data.created_at = DateTime.Now;
                        //data.type = "news";
                        data.author_name = user.User.TrueName;
                        if (data.status == 1)
                        {
                            data.published_at = data.created_at;
                        }
                        cnn.Insert<TKS_FAS_News>(data, ts);
                    }
                    else
                    {
                        string sql = @"update TKS_FAS_News set 
                            title=@Title,summary=@Summary,content=@Content,Sort=@Sort  where Id=@Id";
                        if (data.status == 1)
                        {
                            sql = @"update TKS_FAS_News set 
                            title=@Title,summary=@Summary,content=@Content,published_at=@published_at,Sort=@Sort where Id=@Id";
                            cnn.Execute(sql, new
                            {
                                Title = data.title,
                                Summary = data.summary,
                                Content = data.content,
                                published_at = DateTime.Now,
                                Id = data.id,
                                Sort= data.Sort
                            }, ts);
                        }
                        else
                        {
                            cnn.Execute(sql, new
                            {
                                Title = data.title,
                                Summary = data.summary,
                                Content = data.content,
                                Id = data.id,
                                Sort = data.Sort
                            }, ts);
                        }
                        
                    }

                    ts.Commit();
                    return data;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }
            }
        }
    }
}
