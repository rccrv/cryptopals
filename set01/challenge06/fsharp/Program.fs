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
        val mutable smallerkeys: array<int>
        val minsizes: int
        new (fname: string, size: int) = {
            content = Convert.FromBase64String (File.ReadAllText fname) |> List.ofArray
            minsizes = size
            smallerkeys = Array.zeroCreate size
        }

        member this.ProcessKeysize(keysize: int) =
            let mutable v = []
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
            for i in indices do
                v <- v @ [(float (hd s.[fst i] s.[snd i])) / (float keysize)]
            let r = List.sum v / (float indices.Length)
            r

        member this.ProbableKeysizes(range: seq<int>) =
            let mutable v = []
            for i in range do
                v <- v @ [this.ProcessKeysize(i)]
            if v.Length > 2 then
                let mins = (List.sort v).[0..3]
                let r = [|
                    2 + List.findIndex (fun i -> i = mins.[0]) v;
                    2 + List.findIndex (fun i -> i = mins.[1]) v;
                    2 + List.findIndex (fun i -> i = mins.[2]) v;
                    2 + List.findIndex (fun i -> i = mins.[3]) v;
                |]
                Array.Copy(r, this.smallerkeys, this.minsizes)
            ()

        member this.BreakString() =
            let mutable v = []
            for i in this.smallerkeys do
                let divisions = this.content.Length / i
                (*
                for j in seq { 1 .. divisions} do
                    v <- v @ [ this.content[j..j-1] ]
                    0 |> ignore
                *)
                let lastsize = this.content.Length - i * divisions
                printfn "%d, %d" divisions lastsize
            ()
    end

[<EntryPoint>]
let main argv =
    if argv.Length >= 1 then
        let analyze = Analyze(argv.[0], 4)
        printfn "%d" analyze.content.Length
        analyze.ProbableKeysizes(seq { 2 .. 40 }) |> ignore
        printfn "%A" analyze.smallerkeys
        let firstkey = analyze.smallerkeys.[0]
        let s = analyze.content.[0..firstkey-1]
        let news = (Seq.map (char >> string) s) |> String.concat ""
        printfn "%A"  news
        analyze.BreakString() |> ignore
    0
