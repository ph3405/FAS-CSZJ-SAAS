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
    public class RoleController : ApiController
    {
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sso/roleListSearch")]
        public ResponseRoleListSearch RoleListSearch([FromBody]RequestRoleListSearch request)
        {
            try
            {
                RoleBLL bll = new RoleBLL();
                return bll.RoleListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("sso/roleGet")]
        public ResponseRoleGet RoleGet([FromBody]RequestRoleGet request)
        {
            try
            {
                RoleBLL bll = new RoleBLL();
                return bll.RoleGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/roleUpdate")]
        public ResponseRoleUpdate RoleUpdate([FromBody]RequestRoleUpdate request)
        {
            try
            {
                RoleBLL bll = new RoleBLL();
                return bll.RoleUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/roleAdd")]
        public ResponseRoleAdd RoleAdd([FromBody]RequestRoleAdd request)
        {
            try
            {
                RoleBLL bll = new RoleBLL();
                return bll.RoleAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/roleDel")]
        public ResponseRoleDelete RoleDelete([FromBody]RequestRoleDelete request)
        {
            try
            {
                RoleBLL bll = new RoleBLL();
                return bll.RoleDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
