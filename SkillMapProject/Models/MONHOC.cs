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
    
    public partial class MONHOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MONHOC()
        {
            this.HISTORY_SKILLMAP = new HashSet<HISTORY_SKILLMAP>();
            this.SKILLMAPs = new HashSet<SKILLMAP>();
        }
    
        public string MaBoMon { get; set; }
        public string TenBoMon { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int ModifyBy { get; set; }
        public System.DateTime ModifyDate { get; set; }
        public string LoaiMonHoc { get; set; }
        public string Dept { get; set; }
        public Nullable<int> Removed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORY_SKILLMAP> HISTORY_SKILLMAP { get; set; }
        public virtual Member Member { get; set; }
        public virtual Member Member1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SKILLMAP> SKILLMAPs { get; set; }
    }
}
