using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;

/// <summary>
/// SessionHelper 的摘要说明
/// </summary>
public class SessionHelper
{
    public SessionHelper()
    {
       
    }
    static string key = "user";   
    public static UserSession GetUserInfo()
    {
        try
        {
            var o = System.Web.HttpContext.Current.Session[key] as UserSession;
            return o;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public static void SetUserInfo(UserSession user)
    {
        System.Web.HttpContext.Current.Session[key] = user;
    }

    public static void Clear() {
        System.Web.HttpContext.Current.Session.Clear();
    }

   
}