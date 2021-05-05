namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Activities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        PlayerId = c.String(maxLength: 128),
                        LeagueId = c.Int(nullable: false),
                        DateOfActivity = c.DateTimeOffset(nullable: false, precision: 7),
                        PlayerZipCode = c.Int(nullable: false),
                        LeagueZipCode = c.Int(nullable: false),
                        StartingRank = c.Int(nullable: false),
                        EndingRank = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActivityId)
                .ForeignKey("dbo.League", t => t.LeagueId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUser", t => t.PlayerId)
                .Index(t => t.PlayerId)
                .Index(t => t.LeagueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activity", "PlayerId", "dbo.ApplicationUser");
            DropForeignKey("dbo.Activity", "LeagueId", "dbo.League");
            DropIndex("dbo.Activity", new[] { "LeagueId" });
            DropIndex("dbo.Activity", new[] { "PlayerId" });
            DropTable("dbo.Activity");
        }
    }
}
