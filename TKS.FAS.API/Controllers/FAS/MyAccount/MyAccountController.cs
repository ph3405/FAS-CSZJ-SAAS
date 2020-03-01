using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.MyAccount
{
    public class MyAccountController : ApiController
    {
        [HttpPost]
        [Route("fas/set/MyAccountListSearch")]
        public ResponseMyAccountListSearch MyAccountListSearch([FromBody]RequestMyAccountListSearch request)
        {
            try
            {
                MyAccountBLL bll = new MyAccountBLL();
                return bll.MyAccountListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/MyAccountGet")]
        public ResponseMyAccountGet MyAccountGet([FromBody]RequestMyAccountGet request)
        {
            try
            {
                MyAccountBLL bll = new MyAccountBLL();
                return bll.MyAccountGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/MyAccountAdd")]
        public ResponseMyAccountAdd MyAccountAdd([FromBody]RequestMyAccountAdd request)
        {
            try
            {
                MyAccountBLL bll = new MyAccountBLL();
                return bll.MyAccountAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/MyAccountDel")]
        public ResponseMyAccountDelete MyAccountDelete([FromBody]RequestMyAccountDelete request)
        {
            try
            {
                MyAccountBLL bll = new MyAccountBLL();
                return bll.MyAccountDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/DelMyAccount")]
        public ResponseMyAccountDelete DelMyAccount([FromBody]RequestMyAccountDelete request)
        {
            try
            {
                MyAccountBLL bll = new MyAccountBLL();
                return bll.DelMyAccount(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
