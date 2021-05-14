using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillMapProject.Models
{
    public class CertificateByUser
    {
        public int userID { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Dept { get; set; }
        public string Customer { get; set; }
        public string DateEnter { get; set; }
        public List<Certification> ListSkills { get; set; }
        public Dictionary<Skill, Certification> dics { get; set; }
    }

  
}