using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Entity;

namespace TKS.FAS.API.Controllers.FAS.News
{
    public class NewsController : ApiController
    {
        [HttpPost]
        [Route("fas/set/NewsSearch")]
        public ResponseNewsSearch Search([FromBody]RequestNewsSearch request)
        {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.NewsSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/BookSearch")]
        public ResponseNewsSearch BookSearch([FromBody]RequestNewsSearch request)
        {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.BookSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/NewsPublish")]
        public ResponseNewsPublish Publish([FromBody]RequestNewsPublish request)
        {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.Publish(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/NewsUnPublish")]
        public ResponseNewsUnPublish UnPublish([FromBody]RequestNewsUnPublish request)
        {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.UnPublish(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/NewsDel")]
        public ResponseNewsDel Del([FromBody]RequestNewsDel request)
        {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.Del(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpGet]
        [Route("fas/set/newsTop10")]
        public List<TKS_FAS_News> Top10() {
            try
            {
                NewsBLL bll = new NewsBLL();
                return bll.Top10();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
