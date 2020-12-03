﻿using SkillMapProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMapProject.Helper
{
    public static class SessionHelper
    {
        public static T Get<T>(string key)
        {
            var valueFromSession = HttpContext.Current.Session[key];
            if (valueFromSession is T)
            {
                return (T)valueFromSession;
            }
            return default(T);
        }

        public static void Set(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
        public static bool IsLogIn()
        {
            return Get<Member>(Constant.SESSION_LOGIN) != null;
        }
    }
}