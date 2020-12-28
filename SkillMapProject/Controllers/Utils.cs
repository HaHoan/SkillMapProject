using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMapProject.Controllers
{
    public static class Ultils
    {
        public static DateTime CalYearDateTime(this DateTime datetime)
        {
            DateTime genrateDate = new DateTime();


            return genrateDate;
        }

        public static bool IsNull(string value)
        {
            if (value != "" && !string.IsNullOrEmpty(value) && value.Length >= 1)
            {
                return true;
            }
            return false;
        }
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}