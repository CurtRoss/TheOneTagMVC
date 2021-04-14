namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewStuff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUser", "IsPlayingRound", c => c.Boolean(nullable: false));
            AddColumn("dbo.ApplicationUser", "Score", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUser", "Score");
            DropColumn("dbo.ApplicationUser", "IsPlayingRound");
        }
    }
}
