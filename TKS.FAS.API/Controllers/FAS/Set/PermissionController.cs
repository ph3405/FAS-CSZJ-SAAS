using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.SSO;
using TKS.FAS.BLL;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.API.Controllers.SSO
{

    public class PermissionController : ApiController
    {
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/set/userListSearch")]
        public ResponseUserListSearch UserListSearch([FromBody]RequestUserListSearch request)
        {
            try
            {

                PermissionBLL bll = new PermissionBLL();
                return bll.UserListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/userGet")]
        public ResponseUserGet UserGet([FromBody]RequestUserGet request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/userUpdate")]
        public ResponseUserUpdate UserUpdate([FromBody]RequestUserUpdate request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/userAdd")]
        public ResponseUserAdd UserAdd([FromBody]RequestUserAdd request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/userDel")]
        public ResponseUserDelete UserDelete([FromBody]RequestUserDelete request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/userRoleCheckedGet")]
        public ResponseUserRoleCheckedGet UserRoleCheckedGet([FromBody]RequestUserRoleCheckedGet request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserRoleCheckedGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        
        [Route("fas/set/userRoleCheckedUpdate")]
        public ResponseUserRoleCheckedUpdate UserRoleCheckedUpdate([FromBody]RequestUserRoleCheckedUpdate request)
        {
            try
            {
                PermissionBLL bll = new PermissionBLL();
                return bll.UserRoleCheckedUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
