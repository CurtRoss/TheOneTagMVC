namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedToTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.League", "OwnerId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.League", "OwnerId");
        }
    }
}
