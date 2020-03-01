using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;
namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class CalHelperController : ApiController
    {
        [HttpPost]
        [Route("fas/set/CalHelperListSearch")]
        public ResponseCalHelperListSearch CalHelperListSearch([FromBody]RequestCalHelperListSearch request)
        {
            try
            {
                CalHelperBLL bll = new CalHelperBLL();
                return bll.CalHelperListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/CalHelperGet")]
        public ResponseCalHelperGet CalHelperGet([FromBody]RequestCalHelperGet request)
        {
            try
            {
                CalHelperBLL bll = new CalHelperBLL();
                return bll.CalHelperGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CalHelperUpdate")]
        public ResponseCalHelperUpdate CalHelperUpdate([FromBody]RequestCalHelperUpdate request)
        {
            try
            {
                CalHelperBLL bll = new CalHelperBLL();
                return bll.CalHelperUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CalHelperAdd")]
        public ResponseCalHelperAdd CalHelperAdd([FromBody]RequestCalHelperAdd request)
        {
            try
            {
                CalHelperBLL bll = new CalHelperBLL();
                return bll.CalHelperAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CalHelperDel")]
        public ResponseCalHelperDelete CalHelperDelete([FromBody]RequestCalHelperDelete request)
        {
            try
            {
                CalHelperBLL bll = new CalHelperBLL();
                return bll.CalHelperDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
