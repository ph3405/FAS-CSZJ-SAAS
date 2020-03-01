using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;


namespace TKS.FAS.API.Controllers.FAS.TPLManage
{
    public class TPLMController : ApiController
    {
        [HttpPost]
        [Route("fas/tplManage/TPLMListSearch")]
        public ResponseTPLMListSearch TPLMListSearch([FromBody]RequestTPLMListSearch request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/tplManage/TPLMGet")]
        public ResponseTPLMGet TPLMGet([FromBody]RequestTPLMGet request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/tplManage/TPLMGet2")]
        public ResponseTPLMGet TPLMGet2([FromBody]RequestTPLMGet request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLMGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplManage/TPLMUpdate")]
        public ResponseTPLMUpdate TPLMUpdate([FromBody]RequestTPLMUpdate request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplManage/TPLMUpdate2")]
        public ResponseTPLMUpdate TPLMUpdate2([FromBody]RequestTPLMUpdate request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLMUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplManage/TPLMAdd")]
        public ResponseTPLMAdd TPLMAdd([FromBody]RequestTPLMAdd request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplManage/TPLMAdd2")]
        public ResponseTPLMAdd TPLMAdd2([FromBody]RequestTPLMAdd request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLMAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/tplManage/TPLMDel")]
        public ResponseTPLMDelete TPLMDelete([FromBody]RequestTPLMDelete request)
        {
            try
            {
                TPLMBLL bll = new TPLMBLL();
                return bll.TPLDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
