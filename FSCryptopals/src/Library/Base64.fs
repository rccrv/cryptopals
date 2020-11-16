namespace Library

module Base64 =
    let base64encode (str: string) =
        let d =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"

        let mutable r = ""

        for n in Hex.hexstringyieldnextbyte str 3 do
            let d1 = int n.[0] &&& (0b11111100 >>> 2)

            let d2 =
                ((int n.[0] &&& 0b00000011) <<< 4)
                ||| ((int n.[1] &&& 0b11110000) >>> 4)

            let d3 =
                ((int n.[1] &&& 0b00001111) <<< 2)
                ||| ((int n.[2] &&& 0b11000000) >>> 6)

            let d4 = int n.[2] &&& 0b00111111
            r <- r + sprintf "%c%c%c%c" d.[d1] d.[d2] d.[d3] d.[d4]

        r
