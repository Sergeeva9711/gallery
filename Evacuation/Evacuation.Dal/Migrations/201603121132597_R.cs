namespace Evacuation.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class R : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Name", c => c.String());
            AlterColumn("dbo.Projects", "Image", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Image", c => c.Binary(nullable: false));
            AlterColumn("dbo.Projects", "Name", c => c.String(nullable: false));
        }
    }
}
