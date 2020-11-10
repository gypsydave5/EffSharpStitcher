// Learn more about F# at http://fsharp.org

open FSharp.Data

[<EntryPoint>]
let main argv =
    let response = Http.RequestString "garbage"
    printf "%s" response
    0 // return an integer exit code
