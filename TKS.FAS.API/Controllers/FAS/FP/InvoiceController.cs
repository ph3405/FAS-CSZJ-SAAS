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
    public class InvoiceController : ApiController
    {
        [HttpPost]
        [Route("fas/fp/GZInvoiceListSearch")]
        public ResponseInvoiceListSearch GZInvoiceListSearch([FromBody]RequestInvoiceListSearch request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.GZInvoiceListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


        [HttpPost]
        [Route("fas/fp/SFInvoiceListSearch")]
        public ResponseSFInvoiceListSearch SFInvoiceListSearch([FromBody]RequestInvoiceListSearch request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.SFInvoiceListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/fp/SFWarnListSearch")]
        public ResponseSFInvoiceListSearch SFWarnListSearch([FromBody]RequestInvoiceListSearch request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.SFWarnListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/fp/SFNoTimeListSearch")]
        public ResponseSFInvoiceListSearch SFNoTimeListSearch([FromBody]RequestInvoiceListSearch request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.SFNoTimeListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


        [HttpPost]
        [Route("fas/fp/DZInvoiceListSearch")]
        public ResponseInvoiceListSearch DZInvoiceListSearch([FromBody]RequestInvoiceListSearch request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.DZInvoiceListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/fp/InvoiceGet")]
        public ResponseInvoiceGet InvoiceGet([FromBody]RequestInvoiceGet request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [Route("fas/fp/WX_InvoiceGet")]
        public ResponseInvoiceGet WX_InvoiceGet([FromBody]RequestInvoiceGet request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceUpdate")]
        public ResponseInvoiceUpdate InvoiceUpdate([FromBody]RequestInvoiceUpdate request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_InvoiceUpdate")]
        public ResponseInvoiceUpdate WX_InvoiceUpdate([FromBody]RequestInvoiceUpdate request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceAdd")]
        public ResponseInvoiceAdd InvoiceAdd([FromBody]RequestInvoiceAdd request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_InvoiceAdd")]
        public ResponseInvoiceAdd WX_InvoiceAdd([FromBody]RequestInvoiceAdd request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_InvoiceDelete")]
        public ResponseInvoiceDelete WX_InvoiceDelete([FromBody]RequestInvoiceDelete request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_SFInvoiceDelete")]
        public ResponseInvoiceDelete WX_SFInvoiceDelete([FromBody]RequestInvoiceDelete request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_SFInvoiceDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_SFWarnDelete")]
        public ResponseInvoiceAdd WX_SFWarnDelete([FromBody]RequestInvoiceAdd request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_SFWarnDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_InvoiceDJ")]
        public ResponseInvoiceDJ WX_InvoiceDJ([FromBody]RequestInvoiceDJ request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceDJ(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/fp/InvoiceDel")]
        public ResponseInvoiceDelete InvoiceDelete([FromBody]RequestInvoiceDelete request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceDJ")]
        public ResponseInvoiceDJ InvoiceDJ([FromBody]RequestInvoiceDJ request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceDJ(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/WX_InvoiceCX")]
        public ResponseInvoiceDJ WX_InvoiceCX([FromBody]RequestInvoiceDJ request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.WX_InvoiceCX(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceCX")]
        public ResponseInvoiceCX InvoiceCX([FromBody]RequestInvoiceCX request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceCX(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceLZ")]
        public ResponseInvoiceLZ InvoiceLZ([FromBody]RequestInvoiceLZ request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.InvoiceLZ(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/fp/InvoiceDataGet")]
        public ResponseInvoiceDataGet InvoiceDataGet([FromBody]RequestInvoiceDataGet request)
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.DataGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/fp/UploadFile")]
        public ResponseInvoiceCX UploadFile()
        {
            try
            {
                InvoiceBLL bll = new InvoiceBLL();
                return bll.UploadFile();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    }
}
