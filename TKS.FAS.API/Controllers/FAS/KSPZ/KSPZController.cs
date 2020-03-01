using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.KSPZ
{
    public class KSPZController : ApiController
    {
       
        /// <summary>
        /// 快速凭证模板获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/KSPZ/KSPZTPLsGet")]
        public ResponseKSPZTPLsGet KSPZCheckTPLsGet([FromBody]RequestKSPZTPLsGet request)
        {
            try
            {
                KSPZBLL bll = new KSPZBLL();
                return bll.KSPZTPLsGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

       
    }
}
