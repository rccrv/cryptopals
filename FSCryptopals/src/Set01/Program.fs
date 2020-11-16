open System
open Library

let challenge01 =
    let s =
        "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"

    Base64.base64encode s

let challenge02 =
    let s1 = "1c0111001f010100061a024b53535009181c"
    let s2 = "686974207468652062756c6c277320657965"

    Xor.xorbytes s1 s2

let challenge03 =
    let s =
        "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"

    let bestfit =
        Stats.analyzebytes (Hex.hexstringyieldnextbyte s 1)

    let (c, _, r) = bestfit

    sprintf "Decoded by using %c\nResult: %s" c r

let challenge04 =
    let s = "data/4.txt"

    let mutable bestfit = ('\000', Double.PositiveInfinity, "")

    let bytes =
        File.readfile s Constants.FileMode.HexString

    // Make this a function of the Stats module
    for line in bytes do
        let r = Stats.analyzebytes line
        let (_, rn, _) = r
        let (_, bn, _) = bestfit
        if rn < bn then bestfit <- r

    let (c, _, r) = bestfit
    sprintf "Decoded by using %c\nResult: %s" c r

(*
let challenge06 =
    let s = "data/6.txt"

    let bytes = File.readfile s Constants.FileMode.Base64String

    for line in bytes do
        let r = Stats.analyzebytes line
        printfn "===== %A =====" r

    let r = ""

    r

*)

[<EntryPoint>]
let main argv =
    let och01 = challenge01
    printfn "Challenge 01:\n%s\n\n" och01

    let och02 = challenge02
    printfn "Challenge 02:\n%s\n\n" och02

    let och03 = challenge03
    printfn "Challenge 03:\n%s\n\n" och03

    let och04 = challenge04
    printfn "Challenge 04:\n%s" och04

    0
