open System
open System.IO
open System.Numerics

let hd s1 s2 =
    let lhd =
        (fun ac c ->
            ac
            + BitOperations.PopCount(uint32 ((uint8 (fst c)) ^^^ (uint8 (snd c)))))

    let sum = Seq.fold lhd 0 (Seq.zip s1 s2)
    sum

type Analyze =
    struct
        val content: list<byte>
        val minsizes: int
        val mutable smallerkeys: array<int>
        val mutable key: array<string>

        new(fname: string, size: int) =
            { content =
                  Convert.FromBase64String(File.ReadAllText fname)
                  |> List.ofArray
              minsizes = size
              smallerkeys = Array.zeroCreate size
              key = Array.zeroCreate size }

        member this.ProcessKeysize(keysize: int) =
            let self = this

            let s =
                seq {
                    for i in 0 .. self.minsizes do
                        (new string(List.map (char) self.content.[(i * keysize)..((i + 1) * keysize - 1)]
                                    |> Array.ofList))
                }
                |> Array.ofSeq

            let indices =
                [| (0, 1)
                   (0, 2)
                   (0, 3)
                   (1, 2)
                   (1, 3)
                   (2, 3) |]

            let v =
                Array.map (fun i -> (float (hd s.[fst i] s.[snd i])) / (float keysize)) indices

            let r = Array.sum v / (float indices.Length)
            r

        member this.ProbableKeysizes(range: seq<int>) =
            let self = this

            let v =
                Seq.map (fun i -> self.ProcessKeysize(i)) range
                |> List.ofSeq

            let mins = (List.sort v).[0..this.minsizes - 1]

            let r =
                seq {
                    for k in 0 .. self.minsizes - 1 do
                        (2 + List.findIndex (fun i -> i = mins.[k]) v)
                }
                |> Array.ofSeq

            Array.Copy(r, this.smallerkeys, this.minsizes)

        member this.TransposeStrings() =
            let self = this

            let rec breakstring (s: string) (keysize: int): list<string> =
                let mutable r = []

                if s.Length < keysize then
                    let lastsize = self.content.Length % keysize
                    if lastsize <> 0 then r <- [ s.[0..lastsize - 1] ] else r <- []
                else
                    r <-
                        [ s.[0..keysize - 1] ]
                        @ breakstring s.[keysize..] keysize

                r

            let transpose (s : string) (keysize : int) =
                let rec transposef (v : array<string>) (element : int) =
                    let mutable r = []
                    if element < v.[v.Length - 1].Length then
                        let se = seq { for i in 0 .. v.Length - 1 do v.[i].[element] } |> Array.ofSeq
                        r <- [se] @ transposef v (element + 1)
                    else
                        let se = seq { for i in 0 .. v.Length - 2 do v.[i].[element] } |> Array.ofSeq
                        r <- [se] @ transposef v.[0..v.Length - 2] (element + 1)
                    r

                let v = breakstring s keysize |> Array.ofList
                printfn "%d, %d, %d" v.Length v.[0].Length keysize
                let r = transposef v 0
                r

            for i in this.smallerkeys do
                let mutable s =
                    (Seq.map (char >> string) this.content)
                    |> String.concat ""

                let t = transpose s i
                printfn "%A" t
    end

[<EntryPoint>]
let main argv =
    if argv.Length >= 1 then
        let analyze = Analyze(argv.[0], 4)
        printfn "%d" analyze.content.Length
        analyze.ProbableKeysizes(seq { 2 .. 40 })
        printfn "%A" analyze.smallerkeys
        analyze.TransposeStrings()

    0
