namespace Library

module Xor =
    let xorbytes (s1: string) (s2: string) =
        let mutable r = ""

        let b1 = Hex.hexstringyieldnextbyte s1 1
        let b2 = Hex.hexstringyieldnextbyte s2 1

        for n in (Seq.zip b1 b2) do
            r <- r + sprintf "%x" ((fst n).[0] ^^^ (snd n).[0])

        r
