namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserLeague", "RoundScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserLeague", "RoundScore");
        }
    }
}
