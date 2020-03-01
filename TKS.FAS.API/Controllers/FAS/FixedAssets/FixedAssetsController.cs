using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS 
{
    public class FixedAssetsController : ApiController
    {
        [HttpPost]
        [Route("fas/FixedAssets/FixedAssetsListSearch")]
        public ResponseFixedAssetsListSearch FixedAssetsListSearch([FromBody]RequestFixedAssetsListSearch request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/FixedAssets/FixedAssetsChangeListSearch")]
        public ResponseFixedAssetsChangeListSearch FixedAssetsChangeListSearch([FromBody]RequestFixedAssetsChangeListSearch request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsChangeListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/FixedAssets/FixedAssetsGet")]
        public ResponseFixedAssetsGet FixedAssetsGet([FromBody]RequestFixedAssetsGet request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/FixedAssets/FixedAssetsUpdate")]
        public ResponseFixedAssetsUpdate FixedAssetsUpdate([FromBody]RequestFixedAssetsUpdate request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/FixedAssets/FixedAssetsAdd")]
        public ResponseFixedAssetsAdd FixedAssetsAdd([FromBody]RequestFixedAssetsAdd request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/FixedAssets/FixedAssetsDel")]
        public ResponseFixedAssetsDelete FixedAssetsDelete([FromBody]RequestFixedAssetsDelete request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/FixedAssets/FixedAssetsDeal")]
        public ResponseFixedAssetsDeal FixedAssetsDeal([FromBody]RequestFixedAssetsDeal request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.FixedAssetsDeal(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/FixedAssets/IsGenPZ")]
        public ResponseFixedAssetsIsGenPZ IsGenPZ([FromBody]RequestFixedAssetsIsGenPZ request)
        {
            try
            {
                FixedAssetsBLL bll = new FixedAssetsBLL();
                return bll.IsGenPZ(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
