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
    
    public partial class HISTORY_SKILLMAP
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string StaffCode { get; set; }
        public string MaBoMon { get; set; }
        public System.DateTime NgayThamGia { get; set; }
        public string GhiChu { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual MONHOC MONHOC { get; set; }
    }
}
