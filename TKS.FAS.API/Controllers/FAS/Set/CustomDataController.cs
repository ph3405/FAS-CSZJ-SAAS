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
    public class CustomDataController : ApiController
    {


        [Route("fas/set/AddUIInit")]
        public ResponseAddUIInit SearchUIInit([FromBody]RequestAddUIInit request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.AddUIInit(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/set/SearchUIInit")]
        public ResponseSearchUIInit SearchUIInit([FromBody]RequestSearchUIInit request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.SearchUIInit(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/set/CustomDataListSearch")]
        public ResponseCustomDataListSearch CustomDataListSearch([FromBody]RequestCustomDataListSearch request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.CustomDataListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/CustomDataGet")]
        public ResponseCustomDataGet CustomDataGet([FromBody]RequestCustomDataGet request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.CustomDataGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CustomDataUpdate")]
        public ResponseCustomDataUpdate CustomDataUpdate([FromBody]RequestCustomDataUpdate request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.CustomDataUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CustomDataAdd")]
        public ResponseCustomDataAdd CustomDataAdd([FromBody]RequestCustomDataAdd request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.CustomDataAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/CustomDataDel")]
        public ResponseCustomDataDelete CustomDataDelete([FromBody]RequestCustomDataDelete request)
        {
            try
            {
                CustomDataBLL bll = new CustomDataBLL();
                return bll.CustomDataDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
