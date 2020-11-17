open System
open System.IO

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

let analyzestring (s: string) =
    let l =
        [ seq { 'a' .. 'z' }
          seq { 'A' .. 'Z' }
          seq { ':' .. '@' }
          seq { ' ' .. '/' }
          seq { '0' .. '9' } ]
        |> Seq.concat

    let mutable bestfit = ('\000', Double.PositiveInfinity, "")

    let bytes =
        seq {
            for i in 0 .. 2 .. s.Length - 1 do
                Convert.ToByte(s.[i..i + 1], 16)
        }
        |> List.ofSeq

    for c in l do
        let xored =
            String.concat ""
            <| (List.map ((fun b -> char (b ^^^ byte c)) >> string) bytes)

        let n = chisquare (xored.ToLower())
        let (_, bn, _) = bestfit
        if n < bn then bestfit <- (c, n, xored)

    bestfit

[<EntryPoint>]
let main argv =
    let mutable bestfit = ('\000', Double.PositiveInfinity, "")

    if argv.Length >= 1 then
        let lines = File.ReadAllLines argv.[0]

        for line in lines do
            let r = analyzestring line
            let (_, rn, _) = r
            let (_, bn, _) = bestfit
            if rn < bn then bestfit <- r

    let (c, _, r) = bestfit
    printfn "Decoded by using %c\nResult: %s" c r
    0
