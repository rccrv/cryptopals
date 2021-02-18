open System
open System.IO
open System.Security.Cryptography

// TODO: Well, I have to make encrypt and decrypt pring the initial text.
let encrypt (plainText: byte []) (key: byte []): string =
    use aes = Aes.Create()
    aes.Mode <- CipherMode.ECB

    let encryptor =
        aes.CreateEncryptor(key, Array.zeroCreate 16)

    use msEncrypt = new MemoryStream(plainText)

    let csEncrypt =
        new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Read)

    use srEncrypt = new StreamReader(csEncrypt)
    srEncrypt.ReadToEnd()

let decrypt (cipherText: byte []) (key: byte []): string =
    use aes = Aes.Create()
    aes.Mode <- CipherMode.ECB
    aes.Padding <- PaddingMode.Zeros

    let decryptor =
        aes.CreateDecryptor(key, Array.zeroCreate 16)

    use msDecrypt = new MemoryStream(cipherText)

    let csDecrypt =
        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

    use srDecrypt = new StreamReader(csDecrypt)
    srDecrypt.ReadToEnd()

let xor (s: string) (key: string) =
    let mutable r = ""

    let cycle =
        Seq.init s.Length (fun i -> byte key.[i % 3])

    let iter = Seq.zip (s |> Seq.map (byte)) cycle

    for i in iter do
        r <- r + sprintf "%02x" ((fst i) ^^^ (snd i))

    r

[<EntryPoint>]
let main argv =
    let mymessagetext = "ThisThisThisThis"B
    //printfn "%d" mymessagetext.Length
    let key = "YELLOW SUBMARINE"B
    let en = encrypt mymessagetext key
    let mutable bytes =  System.Text.Encoding.ASCII.GetBytes en
    //printfn "%d" bytes.Length
    if bytes.Length % 16 <> 0 then
        bytes <- Array.append bytes (Array.zeroCreate (16 - bytes.Length % 16))
    //printfn "%d" bytes.Length
    let de = decrypt bytes key
    let iv =
        seq {
            for c in "fake 0th ciphertext block" do
                byte c
        }
        |> Array.ofSeq

    //printfn "%A" iv
    //printfn "%A" en
    printfn "%A" mymessagetext
    let output = System.Text.Encoding.Unicode.GetBytes de
    printfn "%A" output
    printfn "%d" mymessagetext.Length
    printfn "%d" de.Length
    //printfn "%d" en.Length
    0
