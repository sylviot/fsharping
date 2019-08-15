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


//module Pipeline = 
//    let numbers = [0..100]
//        |> List.filter (fn n -> n % 2 == 0)

let sayName name = 
    sprintf "Hello %s" name


[<EntryPoint>]
let main argv =
    let names = ["Anna"; "Betto"; "Carlos"]
    names
    |> List.map sayName
    |> List.iter (fun name -> printfn "%s - %s" name name)
    let sampleHere = [for i in 0..99 -> (i, i*i)]
    printfn "asdas %A" sampleHere
    0
