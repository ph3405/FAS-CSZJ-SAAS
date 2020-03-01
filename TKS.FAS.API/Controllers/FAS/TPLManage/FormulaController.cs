using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.API.Controllers.FAS.TPLManage
{
    public class FormulaController : ApiController
    {
        [HttpPost]
        [Route("fas/tplmanage/FormulaListSearch")]
        public ResponseFormulaListSearch FormulaListSearch([FromBody]RequestFormulaListSearch request)
        {
            try
            {
                FormulaBLL bll = new FormulaBLL();
                return bll.FormulaListSearch(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPost]
        [Route("fas/tplmanage/FormulaGet")]
        public ResponseFormulaGet FormulaGet([FromBody]RequestFormulaGet request)
        {
            try
            {
                FormulaBLL bll = new FormulaBLL();
                return bll.FormulaGet(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
               Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [Route("fas/tplmanage/FormulaUpdate")]
        public ResponseFormulaUpdate FormulaUpdate([FromBody]RequestFormulaUpdate request)
        {
            try
            {
                FormulaBLL bll = new FormulaBLL();
                return bll.FormulaUpdate(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

    

        [Route("fas/tplmanage/FormulaAdd")]
        public ResponseFormulaAdd FormulaAdd([FromBody]RequestFormulaAdd request)
        {
            try
            {
                FormulaBLL bll = new FormulaBLL();
                return bll.FormulaAdd(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
              Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
       
        [Route("fas/tplmanage/FormulaDel")]
        public ResponseFormulaDelete FormulaDelete([FromBody]RequestFormulaDelete request)
        {
            try
            {
                FormulaBLL bll = new FormulaBLL();
                return bll.FormulaDelete(request);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
             Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
