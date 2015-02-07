namespace PCExpert.Core.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComponentCharacteristics",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        ComponentType = c.Int(nullable: false),
                        FormattingPattern = c.String(),
                        type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CharacteristicValues",
                c => new
                    {
                        CharacteristicId = c.Guid(nullable: false),
                        ComponentId = c.Guid(nullable: false),
                        BoolValue = c.Boolean(),
                        IntValue = c.Decimal(precision: 18, scale: 2),
                        StringValue = c.String(),
                        type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CharacteristicId, t.ComponentId })
                .ForeignKey("dbo.ComponentCharacteristics", t => t.CharacteristicId, cascadeDelete: true)
                .ForeignKey("dbo.PCComponents", t => t.ComponentId, cascadeDelete: true)
                .Index(t => t.CharacteristicId)
                .Index(t => t.ComponentId);
            
            CreateTable(
                "dbo.PCComponents",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        AveragePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "Idx_PCComponent_NameUnique");
            
            CreateTable(
                "dbo.ComponentInterfaces",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "Idx_ComponentInterface_NameUnique");
            
            CreateTable(
                "dbo.PCConfigurations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        PublicName = c.String(maxLength: 250),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ParentToChildComponents",
                c => new
                    {
                        ParentComponent_id = c.Guid(nullable: false),
                        ChildComponent_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ParentComponent_id, t.ChildComponent_id })
                .ForeignKey("dbo.PCComponents", t => t.ParentComponent_id)
                .ForeignKey("dbo.PCComponents", t => t.ChildComponent_id)
                .Index(t => t.ParentComponent_id)
                .Index(t => t.ChildComponent_id);
            
            CreateTable(
                "dbo.ComponentToContainedSlots",
                c => new
                    {
                        Component_id = c.Guid(nullable: false),
                        ContainedSlot_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Component_id, t.ContainedSlot_id })
                .ForeignKey("dbo.PCComponents", t => t.Component_id, cascadeDelete: true)
                .ForeignKey("dbo.ComponentInterfaces", t => t.ContainedSlot_id, cascadeDelete: true)
                .Index(t => t.Component_id)
                .Index(t => t.ContainedSlot_id);
            
            CreateTable(
                "dbo.ComponentToPlugSlots",
                c => new
                    {
                        Component_id = c.Guid(nullable: false),
                        ContainedSlot_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Component_id, t.ContainedSlot_id })
                .ForeignKey("dbo.PCComponents", t => t.Component_id, cascadeDelete: true)
                .ForeignKey("dbo.ComponentInterfaces", t => t.ContainedSlot_id, cascadeDelete: true)
                .Index(t => t.Component_id)
                .Index(t => t.ContainedSlot_id);
            
            CreateTable(
                "dbo.ComponentToConfiguration",
                c => new
                    {
                        config_Id = c.Guid(nullable: false),
                        component_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.config_Id, t.component_Id })
                .ForeignKey("dbo.PCConfigurations", t => t.config_Id, cascadeDelete: true)
                .ForeignKey("dbo.PCComponents", t => t.component_Id, cascadeDelete: true)
                .Index(t => t.config_Id)
                .Index(t => t.component_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComponentToConfiguration", "component_Id", "dbo.PCComponents");
            DropForeignKey("dbo.ComponentToConfiguration", "config_Id", "dbo.PCConfigurations");
            DropForeignKey("dbo.ComponentToPlugSlots", "ContainedSlot_id", "dbo.ComponentInterfaces");
            DropForeignKey("dbo.ComponentToPlugSlots", "Component_id", "dbo.PCComponents");
            DropForeignKey("dbo.ComponentToContainedSlots", "ContainedSlot_id", "dbo.ComponentInterfaces");
            DropForeignKey("dbo.ComponentToContainedSlots", "Component_id", "dbo.PCComponents");
            DropForeignKey("dbo.ParentToChildComponents", "ChildComponent_id", "dbo.PCComponents");
            DropForeignKey("dbo.ParentToChildComponents", "ParentComponent_id", "dbo.PCComponents");
            DropForeignKey("dbo.CharacteristicValues", "ComponentId", "dbo.PCComponents");
            DropForeignKey("dbo.CharacteristicValues", "CharacteristicId", "dbo.ComponentCharacteristics");
            DropIndex("dbo.ComponentToConfiguration", new[] { "component_Id" });
            DropIndex("dbo.ComponentToConfiguration", new[] { "config_Id" });
            DropIndex("dbo.ComponentToPlugSlots", new[] { "ContainedSlot_id" });
            DropIndex("dbo.ComponentToPlugSlots", new[] { "Component_id" });
            DropIndex("dbo.ComponentToContainedSlots", new[] { "ContainedSlot_id" });
            DropIndex("dbo.ComponentToContainedSlots", new[] { "Component_id" });
            DropIndex("dbo.ParentToChildComponents", new[] { "ChildComponent_id" });
            DropIndex("dbo.ParentToChildComponents", new[] { "ParentComponent_id" });
            DropIndex("dbo.ComponentInterfaces", "Idx_ComponentInterface_NameUnique");
            DropIndex("dbo.PCComponents", "Idx_PCComponent_NameUnique");
            DropIndex("dbo.CharacteristicValues", new[] { "ComponentId" });
            DropIndex("dbo.CharacteristicValues", new[] { "CharacteristicId" });
            DropTable("dbo.ComponentToConfiguration");
            DropTable("dbo.ComponentToPlugSlots");
            DropTable("dbo.ComponentToContainedSlots");
            DropTable("dbo.ParentToChildComponents");
            DropTable("dbo.PCConfigurations");
            DropTable("dbo.ComponentInterfaces");
            DropTable("dbo.PCComponents");
            DropTable("dbo.CharacteristicValues");
            DropTable("dbo.ComponentCharacteristics");
        }
    }
}
