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

    public class UserController : ApiController
    {
        /// <summary>
        /// 用户列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("sso/userListSearch")]
        public ResponseUserListSearch UserListSearch([FromBody]RequestUserListSearch request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.UserListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("sso/userGet")]
        public ResponseUserGet UserGet([FromBody]RequestUserGet request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/userUpdate")]
        public ResponseUserUpdate UserUpdate([FromBody]RequestUserUpdate request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/userUpdate2")]
        public ResponseUserUpdate UserUpdate2([FromBody]RequestUserUpdate request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserUpdate2(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("sso/userAdd")]
        public ResponseUserAdd UserAdd([FromBody]RequestUserAdd request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/userDel")]
        public ResponseUserDelete UserDelete([FromBody]RequestUserDelete request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("sso/userRoleCheckedGet")]
        public ResponseUserRoleCheckedGet UserRoleCheckedGet([FromBody]RequestUserRoleCheckedGet request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserRoleCheckedGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        
        [Route("sso/userRoleCheckedUpdate")]
        public ResponseUserRoleCheckedUpdate UserRoleCheckedUpdate([FromBody]RequestUserRoleCheckedUpdate request)
        {
            try
            {
                UserBLL bll = new UserBLL();
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
