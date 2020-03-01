using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;


namespace TKS.FAS.API.Controllers.FAS.FPtplanage
{
    public class FPtplController : ApiController
    {
        [HttpPost]
        [Route("fas/tplanage/FPtplListSearch")]
        public ResponseFPtplListSearch FPtplListSearch([FromBody]RequestFPtplListSearch request)
        {
            try
            {
                FPtplBLL bll = new FPtplBLL();
                return bll.FPtplListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/tplmanage/FPtplGet")]
        public ResponseFPtplGet FPtplGet([FromBody]RequestFPtplGet request)
        {
            try
            {
                FPtplBLL bll = new FPtplBLL();
                return bll.FPtplGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/FPtplUpdate")]
        public ResponseFPtplUpdate FPtplUpdate([FromBody]RequestFPtplUpdate request)
        {
            try
            {
                FPtplBLL bll = new FPtplBLL();
                return bll.FPtplUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/FPtplAdd")]
        public ResponseFPtplAdd FPtplAdd([FromBody]RequestFPtplAdd request)
        {
            try
            {
                FPtplBLL bll = new FPtplBLL();
                return bll.FPtplAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/FPtplDel")]
        public ResponseFPtplDelete FPtplDelete([FromBody]RequestFPtplDelete request)
        {
            try
            {
                FPtplBLL bll = new FPtplBLL();
                return bll.FPtplDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
