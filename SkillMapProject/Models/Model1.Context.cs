﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GA_UMCEntities : DbContext
    {
        public GA_UMCEntities()
            : base("name=GA_UMCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MS_Department> MS_Department { get; set; }
    
        public virtual ObjectResult<sp_Get_All_Check_Solders_Result> sp_Get_All_Check_Solders(string deptCode)
        {
            var deptCodeParameter = deptCode != null ?
                new ObjectParameter("deptCode", deptCode) :
                new ObjectParameter("deptCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_Get_All_Check_Solders_Result>("sp_Get_All_Check_Solders", deptCodeParameter);
        }
    
        public virtual ObjectResult<sp_Get_All_CheckEye_Result> sp_Get_All_CheckEye(string deptCode, string staffCode)
        {
            var deptCodeParameter = deptCode != null ?
                new ObjectParameter("deptCode", deptCode) :
                new ObjectParameter("deptCode", typeof(string));
    
            var staffCodeParameter = staffCode != null ?
                new ObjectParameter("staffCode", staffCode) :
                new ObjectParameter("staffCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_Get_All_CheckEye_Result>("sp_Get_All_CheckEye", deptCodeParameter, staffCodeParameter);
        }
    
        public virtual ObjectResult<sp_Get_All_Staff_2_Result> sp_Get_All_Staff_2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_Get_All_Staff_2_Result>("sp_Get_All_Staff_2");
        }
    }
}
