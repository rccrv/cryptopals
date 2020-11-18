open System
open System.IO
open System.Security.Cryptography

let decrypt (cipherText: byte []) (key: byte []): string =
    use aes = Aes.Create()
    aes.Mode <- CipherMode.ECB

    let decryptor =
        aes.CreateDecryptor(key, Array.zeroCreate 16)

    use msDecrypt = new MemoryStream(cipherText)

    use csDecrypt =
        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

    use srDecrypt = new StreamReader(csDecrypt)
    srDecrypt.ReadToEnd()

[<EntryPoint>]
let main argv =
    let key = "YELLOW SUBMARINE"B

    if argv.Length >= 1 then
        let bytes =
            Convert.FromBase64String(File.ReadAllText argv.[0])

        let r = decrypt bytes key
        printfn "%s" r

    0
