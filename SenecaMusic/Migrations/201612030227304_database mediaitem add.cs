namespace SenecaMusic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databasemediaitemadd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        StringId = c.String(nullable: false, maxLength: 100),
                        Caption = c.String(nullable: false, maxLength: 100),
                        ContentType = c.String(nullable: false, maxLength: 200),
                        Content = c.Binary(),
                        Artist_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.Artist_Id)
                .Index(t => t.Artist_Id);
            
            AddColumn("dbo.Tracks", "AudioType", c => c.String(maxLength: 200));
            AddColumn("dbo.Tracks", "Clip", c => c.Binary());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MediaItems", "Artist_Id", "dbo.Artists");
            DropIndex("dbo.MediaItems", new[] { "Artist_Id" });
            DropColumn("dbo.Tracks", "Clip");
            DropColumn("dbo.Tracks", "AudioType");
            DropTable("dbo.MediaItems");
        }
    }
}
