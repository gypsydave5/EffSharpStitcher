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

let get (path: string) =
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

[<Fact>]
let ``1 add 1 = 2`` () = 1 + 1 |> should equal 2

[<Fact>]
let ``routing for valid URL`` () =
    let (spy, stitcher) = spyOn (+)
    let testApp = WebStitcher.app stitcher
    let request = get "/stitcher?url=bob&url=joe"

    let response =
        testApp request
        |> Async.RunSynchronously
        |> Option.get

    let body =
        match response.response.content with
        | Suave.Http.Bytes bytes -> System.Text.Encoding.ASCII.GetString bytes
        | _ -> ""

    body |> should equal "joebob"
    spy.Calls.Head |> should equal ("joe", "bob")
