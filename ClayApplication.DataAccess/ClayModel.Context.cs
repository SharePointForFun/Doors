﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClayApplication.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ClayDbEntities : DbContext
    {
        public ClayDbEntities()
            : base("name=ClayDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Door> Door { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DoorAccess> DoorAccess { get; set; }
        public DbSet<DoorAccessLog> DoorAccessLog { get; set; }
    }
}