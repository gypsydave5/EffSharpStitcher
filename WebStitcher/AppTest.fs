module WebStitcherTests

open Xunit
open FsUnit.Xunit

type Spy() =
    let mutable calls = []
    member spy.Record(args) = calls <- args :: calls
    member spy.Calls = calls

let spyOn f =
    let spy = Spy()
    (spy,
     (fun a1 a2 ->
         spy.Record(a1, a2)
         f a1 a2))

let newGetRequest (path: string) =
    let (path, query) =
        match (path.Split "?") with
        | [| path; query |] -> (path, query)
        | [| path |] -> (path, "")
        | _ -> ("", "")

    let request: Suave.Http.HttpRequest =
        { Suave.Http.HttpRequest.empty with
              rawPath = path
              rawQuery = query
              rawMethod = "GET" }

    { Suave.Http.HttpContext.empty with
          request = request }

let callRouteSync route request =
    route request
    |> Async.RunSynchronously
    |> Option.get

let getBodyString (ctx: Suave.Http.HttpContext) =
    match ctx.response.content with
    | Suave.Http.Bytes bytes -> System.Text.Encoding.ASCII.GetString bytes
    | x -> failwith (sprintf "Expected bytes but got %s" (x.ToString()))

[<Fact>]
let ``1 add 1 = 2`` () = 1 + 1 |> should equal 2

[<Fact>]
let ``routing for valid URL`` () =
    let (spy, stitcher) = spyOn (+)
    let testApp = WebStitcher.app stitcher

    let body =
        newGetRequest "/stitcher?url=bob&url=joe"
        |> callRouteSync testApp
        |> getBodyString

    body |> should equal "joebob"
    spy.Calls.Head |> should equal ("joe", "bob")
