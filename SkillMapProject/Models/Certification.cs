//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkillMapProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Certification
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int SkillID { get; set; }
        public string Mark { get; set; }
        public string Result { get; set; }
        public Nullable<System.DateTime> DateComplete { get; set; }
        public System.DateTime DateMark { get; set; }
        public Nullable<int> Marker { get; set; }
        public Nullable<int> Updator { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string Note { get; set; }
        public Nullable<int> Sort { get; set; }
        public string Task { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Member Member1 { get; set; }
        public virtual Member Member2 { get; set; }
        public virtual Skill Skill { get; set; }
    }
}