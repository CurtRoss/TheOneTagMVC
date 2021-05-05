namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondHashCodesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLeague", "IdHash", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLeague", "IdHash");
        }
    }
}
