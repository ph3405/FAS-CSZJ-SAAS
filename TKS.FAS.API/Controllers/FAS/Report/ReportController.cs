using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.BLL.FAS.Report;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.Report
{
    public class ReportController : ApiController
    {
        [HttpPost]
        [Route("fas/report/ZCFZGet")]
        public ResponseZCFZGet ZCFZGet([FromBody]RequestZCFZGet request)
        {
            try
            {
                ReportBLL bll = new ReportBLL();
                return bll.ZCFZGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/report/PrintZCFZ")]
        public PrintZCFZGet PrintZCFZ([FromBody]RequestZCFZGet request)
        {
            try
            {
                ReportBLL bll = new ReportBLL();
                return bll.PrintZCFZ(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/report/LRGet")]
        public ResponseLRGet LRGet([FromBody]RequestLRGet request)
        {
            try
            {
                ReportBLL bll = new ReportBLL();
                return bll.LRGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/report/SJGet")]
        public ResponseSJGet SJGet([FromBody]RequestSJGet request)
        {
            try
            {
                ReportBLL2 bll = new ReportBLL2();
                return bll.SJGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        //[HttpPost]
        //[Route("fas/report/WX_SJGet")]
        //public ResponseSJGet WX_SJGet([FromBody]RequestSJGet request)
        //{
        //    try
        //    {
        //        ReportBLL2 bll = new ReportBLL2();
        //        return bll.WX_SJGet(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(
        //        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }

        //}

        [HttpPost]
        [Route("fas/report/JYGet")]
        public ResponseJYGet JYGet([FromBody]RequestJYGet request)
        {
            try
            {
                ReportBLL3 bll = new ReportBLL3();
                return bll.JYGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        //[HttpPost]
        //[Route("fas/report/WX_JYGet")]
        //public ResponseJYGet WX_JYGet([FromBody]RequestJYGet request)
        //{
        //    try
        //    {
        //        ReportBLL3 bll = new ReportBLL3();
        //        return bll.WX_JYGet(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new HttpResponseException(
        //        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
        //    }

        //}
    }
}
