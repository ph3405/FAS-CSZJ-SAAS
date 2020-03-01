using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.API.Controllers.FAS.OCR
{
    public class OCRController : ApiController
    {
        [HttpPost]
        [Route("fas/ocr/GetOcrImgInfo")]
        public ResponseOcrInfo GetOcrImgInfo([FromBody]RequestOcrInfo request)
        {
            try
            {
                OcrBll bll = new OcrBll();
                return bll.GetResponseOcrInfo(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }




            
        }
    }
}
