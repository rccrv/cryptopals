open Library

let challenge01 =
    let s =
        "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"

    let r = Base64.base64encode s

    r

let challenge02 =
    let s1 = "1c0111001f010100061a024b53535009181c"
    let s2 = "686974207468652062756c6c277320657965"

    let r = Xor.xorbytes s1 s2

    r

[<EntryPoint>]
let main argv =
    let och01 = challenge01
    printfn "%s" och01

    let och02 = challenge02
    printfn "%s" och02

    0
