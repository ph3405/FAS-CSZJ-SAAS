using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.API.Controllers.FAS.Doc
{
    public class DocController : ApiController
    {
        [HttpPost]
        [Route("fas/doc/DocListSearch")]
        public ResponseDocListSearch DocListSearch([FromBody]RequestDocListSearch request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/doc/GetDocByAccount")]
        public ResponseDocListSearch GetDocByAccount([FromBody]RequestDocListSearch request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.GetDocByAccount(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/doc/DocAttachment")]
        public ResponseDocListAttachment DocAttachment([FromBody]RequestDocListSearch request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocAttachment(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/doc/DocGet")]
        public ResponseDocGet DocGet([FromBody]RequestDocGet request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/doc/DocUpdate")]
        public ResponseDocUpdate DocUpdate([FromBody]RequestDocUpdate request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/doc/DocAdd")]
        public ResponseDocAdd DocAdd([FromBody]RequestDocAdd request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/doc/DocDel")]
        public ResponseDocDelete DocDelete([FromBody]RequestDocDelete request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/doc/DocAudit")]
        public ResponseDocAudit DocAudit([FromBody]RequestDocAudit request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocAudit(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/doc/DocUnAudit")]
        public ResponseDocUnAudit DocUnAudit([FromBody]RequestDocUnAudit request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.DocUnAudit(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/doc/GetNextDocNO")]
        public ResponseDocNOGet GetNextDocNO([FromBody]RequestDocNOGet request)
        {
            try
            {
                DocBLL bll = new DocBLL();
                return bll.GetNextDocNO(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
