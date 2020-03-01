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
    public class TPLController : ApiController
    {
        [HttpPost]
        [Route("fas/set/TPLListSearch")]
        public ResponseTPLListSearch TPLListSearch([FromBody]RequestTPLListSearch request)
        {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.TPLListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/TPLGet")]
        public ResponseTPLGet TPLGet([FromBody]RequestTPLGet request)
        {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.TPLGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/TPLUpdate")]
        public ResponseTPLUpdate TPLUpdate([FromBody]RequestTPLUpdate request)
        {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.TPLUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/TPLAdd")]
        public ResponseTPLAdd TPLAdd([FromBody]RequestTPLAdd request)
        {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.TPLAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/TPLDel")]
        public ResponseTPLDelete TPLDelete([FromBody]RequestTPLDelete request)
        {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.TPLDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/FPTPLSearch")]
        public ResponseFPtplListSearch FPTplSearch([FromBody]RequestFPtplListSearch request) {
            try
            {
                TPLBLL bll = new TPLBLL();
                return bll.FPtplListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
