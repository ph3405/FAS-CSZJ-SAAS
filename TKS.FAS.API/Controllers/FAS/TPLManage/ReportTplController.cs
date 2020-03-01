using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.BBtplanage
{
    public class ReportTplController : ApiController
    {
        [HttpPost]
        [Route("fas/tplmanage/BBtplListSearch")]
        public ResponseBBtplListSearch BBtplListSearch([FromBody]RequestBBtplListSearch request)
        {
            try
            {
                 ReportTplBLL bll= new ReportTplBLL();
                return bll.BBtplListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/tplmanage/BBtplGet")]
        public ResponseBBtplGet BBtplGet([FromBody]RequestBBtplGet request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBtplGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBtplUpdate")]
        public ResponseBBtplUpdate BBtplUpdate([FromBody]RequestBBtplUpdate request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBtplUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBtplAdd")]
        public ResponseBBtplAdd BBtplAdd([FromBody]RequestBBtplAdd request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBtplAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBtplDel")]
        public ResponseBBtplDelete BBtplDelete([FromBody]RequestBBtplDelete request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBtplDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        [HttpPost]
        [Route("fas/tplmanage/BBDetailtplListSearch")]
        public ResponseBBDetailtplListSearch BBDetailtplListSearch([FromBody]RequestBBDetailtplListSearch request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBDetailtplListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/tplmanage/BBDetailtplGet")]
        public ResponseBBDetailtplGet BBDetailtplGet([FromBody]RequestBBDetailtplGet request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBDetailtplGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBDetailtplUpdate")]
        public ResponseBBDetailtplUpdate BBDetailtplUpdate([FromBody]RequestBBDetailtplUpdate request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBDetailtplUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBDetailtplAdd")]
        public ResponseBBDetailtplAdd BBDetailtplAdd([FromBody]RequestBBDetailtplAdd request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBDetailtplAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/BBDetailtplDel")]
        public ResponseBBDetailtplDelete BBDetailtplDelete([FromBody]RequestBBDetailtplDelete request)
        {
            try
            {
                ReportTplBLL bll = new ReportTplBLL();
                return bll.BBDetailtplDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


    }
}
