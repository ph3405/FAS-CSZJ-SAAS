using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.QM
{
    public class QMController : ApiController
    {
        /// <summary>
        /// 获取当前激活年度的会计期间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/qm/QMListSearch")]
        public ResponseQMListSearch QMListSearch([FromBody]RequestQMListSearch request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        /// <summary>
        /// 期末检查模板获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/qm/QMCheckTPLsGet")]
        public ResponseQMCheckTPLsGet QMCheckTPLsGet([FromBody]RequestQMCheckTPLsGet request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMCheckTPLsGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        /// <summary>
        /// 期末结转
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/qm/QMCarryOver")]
        public ResponseQMCarryOver QMCarryOver([FromBody]RequestQMCarryOver request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMCarryOver(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/qm/QMUnCarryOver")]
        public ResponseQMUnCarryOver QMUnCarryOver([FromBody]RequestQMUnCarryOver request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMUnCarryOver(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/qm/QMSYStatusGet")]
        public ResponseQMSYStatusGet QMSYStatusGet([FromBody]RequestQMSYStatusGet request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMSYStatusGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/qm/QMSYGen")]
        public ResponseQMSYGen QMSYGen([FromBody]RequestQMSYGen request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMSYGen(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/qm/QMSCCBGen")]
        public ResponseSCCBGen QMSCCBGen([FromBody]RequestSCCBGen request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.SCCBGen(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/qm/QMValidate")]
        public ResponseQMValidate QMValidate([FromBody]RequestQMValidate request)
        {
            try
            {
                QMBLL bll = new QMBLL();
                return bll.QMValidate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
    }
}
