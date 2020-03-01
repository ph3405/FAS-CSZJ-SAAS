using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.SSO;
using TKS.FAS.BLL;

namespace TKS.FAS.API.Controllers.SSO
{
    public class NodeController : ApiController
    {
        /// <summary>
        /// 机构列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sso/nodeListSearch")]
        public ResponseNodeListSearch NodeListSearch([FromBody]RequestNodeListSearch request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.NodeListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("sso/supplierListSearch")]
        public ResponseNodeListSearch SupplierListSearch([FromBody]RequestNodeListSearch request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.SupplierSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("sso/NodeGet")]
        public ResponseNodeGet NodeGet([FromBody]RequestNodeGet request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.NodeGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/NodeUpdate")]
        public ResponseNodeUpdate NodeUpdate([FromBody]RequestNodeUpdate request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.NodeUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/NodeAdd")]
        public ResponseNodeAdd NodeAdd([FromBody]RequestNodeAdd request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.NodeAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/NodeDel")]
        public ResponseNodeDelete NodeDelete([FromBody]RequestNodeDelete request)
        {
            try
            {
                NodeBLL bll = new NodeBLL();
                return bll.NodeDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
