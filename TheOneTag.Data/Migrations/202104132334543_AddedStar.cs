namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUser", "IsStarred", c => c.Boolean(nullable: false));
            DropColumn("dbo.ApplicationUser", "IsPlayingRound");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUser", "IsPlayingRound", c => c.Boolean(nullable: false));
            DropColumn("dbo.ApplicationUser", "IsStarred");
        }
    }
}
