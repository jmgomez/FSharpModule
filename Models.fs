namespace FSharpModule.Models
open Orchard.ContentManagement
open Orchard.ContentManagement.Records
open System.ComponentModel.DataAnnotations
open System.Xml.Serialization
type DVA = DefaultValueAttribute

type MapRecord() = 
    inherit ContentPartRecord()
    [<DVA(false)>] val mutable private lat : double 
    [<DVA(false)>] val mutable private lon : double 

    abstract Latitude : double with get,set 
    override this.Latitude with get() = this.lat
                           and set(v) = this.lat <- v

    abstract Longitude : double with get,set
    override this.Longitude with get() = this.lon 
                            and set(v) = this.lon <- v         
type MapPart() = 
    inherit ContentPart<MapRecord>()
    [<Required>]
    member this.Latitude 
        with get() = base.Record.Latitude
        and set(v) = base.Record.Latitude <- v
    [<Required>]
    member this.Longitude 
        with get() = base.Record.Longitude
        and set(v) = base.Record.Longitude <- v


