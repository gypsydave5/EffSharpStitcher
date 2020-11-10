module StitcherTests

open Stitcher
open Xunit
open FsUnit.Xunit

open Stitcher.Stitcher

[<Fact>]
let ``it stitches the responses from two urls together`` () =
    let response =  "I am a response"
    let stubHTTP url = response in
    let result = stitch stubHTTP "url one" "url two" in
    result |> should equal (response + response)
    
[<Fact>]
let ``it handles garbage URLs by returning an empty string`` () =
    let stubHTTP url = failwith "NO" in
    let result = stitch stubHTTP "garbage" "more garbage" in
    result |> should equal ""