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
    
    public partial class UMC_SKILLEntities : DbContext
    {
        public UMC_SKILLEntities()
            : base("name=UMC_SKILLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Certification> Certifications { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<OLYMPIC> OLYMPICS { get; set; }
        public virtual DbSet<Role_Member> Role_Member { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SkillLevel> SkillLevels { get; set; }
    }
}
