open System

[<EntryPoint>]
let main argv =
    let d = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"
    let s = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
    let mutable r = ""

    for i in seq { 0 .. 6 .. s.Length - 1 } do
        let n1 = Convert.ToByte(s.[i..i+1], 16)
        let n2 = Convert.ToByte(s.[i+2..i+3], 16)
        let n3 = Convert.ToByte(s.[i+4..i+5], 16)
        let d1 = int n1 &&& (0b11111100 >>> 2)
        let d2 = ((int n1 &&& 0b00000011) <<< 4) ||| ((int n2 &&& 0b11110000) >>> 4)
        let d3 = ((int n2 &&& 0b00001111) <<< 2) ||| ((int n3 &&& 0b11000000) >>> 6)
        let d4 = int n3 &&& 0b00111111
        r <- r + sprintf "%c%c%c%c" d.[d1] d.[d2] d.[d3] d.[d4]

    printfn "%s" r

    0