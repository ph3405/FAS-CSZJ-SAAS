using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.Period
{
    public class PeriodController : ApiController
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/period/PeriodPaidGet")]
        public ResponsePeriodPaidGet PeriodPaidGet([FromBody]RequestPeriodPaidGet request)
        {
            try
            {
                PeriodBLL bll = new PeriodBLL();
                return bll.PeriodPaidGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/period/WX_PeriodPaidGet")]
        public ResponsePeriodPaidGet WX_PeriodPaidGet([FromBody]RequestPeriodPaidGet request)
        {
            try
            {
                PeriodBLL bll = new PeriodBLL();
                return bll.WX_PeriodPaidGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/period/PeriodGet")]
        public ResponsePeriodPaidGet PeriodGet([FromBody]RequestPeriodPaidGet request)
        {
            try
            {
                PeriodBLL bll = new PeriodBLL();
                return bll.PeriodGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

    }
}
