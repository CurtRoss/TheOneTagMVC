namespace TheOneTag.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangingThings : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.League", "LeaguePassword", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.League", "LeaguePassword", c => c.Int());
        }
    }
}
