open System
open System.IO
open System.Numerics

let hd s1 s2 =
    let lhd = (fun ac c -> ac + BitOperations.PopCount (uint32 ((uint8 (fst c)) ^^^ (uint8 (snd c)))))
    let sum = Seq.fold lhd 0 (Seq.zip s1 s2)
    sum

type Analyze =
    struct
        val content: list<byte>
        val mutable smallerkeys: list<int>
        new (fname: string) = {
            content = Convert.FromBase64String (File.ReadAllText fname) |> List.ofArray
            smallerkeys = [0; 0; 0]
        }

        member this.ProcessKeysize(keysize: int) =
            let s = [|
                new string (List.map (char) this.content.[0..(keysize - 1)] |> Array.ofList);
                new string (List.map (char) this.content.[keysize..(2 * keysize - 1)] |> Array.ofList);
                new string (List.map (char) this.content.[(2 * keysize)..(3 * keysize - 1)] |> Array.ofList);
                new string (List.map (char) this.content.[(3 * keysize)..(4 * keysize - 1)] |> Array.ofList);
            |]
            let indices = [|
                (0, 1); (0, 2); (0, 3);
                (1, 2); (1, 3); (2, 3)
            |]
            let mutable v = []
            for i in indices do
                v <- v @ [(float (hd s.[fst i] s.[snd i])) / (float keysize)]
            let r = List.sum v / (float indices.Length)
            r
    end

[<EntryPoint>]
let main argv =
    if argv.Length >= 1 then
        let analyze = Analyze argv.[0]
        for i in seq { 2 .. 40 } do
            let r = analyze.ProcessKeysize i
            printfn "(%d, %f)" i r
    0
