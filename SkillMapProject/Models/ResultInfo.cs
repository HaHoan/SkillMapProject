using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMapProject.Models
{
    public class ResultInfo
    {
        public int code { get; set; }
        public string message { get; set; }
    }
    public static class RESULT
    {
        public static int SUCCESS = 1;
        public static int ERROR = 2;
    }
}