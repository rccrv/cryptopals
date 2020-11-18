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

let ALPHABET =
    [| ('a', 0.08167)
       ('b', 0.01492)
       ('c', 0.02782)
       ('d', 0.04253)
       ('e', 0.12702)
       ('f', 0.02228)
       ('g', 0.02015)
       ('h', 0.06094)
       ('i', 0.06966)
       ('j', 0.00153)
       ('k', 0.00772)
       ('l', 0.04025)
       ('m', 0.02406)
       ('n', 0.06749)
       ('o', 0.07507)
       ('p', 0.01929)
       ('q', 0.00095)
       ('r', 0.05987)
       ('s', 0.06327)
       ('t', 0.09056)
       ('u', 0.02758)
       ('v', 0.00978)
       ('w', 0.02360)
       ('x', 0.00150)
       ('y', 0.01974)
       ('z', 0.00074) |]

let rec chisquare (xored: string): double =
    match xored with
    | xored when String.IsNullOrEmpty xored -> 0.0
    | _ ->
        let c = xored.[0]

        match c with
        | c when int c > 128 -> Double.PositiveInfinity
        | c when Char.IsLower c ->
            let e = snd ALPHABET.[int c - int 'a']
            ((1.0 - e) ** 2.0) / e + chisquare (xored.[1..])
        | c when Char.IsNumber c -> 5.0 + chisquare (xored.[1..])
        | c when Char.IsWhiteSpace c -> 0.1 + chisquare (xored.[1..])
        | c when Char.IsPunctuation c -> 5.0 + chisquare (xored.[1..])
        | _ -> Double.PositiveInfinity

let analyzestring (bytes: list<byte>) =
    let l =
        [ seq { 'a' .. 'z' }
          seq { 'A' .. 'Z' }
          seq { ':' .. '@' }
          seq { ' ' .. '/' }
          seq { '0' .. '9' } ]
        |> Seq.concat

    let mutable bestfit = ('\000', Double.PositiveInfinity, "")

    for c in l do
        let xored =
            String.concat ""
            <| (List.map ((fun b -> char (b ^^^ byte c)) >> string) bytes)

        let n = chisquare (xored.ToLower())
        let (_, bn, _) = bestfit
        if n < bn then bestfit <- (c, n, xored)

    bestfit

type Analyze =
    struct
        val content: list<byte>
        val minsizes: int
        val mutable smallerkeys: array<int>

        new(fname: string, size: int) =
            { content =
                  Convert.FromBase64String(File.ReadAllText fname)
                  |> List.ofArray
              minsizes = size
              smallerkeys = Array.zeroCreate size }

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
            let mutable key = []
            let s2b = fun (s : string) ->
                seq {
                    for i in 0 .. s.Length - 1 do
                        byte s.[i]
                }
                |> List.ofSeq

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

            let rec transpose (v: array<string>) (keysize: int) (element: int) =
                match element with
                | element when element >= v.[v.Length - 1].Length
                               && element < keysize ->
                    let f =
                        fun e ->
                            Array.map (fun (c: string) -> string c.[e]) v.[0..v.Length - 2]
                            |> List.ofArray

                    [ ("", (f element)) |> System.String.Join ]
                    @ transpose v keysize (element + 1)
                | element when element < v.[v.Length - 1].Length ->
                    let f =
                        fun e ->
                            Array.map (fun (c: string) -> string c.[e]) v.[0..v.Length - 1]
                            |> List.ofArray

                    [ ("", (f element)) |> System.String.Join ]
                    @ transpose v keysize (element + 1)
                | _ -> []


            for i in this.smallerkeys do
                let s =
                    (Seq.map (char >> string) this.content)
                    |> String.concat ""

                let mutable k = ""

                let v = breakstring s i |> Array.ofList
                let r = transpose v i 0

                for j in r do
                    let bestfit = analyzestring (s2b j)
                    let (c, _, _) = bestfit
                    k <- k + (string c)

                key <- key @ [ k ]

            key
    end

let unxorrepeatedkey (s: string) (key: string) =
    let mutable r = ""

    let cycle =
        Seq.init s.Length (fun i -> byte key.[i % key.Length])

    let iter = Seq.zip (s |> Seq.map (byte)) cycle

    for i in iter do
        r <- r + sprintf "%02x" ((fst i) ^^^ (snd i))

    r

[<EntryPoint>]
let main argv =
    if argv.Length >= 1 then
        let analyze = Analyze(argv.[0], 4)
        analyze.ProbableKeysizes(seq { 2 .. 40 })
        let keys = analyze.TransposeStrings()

        let s =
            (Seq.map (char >> string) analyze.content)
            |> String.concat ""

        for k in keys do
            if k.Trim() <> "" then
                let key = k
                unxorrepeatedkey s key |> ignore
                printfn "Keysize: %d\nKey: %s" key.Length key

    0
