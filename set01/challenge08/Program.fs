open System
open System.IO

let bytesentropy (b: list<byte>) =
    let f =
        fun (l: list<(bool * int)>) ->
            let n = (List.filter (fun i -> (fst i)) l)

            let r =
                if n.Length > 0 then
                    let v = float (snd n.[0]) / float (b.Length)
                    v * Math.Log2(v)
                else
                    0.0

            r

    let s = Set.ofList b

    let prob =
        seq {
            for n in s do
                f (List.countBy (fun i -> i = n) b)
        }

    let entropy = -Seq.sum prob

    entropy

[<EntryPoint>]
let main argv =
    // One problem of AES in ECB mode is that when blocks of 16 bytes are equal they will produce
    // the same output of 16 bytes. I assumed that the line having the smallest Shannon entropy
    // would be the right one given the problem statement puts emphasis on the previous information.
    //
    // The idea is that I to just do a simple calculation instead of doing analysis 16 bytes at a
    // time for each line. If you actually print all the entropies. The entropy of line 133 is almost
    // 0.5 larger than the next smaller value, thus it is our encrypted line.
    let s2b =
        fun (s: string) ->
            seq {
                for i in 0 .. 2 .. s.Length - 1 do
                    Convert.ToByte(s.[i..i + 1], 16)
            }
            |> List.ofSeq

    if argv.Length >= 1 then
        let mutable ln = 1
        let mutable minimumentropy = Double.PositiveInfinity
        let mutable minimumline = 1
        let lines = File.ReadAllLines argv.[0]

        for line in lines do
            let e = (bytesentropy (s2b line))

            if e < minimumentropy then
                minimumline <- ln
                minimumentropy <- e

            ln <- ln + 1

        printfn
            "Line %d has the smallest entroty (%f) and probably was encrypted with AES in ECB mode"
            minimumline
            minimumentropy

    0
