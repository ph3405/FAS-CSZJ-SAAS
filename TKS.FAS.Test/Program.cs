using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKS.FAS.BLL;
using TKS.FAS.Entity;

namespace TKS.FAS.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            

            RoleBLL bll = new RoleBLL();
            UserBLL userBll = new UserBLL();
            for (int i = 1; i <=1000; i++)
            {
                //userBll.UserAdd(new Entity.SSO.RequestUserAdd
                //{
                //    Data = new TKS_FAS_User
                //    {

                //        CreateUser = "zm"+i.ToString (),
                //        CreateDate = DateTime.Now,
                //        Id = Guid.NewGuid().ToString("N"),
                //        Sex = "0",
                //        Status = "1",
                //        UserName = "zm" + i.ToString(),
                //        TrueName = ""

                //    }
                //});
                bll.RoleAdd(new Entity.SSO.RequestRoleAdd {
                    Data = new TKS_FAS_Role {
                        Id = Guid.NewGuid().ToString("N"),
                        Name = "角色" + i.ToString(),
                        CreateUser = "zm" + i.ToString(),
                        CreateDate = DateTime.Now
                     }
                });
            }
            try
            {
                //for (int i = 1; i <= 100; i++)
                //    bll.RoleAdd(new Entity.SSO.RequestRoleAdd
                //    {
                //        Data = new TKS_FAS_Role
                //        {
                //            CreateUser = "123",
                //            id = i.ToString(),
                //            Name = "12322",
                //            CreateDate = DateTime.Now

                //        }
                //    });
            }
            catch (Exception ex)
            {

            }
            var res = bll.RoleUpdate(new Entity.SSO.RequestRoleUpdate
            {
                Data = new TKS_FAS_Role
                {
                    Id = "100",
                    Name = "wo de",
                    UpdateUser = "test",
                    UpdateDate = DateTime.Parse("2017-09-09 12:12:12")
                }
            });

            Console.WriteLine(res.Message);


            //var r = bll.RoleDelete(new Entity.SSO.RequestRoleDelete
            //{
            //    Data = new TKS_FAS_Role
            //    {
            //        Id = "99"
            //    }
            //});
            //Console.WriteLine(r.Message);
            var roles = bll.RoleListSearch(new Entity.SSO.RequestRoleListSearch
            {
                PageIndex = 1,
                PageSize = 10,
              
                Token = ""
            });


            foreach (var item in roles.Data)
            {
                Console.WriteLine("{0}----{1}", item.Id, item.Name);
            }

            Console.ReadLine();
        }
    }
}
