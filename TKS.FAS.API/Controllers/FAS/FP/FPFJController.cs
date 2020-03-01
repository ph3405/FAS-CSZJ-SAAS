using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.FPFJ
{
    public class FPFJController : ApiController
    {
        [HttpPost]
        [Route("fas/fp/FPFJListSearch")]
        public ResponseFPFJListSearch FPFJListSearch([FromBody]RequestFPFJListSearch request)
        {
            try
            {
                FPFJBLL bll = new FPFJBLL();
                return bll.FPFJListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/fp/FPFJGet")]
        public ResponseFPFJGet FPFJGet([FromBody]RequestFPFJGet request)
        {
            try
            {
                FPFJBLL bll = new FPFJBLL();
                return bll.FPFJGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/FPFJDel")]
        public ResponseFPFJDelete FPFJDelete([FromBody]RequestFPFJDelete request)
        {
            try
            {
                FPFJBLL bll = new FPFJBLL();
                return bll.FPFJDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
