using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity;

namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class BasicDataController : ApiController
    {
        [HttpPost]
        [Route("fas/BasicData/BasicDataSearch")]
        public ResponseBasicDataSearch BasicDataSearch([FromBody]RequestBasicDataSearch request)
        {
            try
            {
                BasicDataBLL bll = new BasicDataBLL();
                return bll.BasicDataListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }



        [HttpPost]
        [Route("fas/BasicData/WX_BasicDataGet")]
        public ResponseBasicDataGet WX_BasicDataGet([FromBody]RequestInvoiceGet request)
        {
            try
            {
                BasicDataBLL bll = new BasicDataBLL();
                return bll.WX_BasicDataGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        [HttpPost]
        [Route("fas/BasicData/WX_BasicDataUpdate")]
        public ResponseInvoiceUpdate WX_InvoiceUpdate([FromBody]RequestBasicDataSearch request)
        {
            try
            {
                BasicDataBLL bll = new BasicDataBLL();
                return bll.WX_BasicDataUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        [HttpPost]
        [Route("fas/BasicData/WX_BasicDataAdd")]
        public ResponseInvoiceAdd WX_BasicDataAdd([FromBody]RequestBasicDataSearch request)
        {
            try
            {
                BasicDataBLL bll = new BasicDataBLL();
                return bll.WX_BasicDataAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/BasicData/WX_BasicDataDelete")]
        public ResponseInvoiceDelete WX_BasicDataDelete([FromBody]RequestInvoiceDelete request)
        {
            try
            {
                BasicDataBLL bll = new BasicDataBLL();
                return bll.WX_BasicDataDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
