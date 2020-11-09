open System

[<EntryPoint>]
let main argv =
    let s ="Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal"
    let mutable r = ""
    let key = "ICE"
    let cycle = Seq.init s.Length (fun i -> byte key.[i % 3])
    let iter = Seq.zip (s |> Seq.map (byte)) cycle
    for i in iter do
        r <- r + sprintf "%02x" ((fst i) ^^^ (snd i))
    printfn "%s" r
    0
