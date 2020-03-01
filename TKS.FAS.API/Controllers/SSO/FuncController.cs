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
    public class FuncController : ApiController
    {
        [HttpPost]
        [Route("sso/funcTreeGet")]
        public ResponseFuncTreeGet FuncTreeGet([FromBody]RequestFuncTreeGet request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncTreeGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/funcTreeCheckedGet")]
        public ResponseFuncTreeCheckedGet FuncTreeCheckedGet([FromBody]RequestFuncTreeCheckedGet request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncTreeCheckedGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/funcTreeCheckedUpdate")]
        public ResponseFuncTreeCheckedUpdate FuncTreeCheckedUpdate([FromBody]RequestFuncTreeCheckedUpdate request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncTreeCheckedUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [Route("sso/funcGet")]
        public ResponseFuncGet FuncGet([FromBody]RequestFuncGet request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/funcAdd")]
        public ResponseFuncAdd FuncAdd([FromBody]RequestFuncAdd request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/funcDel")]
        public ResponseFuncDelete FuncDelete([FromBody]RequestFuncDelete request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("sso/funcUpdate")]
        public ResponseFuncUpdate FuncUpdate([FromBody] RequestFuncUpdate request)
        {
            try
            {
                FunctionBLL bll = new FunctionBLL();
                return bll.FuncUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}