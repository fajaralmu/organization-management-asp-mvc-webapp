﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrgWebMvc.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ORG_DBEntities : DbContext
    {
        public ORG_DBEntities()
            : base("name=ORG_DBEntities")
        {
        }
        private static ORG_DBEntities dbEntities = null;


        public static ORG_DBEntities Instance()
        {

            if (dbEntities == null || InstApp.Util.Common.ObjectUtil.IsDisposed(dbEntities))
            {
                dbEntities = new ORG_DBEntities();
            }
            return dbEntities;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<division> divisions { get; set; }
        public virtual DbSet<@event> events { get; set; }
        public virtual DbSet<member> members { get; set; }
        public virtual DbSet<position> positions { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<program> programs { get; set; }
        public virtual DbSet<section> sections { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
