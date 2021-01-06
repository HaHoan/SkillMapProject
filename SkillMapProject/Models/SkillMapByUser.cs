using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMapProject.Models
{
    public class SkillMapByUser
    {
        public int userID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Dept { get; set; }
        public string DateEnter { get; set; }
        public List<SKILLMAP> ListSkillMaps { get; set; }
        public Dictionary<MONHOC, SKILLMAP> dics { get; set; }
    }
}