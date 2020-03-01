using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Entity;

namespace TKS.FAS.API.Controllers.FAS.AccountBook
{
    public class AccountBookController : ApiController
    {
        [HttpPost]
        [Route("fas/accountBook/DocSubjectCodeGet")]
        public ResponseDocSubjectCodeGet DocSubjectCodeGet([FromBody]RequestDocSubjectCodeGet request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.DocSubjectCodeGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        /// <summary>
        /// 获取期间区间内的科目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CodeGetInPeriod")]
        public ResponseCodeGetInPeriod CodeGetInPeriod([FromBody]RequestCodeGetInPeriod request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CodeGetInPeriod(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/accountBook/DetailListSearch")]
        public ResponseDetailListSearch DetailListSearch(RequestDetailListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.DetailListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
      

        [HttpPost]
        [Route("fas/accountBook/GenAccountListSearch")]
        public ResponseGenAccountListSearch GenAccountListSearch(RequestGenAccountListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.GenAccountListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 科目余额表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/BalAccountListSearch")]
        public ResponseKMBalListSearch BalAccountListSearch(RequestKMBalListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.BALAccountSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/accountBook/BalAccountListGet")]
        public ResponseKMBalListSearch BalAccountListGet(RequestKMBalListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.BALAccountGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        /// <summary>
        /// 核算项目明细账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CalcuAccountDetailListSearch")]
        public ResponseCalcuAccountDetailListSearch CalcuAccountDetailListSearch(RequestCalcuAccountDetailListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CalcuAccountDetailListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 核算项获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CalculateItemGet")]
        public ResponseCalculateItemGet CalculateItemGet(RequestCalculateItemGet request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CalculateItemGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 核算项值获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CalculateValuesGet")]
        public ResponseCalculateValuesGet CalculateValuesGet(RequestCalculateValuesGet request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CalculateValuesGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 核算项值获取(期间区间内)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CalculateValuesGetInPeriod")]
        public ResponseCalculateValuesGetInPeriod CalculateValuesGetInPeriod(RequestCalculateValuesGetInPeriod request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CalculateValuesGetInPeriod(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        /// <summary>
        /// 科目汇总表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/SummaryListSearch")]
        public ResponseSummaryListSearch SummaryGet([FromBody]RequestSummaryListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.SummaryListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 序时账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/DocDetailListSearch")]
        public ResponseDocDetailAccountListSearch DocDetailListSearch([FromBody]RequestSummaryListSearch request)
        {
            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.DocDetailListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        /// <summary>
        /// 核算项目余额表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("fas/accountBook/CalBalListSearch")]
        public ResponseCalBalListSearch CalBalListSearch([FromBody]RequestCalBalListSearch request) {

            try
            {
                AccountBookBLL bll = new AccountBookBLL();
                return bll.CalBalListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }


    }
}
