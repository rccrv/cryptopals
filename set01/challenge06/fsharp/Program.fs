open System
open System.Numerics

let hd s1 s2 =
    let lhd = (fun ac c -> ac + BitOperations.PopCount (uint32 ((uint8 (fst c)) ^^^ (uint8 (snd c)))))
    let sum = Seq.fold lhd 0 (Seq.zip s1 s2)
    sum

[<EntryPoint>]
let main argv =
    let s1 = "this is a test"
    let s2 = "wokka wokka!!!"
    let r = hd s1 s2
    printfn "%d" r
    0
