namespace TourAgency.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedImageStoring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Images", "Picture", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Images", "Picture", c => c.Binary(nullable: false));
        }
    }
}
