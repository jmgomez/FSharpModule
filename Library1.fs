namespace FSharpModule
open System.Web.Mvc
open Orchard.Themes
open Orchard.ContentManagement
open Orchard.ContentManagement.Records
open System.ComponentModel.DataAnnotations
open Orchard.ContentManagement.Handlers
open Orchard.Data
open Orchard.ContentManagement.Drivers
open EkonBenefits.FSharp.Dynamic
open Orchard.Data.Migration
open Orchard.ContentManagement.MetaData
open Orchard.ContentManagement.MetaData.Builders
open Orchard.Core.Contents.Extensions
open System.Data
open FSharpModule.Models





type Salute() =
    member this.SayHello = "Hello from Fsharp"

[<Themed>]
type FSharpController() =
    inherit Controller()        
    member this.Index() =             
        this.View(Salute())


type SessionConfiguration() =
    interface ISessionConfigurationEvents with 
        member this.Created(cfg,defaultModel) = 
          cfg.ExposeConfiguration(fun c->c.Properties.Add("use_proxy_validator", "false") |>ignore)|>ignore
          ignore()
        member this.Building(cfg) =
          ignore()
        member this.Prepared(cfg) =
          ignore()
        member this.Finished(cfg) =
          ignore()
        member this.ComputingHash(hash) =
          ignore()

type MapHandler(repository:IRepository<MapRecord>) = 
    inherit ContentHandler()
    do        
        base.Filters.Add(StorageFilter.For(repository)) |> ignore
        
            
type MapDriver() = 
    inherit ContentPartDriver<MapPart>()
    override this.Prefix with get() = "MapPart"
    override this.Display(part,displayType,shapeHelper) =             
        this.ContentShape("Parts_Map",fun()-> shapeHelper?Parts_Map(part)) 
        :> DriverResult
    //GET
    override this.Editor(part,shapeHelper) =      
        let editor = shapeHelper?EditorTemplate()       
        editor?TemplateName <- "Parts/Map"
        editor?Model <- part
        editor?Prefix <- this.Prefix          
        this.ContentShape("Parts_Map_Edit",fun()->editor):> DriverResult
    //POST
    override this.Editor(part,updater,shapeHelper) = 
        updater.TryUpdateModel(part,this.Prefix,null,null) |> ignore
        this.Editor(part,shapeHelper)


type Migration() =
    inherit DataMigrationImpl()
    member this.Create()=                
        this.SchemaBuilder.CreateTable("MapRecord",fun table->
            table.ContentPartRecord().Column<double>("Latitude")
            |> fun t->t.Column<double>("Longitude")
            |>ignore)
            |>ignore 
        this.ContentDefinitionManager.AlterPartDefinition("MapPart",fun cfg->
            cfg.Attachable()
            |>ignore)
            |>ignore
        this.ContentDefinitionManager.AlterTypeDefinition("MapWidget",fun cfg->
            cfg.WithPart("MapPart").WithPart("WidgetPart")
            |> fun t->t.WithPart("CommonPart").WithSetting("Stereotype","Widget")
            |>ignore)
            |>ignore
        1

