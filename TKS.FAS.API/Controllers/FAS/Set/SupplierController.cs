using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.BLL.FAS.Set;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.Supplier
{
    public class SupplierController : ApiController
    {

        [HttpPost]
        [Route("fas/set/SupplierListSearch")]
        public ResponseSupplierListSearch SupplierListSearch([FromBody]RequestSupplierListSearch request)
        {
            try
            {
                SupplierBLL bll = new SupplierBLL();
                return bll.SupplierListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/SupplierGet")]
        public ResponseSupplierGet SupplierGet([FromBody]RequestSupplierGet request)
        {
            try
            {
                SupplierBLL bll = new SupplierBLL();
                return bll.SupplierGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SupplierUpdate")]
        public ResponseSupplierUpdate SupplierUpdate([FromBody]RequestSupplierUpdate request)
        {
            try
            {
                SupplierBLL bll = new SupplierBLL();
                return bll.SupplierUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SupplierAdd")]
        public ResponseSupplierAdd SupplierAdd([FromBody]RequestSupplierAdd request)
        {
            try
            {
                SupplierBLL bll = new SupplierBLL();
                return bll.SupplierAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SupplierDel")]
        public ResponseSupplierDelete SupplierDelete([FromBody]RequestSupplierDelete request)
        {
            try
            {
                SupplierBLL bll = new SupplierBLL();
                return bll.SupplierDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
