open Suave
open WebStitcher
open Stitcher
open FSharp.Data

[<EntryPoint>]
let main argv =
    startWebServer defaultConfig (app (Stitcher.stitch Http.RequestString))
    0
