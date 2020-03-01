using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.FJ
{
    public class FJController : ApiController
    {
        [HttpPost]
        [Route("fas/doc/FJListSearch")]
        public ResponseFJListSearch FJListSearch([FromBody]RequestFJListSearch request)
        {
            try
            {
                FJBLL bll = new FJBLL();
                return bll.FJListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/doc/FJGet")]
        public ResponseFJGet FJGet([FromBody]RequestFJGet request)
        {
            try
            {
                FJBLL bll = new FJBLL();
                return bll.FJGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

       
 

        [Route("fas/doc/FJDel")]
        public ResponseFJDelete FJDelete([FromBody]RequestFJDelete request)
        {
            try
            {
                FJBLL bll = new FJBLL();
                return bll.FJDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
