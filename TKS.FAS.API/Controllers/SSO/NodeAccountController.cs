using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL;
using TKS.FAS.Entity.SSO;

namespace TKS.FAS.API.Controllers.SSO
{
    public class NodeAccountController : ApiController
    {

        /// <summary>
        /// 流水明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sso/NodeAccountDetailSearch")]
        public ResponseNodeAccountDetailSearch NodeAccountDetailSearch([FromBody]RequestNodeAccountDetailSearch request)
        {
            try
            {
                NodeAccountAccountBLL bll = new NodeAccountAccountBLL();
                return bll.NodeAccountListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        /// <summary>
        /// 账户明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sso/NodeAccountGet")]
        public ResponseNodeAccountGet NodeAccountGet([FromBody]RequestNodeAccountGet request)
        {
            try
            {
                NodeAccountAccountBLL bll = new NodeAccountAccountBLL();
                return bll.NodeAccountGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


    }
}
