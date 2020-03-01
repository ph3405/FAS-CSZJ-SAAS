using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Entity.SSO;

namespace TKS.FAS.API.Controllers.SSO
{
    public class PrivilegeController : ApiController
    {
        [HttpPost]
        [Route("sso/userLogin")]
        public ResponseUserLogin UserLogin([FromBody]RequestUserLogin request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.UserLogin(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/SendMessage")]
        public ResponseAccountInvitationSend SendMessage([FromBody]RequestAccountInvitationSend request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.SendMessage(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/WXLogin")]
        public ResponseUserLogin_WX WXLogin([FromBody]RequestUserLogin request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.WXLogin(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/WX_CheckInvitation")]
        public ResponseBase WX_CheckInvitation([FromBody]RequestMyAccountAdd request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.WX_CheckInvitation(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/WX_GetAccountList")]
        public ResponseOPAccountListGet WX_GetAccountList([FromBody]RequestOPAccountListGet request)
        {
            try
            {

                UserBLL bll = new UserBLL();
                return bll.WX_GetAccountList(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/userCheck")]
        public ResponseUserLogin UserCheck([FromBody]RequestUserLogin request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserCheck(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [Route("sso/userMenuGet")]
        public ResponseUserMenusGet UserMenuGet([FromBody]RequestUserMenusGet request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.UserMenuGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/GetMenu")]
        public ResponseUserMenusGet GetMenu([FromBody]RequestUserMenusGet request)
        {
            try
            {
                UserBLL bll = new UserBLL();
                return bll.GetMenu(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
