open System

let pad (s: list<byte>) (n: int) =
    let paddedbyte =
        if n > s.Length && n <= 255 then n - s.Length else 0

    let lpad =
        (seq {
            for i in 0 .. paddedbyte - 1 do
                (byte paddedbyte)
         }
         |> List.ofSeq)

    let newlist = s @ lpad
    newlist

[<EntryPoint>]
let main argv =
    let message = "YELLOW SUBMARINE"B |> List.ofArray
    let padded = pad message 20
    printfn "%A" padded
    0
