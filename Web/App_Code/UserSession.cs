using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// UserSession 的摘要说明
/// </summary>
[Serializable]
public class UserSession
{
    
 
    public string Token { get; set; }

    public string UserName { get; set; }

    public string TrueName { get; set; }

    public string Sex { get; set; }

    public string NodeName { get; set; }

    public string Id { get; set; }


}