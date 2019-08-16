open System

module PrimitiveType = 
    let sampleInteger = 1
    let sampleDouble = 3.14
    let sampleArrayRange = [| 0 .. 99 |]
    let sampleArray = [| 1; 2; |]
    let sampleListRange = [0..99]
    let sampleList = [1; 2; 3]
    let sampleHere = [for i in 0..99 -> (i, i*i)]

module Booleans = 
    let sampleTrue = true
    let sampleFalse = false
    let sampleNot = not true
    let sampleAnd = true && true
    let sampleOr = true || true

module String = 
    let sampleString = "value"
    let sampleStringIgnoreScape = @"value\value"
    let sampleLiterals = """Write "values" literal"""
    let sampleSubstring = "abcedf".[0..3]

module Tuples = 
    let tuple = (1, 2, 3)
    let swap (a, b) = (b, a)
    let sampleStructTuple = struct(1, 2)

module Sequences =
    let sampleSeqEmpty = Seq.empty
    let sampleSeqLazy = seq{ yield "a"; yield "b"; yield "c"; }
    let sampleSeqRange = seq{1..99}

module Recursive =
    let rec factorial n =
        if n = 0 then 1 else n * factorial (n - 1)

    let rec fnSumList xs =
        match xs with
        | [] -> 0
        | y::ys -> y + fnSumList ys

module RecordTypes =
    [<Struct>]
    type ContactCardStruct = { Name: string; Phone: string; Verified: bool }

    type ContactCard = { Name: string; Phone: string; Verified: bool }
    let contact1 = { Name = "Anna"; Phone = "000"; Verified = true }
    let contact2 = { Name = "Betto"; Phone = "999"; Verified = false }
    let contact1Fix = { contact1 with Phone = "111"; Verified = false }

    let showContactCard (c: ContactCard) =
        "Name: " + c.Name + " Phone: " + c.Phone + (if not c.Verified then " (unverified) " else " (Verified) ") + c.Phone
    printfn "Contact Anna: %s" (showContactCard contact1)

module DiscriminatedUnion =
    type Address = Address of string
    type Name = Name of string
    type SSN = SSN of int

    let sampleAddress = Address "Rua xxx"
    let sampleName = Name "Name"
    let sampleSSN = SSN 1234567890

    let unwrapAddress (Address address) = address
    let unwrapName (Name name) = name
    let unwrapSSN (SSN ssn) = ssn
    let main =
        printfn "Name: %s, Address: %s %d" (sampleName |> unwrapName) (sampleAddress |> unwrapAddress) (sampleSSN |> unwrapSSN)


// MAIN //
[<EntryPoint>]
let main argv =
    DiscriminatedUnion.main
    0
