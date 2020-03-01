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
    public class CurrencyController : ApiController
    {
        [HttpPost]
        [Route("fas/set/CurrencyListSearch")]
        public ResponseCurrencyListSearch CurrencyListSearch([FromBody]RequestCurrencyListSearch request)
        {
            try
            {
                CurrencyBLL bll = new CurrencyBLL();
                return bll.CurrencyListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/CurrencyGet")]
        public ResponseCurrencyGet CurrencyGet([FromBody]RequestCurrencyGet request)
        {
            try
            {
                CurrencyBLL bll = new CurrencyBLL();
                return bll.CurrencyGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CurrencyUpdate")]
        public ResponseCurrencyUpdate CurrencyUpdate([FromBody]RequestCurrencyUpdate request)
        {
            try
            {
                CurrencyBLL bll = new CurrencyBLL();
                return bll.CurrencyUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CurrencyAdd")]
        public ResponseCurrencyAdd CurrencyAdd([FromBody]RequestCurrencyAdd request)
        {
            try
            {
                CurrencyBLL bll = new CurrencyBLL();
                return bll.CurrencyAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CurrencyDel")]
        public ResponseCurrencyDelete CurrencyDelete([FromBody]RequestCurrencyDelete request)
        {
            try
            {
                CurrencyBLL bll = new CurrencyBLL();
                return bll.CurrencyDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
