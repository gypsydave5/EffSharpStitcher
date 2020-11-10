namespace Stitcher

module Stitcher =
    let hello name = printfn "Hello %s" name

    let stitch (client: string -> string) (url1: string) (url2: string): string =
        try
            let body1 = client url1
            let body2 = client url2
            body1 + body2
        with _ -> ""
