using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class BALController : ApiController
    {
        [HttpPost]
        [Route("fas/set/balListSearch")]
        public ResponseBALListSearch BALListSearch([FromBody]RequestBALListSearch request)
        {
            try
            {
                BALBLL bll = new BALBLL();
             
                return bll.BALListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/balAdd")]
        public ResponseBALAdd BALAdd([FromBody]RequestBALAdd request)
        {
            try
            {
                BALBLL bll = new BALBLL();

                return bll.BALAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [HttpPost]
        [Route("fas/set/balDel")]
        public ResponseBALDelete BALDelete([FromBody]RequestBALDelete request)
        {
            try
            {
                BALBLL bll = new BALBLL();

                return bll.BALDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/set/balUpdate")]
        public ResponseBALUpdate BALUpdate([FromBody]RequestBALUpdate request) {
            try
            {
                BALBLL bll = new BALBLL();

                return bll.BALUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [Route("fas/set/balcal")]
        public ResponseBALCalculate BALCalculate([FromBody]RequestBALCalculate request)
        {
            try
            {
                BALBLL bll = new BALBLL();

                return bll.BALCalculate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


        [HttpPost]
        [Route("fas/set/getExcelBySql")]
        public HttpResponseMessage getExcelBySql([FromBody]RequestExcel request)
        {
            try
            {
                BALBLL bll = new BALBLL();

                //return bll.getExcelBySql(request);
                var file = bll.getExcelBySql(request);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new FileStream(file.DirectoryName, FileMode.Open);
               
                response.Content = new StreamContent(stream);
                //response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                //{
                //    FileName = HttpUtility.UrlEncode(Path.GetFileName(FilePath))
                //};
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlEncode(Path.GetFileName(file.DirectoryName));
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            //try
            //{
            //    BALBLL bll = new BALBLL();

            //    return bll.getExcelBySql(request);
            //}
            //catch (Exception ex)
            //{
            //    throw new HttpResponseException(
            //    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            //}
        }
    }
}
