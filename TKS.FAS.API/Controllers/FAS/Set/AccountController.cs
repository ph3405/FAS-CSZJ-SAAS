using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class AccountController : ApiController
    {
        /// <summary>
        /// account列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/set/accountListSearch")]
        public ResponseAccountListSearch AccountListSearch([FromBody]RequestAccountListSearch request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        /// <summary>
        /// 备份列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/set/BackUpSearch")]
        public ResponseBackUpSearch BackUpSearch([FromBody]RequestBackUpSearch request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.BackUpSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/set/accountGet")]
        public ResponseAccountGet AccountGet([FromBody]RequestAccountGet request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/accountUpdate")]
        public ResponseAccountUpdate AccountUpdate([FromBody]RequestAccountUpdate request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/accountAdd")]
        public ResponseAccountAdd AccountAdd([FromBody]RequestAccountAdd request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/BackUp")]
        public ResponseAccountAdd BackUp([FromBody]RequestAccountAdd request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.BackUp(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/BackUpDelete")]
        public ResponseAccountDelete BackUpDelete([FromBody]RequestAccountDelete request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.BackUpDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/BackUpDownload")]
        public ResponseAccountDelete BackUpDownload([FromBody]RequestAccountDelete request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.BackUpDownload(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/BackUpHuiFu")]
        public ResponseAccountDelete BackUpHuiFu([FromBody]RequestAccountDelete request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.BackUpHuiFu(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/accountDel")]
        public ResponseAccountDelete AccountDelete([FromBody]RequestAccountDelete request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/accountActive")]
        public ResponseAccountActive AccountActive([FromBody]RequestAccountActive request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountActive(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/opAccountListGet")]
        public ResponseOPAccountListGet OPAccountListGet([FromBody]RequestOPAccountListGet request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.OPAccountListGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        #region 外包

        [Route("fas/set/AccountOutSource")]
        public ResponseAccountOutSource AccountOutSource([FromBody]RequestAccountOutSource request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountOutSource(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/set/AccountUnOut")]
        public ResponseAccountUnOut AccountOutSource([FromBody]RequestAccountUnOut request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountUnOut(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [Route("fas/set/AccountAcceptOut")]
        public ResponseAccountAcceptOut AccountAcceptOut([FromBody]RequestAccountAcceptOut request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountAcceptOut(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/AccountRejectOut")]
        public ResponseAccountRejectOut AccountAcceptOut([FromBody]RequestAccountRejectOut request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.AccountRejectOut(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/InvitationCodeGet")]
        public ResponseAccountInvitationCodeGet InvitationCodeGet([FromBody]RequestAccountInvitationCodeGet request)
        {
            try
            {
                AccountSetBLL bll = new AccountSetBLL();
                return bll.InvitationCodeGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }



        #endregion
    }
}
