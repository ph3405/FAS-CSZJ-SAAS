using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_MenuItem
    {
        //     "title": "权限管理",
        //"icon": "&#xe631;",
        //"href": "",
        //"spread": false,
        //"children"
        public string id { get; set; }
        public string title { get; set; }

        public string icon { get; set; }

        public string href { get; set; }

        public bool spread { get; set; }

        public List<TKS_FAS_MenuItem> children { get; set; }
    }
}
