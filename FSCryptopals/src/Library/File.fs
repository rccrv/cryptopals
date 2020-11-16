namespace Library

open System
open System.IO
open Library.Constants

module File =
    let readfile (fname: string) (mode: FileMode): seq<byte []> [] =
        match mode with
        | FileMode.HexString ->
            let lines = File.ReadAllLines fname

            let bytes =
                Array.map (fun l -> Hex.hexstringyieldnextbyte l 1) lines

            bytes
        | FileMode.Base64String ->
            let bytes =
                [| seq { Convert.FromBase64String(File.ReadAllText fname) } |]

            bytes
        | _ -> invalidArg (sprintf "%A" mode) "mode should be a valid FileMode value"
