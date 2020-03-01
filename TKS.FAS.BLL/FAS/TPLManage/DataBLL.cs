using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Entity.FAS;

using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;

using TKS.FAS.Common;

namespace TKS.FAS.BLL
{
    public class DataBLL : CommonBase
    {


        public ResponseDataGet NodeGet(RequestDataGet request)
        {
            ResponseDataGet response = new ResponseDataGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.Query<TKS_FAS_DATA>("select * from TKS_FAS_DATA where parentId=@parentId", new
                    {
                        parentId = request.GroupCode
                    }).ToList ();

                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data ;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseDataGet;
                }
            }
        }



    }
}
