module WebStitcher

open Suave
open Suave.Successful
open Suave.RequestErrors

let getQuery (qs: (string * string option) list) (param: string): string list =
    List.fold (fun acc ->
        function
        | ("url", Some url) -> url :: acc
        | _ -> acc) [] qs

let app stitcher =
    request (fun r ->
        match (getQuery r.query "url") with
        | [ url1; url2 ] -> OK(stitcher url1 url2)
        | _ -> BAD_REQUEST "nope")
