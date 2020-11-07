open System

[<EntryPoint>]
let main argv =
    let s1 = "1c0111001f010100061a024b53535009181c"
    let s2 = "686974207468652062756c6c277320657965"
    let mutable r = ""

    for i in seq { 0 .. 2 .. s1.Length - 1 } do
        let n1 = Convert.ToByte(s1.[i..i+1], 16)
        let n2 = Convert.ToByte(s2.[i..i+1], 16)
        r <- r + sprintf "%x" (n1 ^^^ n2)

    printfn "%s" r

    0
