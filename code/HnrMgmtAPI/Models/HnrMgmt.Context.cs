﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HnrMgmtAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_whut_HnrMgmtEntities : DbContext
    {
        public DB_whut_HnrMgmtEntities()
            : base("name=DB_whut_HnrMgmtEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<T_Award> T_Award { get; set; }
        public virtual DbSet<T_Awardee> T_Awardee { get; set; }
        public virtual DbSet<T_AwdRecord> T_AwdRecord { get; set; }
        public virtual DbSet<T_Client> T_Client { get; set; }
        public virtual DbSet<T_ExmRecord> T_ExmRecord { get; set; }
        public virtual DbSet<T_HnrRecord> T_HnrRecord { get; set; }
        public virtual DbSet<T_Honor> T_Honor { get; set; }
        public virtual DbSet<T_Organization> T_Organization { get; set; }
        public virtual DbSet<T_Right> T_Right { get; set; }
        public virtual DbSet<T_Role> T_Role { get; set; }
        public virtual DbSet<T_Team> T_Team { get; set; }
    }
}
