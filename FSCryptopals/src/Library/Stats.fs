namespace Library

open System

module Stats =
    let rec chisquare (xored: string): double =
        match xored with
        | xored when String.IsNullOrEmpty xored -> 0.0
        | _ ->
            let c = xored.[0]

            match c with
            | c when int c > 128 -> Double.PositiveInfinity
            | c when Char.IsLower c ->
                let e = snd Constants.ALPHABET.[int c - int 'a']
                ((1.0 - e) ** 2.0) / e + chisquare (xored.[1..])
            | c when Char.IsNumber c -> 5.0 + chisquare (xored.[1..])
            | c when Char.IsWhiteSpace c -> 0.1 + chisquare (xored.[1..])
            | c when Char.IsPunctuation c -> 5.0 + chisquare (xored.[1..])
            | _ -> Double.PositiveInfinity

    let analyzebytes (b: seq<byte []>) =
        let l =
            [ seq { 'a' .. 'z' }
              seq { 'A' .. 'Z' }
              seq { ':' .. '@' }
              seq { ' ' .. '/' }
              seq { '0' .. '9' } ]
            |> Seq.concat

        let mutable bestfit = ('\000', Double.PositiveInfinity, "")

        let bytes =
            Seq.map (fun (i: byte []) -> i.[0]) b
            |> List.ofSeq

        for c in l do
            let xored =
                String.concat ""
                <| (List.map ((fun b -> char (b ^^^ byte c)) >> string) bytes)

            let n = chisquare (xored.ToLower())
            let (_, bn, _) = bestfit
            if n < bn then bestfit <- (c, n, xored)

        bestfit
