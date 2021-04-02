namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationUser", "PdgaNum", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationUser", "PdgaNum", c => c.Int(nullable: false));
        }
    }
}
