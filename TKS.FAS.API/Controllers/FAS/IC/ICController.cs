using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Entity;

namespace TKS.FAS.API.Controllers 
{
    public class ICController : ApiController
    {
        [HttpPost]
        [Route("fas/ic/ICRegisterListSearch")]
        public ResponseICRegisterListSearch  Search([FromBody]RequestICRegisterListSearch request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.Search(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


        [HttpPost]
        [Route("fas/ic/ICRegisterGetByUser")]
        public ResponseICRegisterGetByUser GetByUser([FromBody]RequestICRegisterGetByUser request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.GetByUser(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/ic/ICRegisterAdd")]
        public ResponseICRegisterAdd Add([FromBody]RequestICRegisterAdd request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/ic/ICRegisterAccept")]
        public ResponseICRegisterAccept Accept([FromBody]RequestICRegisterAccept request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterAccept(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/ic/ICRegisterPrePay")]
        public ResponseICRegisterPrePay PrePay([FromBody]RequestICRegisterPrePay request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterPrePay(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/ic/ICRegisterRegFinish")]
        public ResponseICRegisterRegFinish RegFinish([FromBody]RequestICRegisterRegFinish request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterRegFinish(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


        [HttpPost]
        [Route("fas/ic/ICRegisterPayAll")]
        public ResponseICRegisterPayAll PayAll([FromBody]RequestICRegisterPayAll request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterPayAll(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


        [HttpPost]
        [Route("fas/ic/ICRegisterEnd")]
        public ResponseICRegisterEnd End([FromBody]RequestICRegisterEnd request)
        {
            try
            {
                ICRegisterBLL bll = new ICRegisterBLL();
                return bll.ICRegisterEnd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
    }
}
