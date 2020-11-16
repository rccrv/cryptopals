namespace Library

open System

module Hex =
    let hexstringyieldnextbyte (str: string) (step: int) =
        let bytes = Convert.FromHexString(str)

        seq {
            for i in 0 .. step .. bytes.Length - 1 do
                yield! seq { bytes.[i..i + step - 1] }
        }
