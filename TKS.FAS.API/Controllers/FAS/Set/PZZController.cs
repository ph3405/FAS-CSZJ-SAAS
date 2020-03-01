using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class PZZController : ApiController
    {
        [HttpPost]
        [Route("fas/set/PZZListSearch")]
        public ResponsePZZListSearch PZZListSearch([FromBody]RequestPZZListSearch request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/PZZGet")]
        public ResponsePZZGet PZZGet([FromBody]RequestPZZGet request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/PZZUpdate")]
        public ResponsePZZUpdate PZZUpdate([FromBody]RequestPZZUpdate request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/PZZAdd")]
        public ResponsePZZAdd PZZAdd([FromBody]RequestPZZAdd request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/PZZDel")]
        public ResponsePZZDelete PZZDelete([FromBody]RequestPZZDelete request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/PZZTotalGet")]
        public ResponsePZZTotalGet PZZTotalGet([FromBody]RequestPZZTotalGet request)
        {
            try
            {
                PZZBLL bll = new PZZBLL();
                return bll.PZZTotalGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


    }
}
