using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.Set
{
    public class SubjectController : ApiController
    {
        [HttpPost]
        [Route("fas/set/subjectListSearch")]
        public ResponseSubjectListSearch SubjectListSearch([FromBody]RequestSubjectListSearch request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/set/CheckSubjectIsReadonly")]
        public ResponseSubjectListSearch CheckSubjectIsReadonly([FromBody]RequestSubjectListSearch request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.CheckSubjectIsReadonly(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/set/subjectTotalGet")]
        public ResponseSubjectTotalGet SubjectTotalGet([FromBody]RequestSubjectTotalGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectTotalGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/GetSubject")]
        public ResponseSubjectTotalGet GetSubject([FromBody]RequestSubjectTotalGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.GetSubject(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpPost]
        [Route("fas/set/tplsubjectTotalGet")]
        public ResponseSubjectTotalGet TPLSubjectTotalGet([FromBody]RequestSubjectTotalGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.TPLSubjectTotalGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/subjectFormulaGet")]
        public ResponseSubjectTotalGet SubjectFormulaGet([FromBody]RequestSubjectTotalGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectFormulaGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/set/SubjectGet")]
        public ResponseSubjectGet SubjectGet([FromBody]RequestSubjectGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SubjectUpdate")]
        public ResponseSubjectUpdate SubjectUpdate([FromBody]RequestSubjectUpdate request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SubjectAdd")]
        public ResponseSubjectAdd SubjectAdd([FromBody]RequestSubjectAdd request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/CheckSubject")]
        public ResponseSubjectAdd CheckSubject([FromBody]RequestSubjectAdd request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.CheckSubject(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [Route("fas/set/SubjectDel")]
        public ResponseSubjectDelete SubjectDelete([FromBody]RequestSubjectDelete request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SubjectUIInit")]
        public ResponseSubjectUIInit SubjectUIInit([FromBody]RequestSubjectUIInit request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectUIInit(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/set/SubjectAssGet")]
        public ResponseSubjectAssGet SubjectAssGet([FromBody]RequestSubjectAssGet request)
        {
            try
            {
                SubjectBLL bll = new SubjectBLL();
                return bll.SubjectAssGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }


    }
}
