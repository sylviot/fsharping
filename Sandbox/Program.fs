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

    [<Struct>]
    type Shape =
        | Circle of radius: float
        | Square of side: float
        | Triangle of height: float * width: float

    let sampleAddress = Address "Rua xxx"
    let sampleName = Name "Name"
    let sampleSSN = SSN 1234567890

    let unwrapAddress (Address address) = address
    let unwrapName (Name name) = name
    let unwrapSSN (SSN ssn) = ssn

    let sampleCircle = Shape.Circle 3.0
    let sampleTriangle = Shape.Triangle(3., 4.)

    let unwrapShapeCircle (Circle r) = r
    let unwrapShapeTriangle (Triangle(h, w)) = (h*w)

    let main =
        printfn "Name: %s, Address: %s %d" (sampleName |> unwrapName) (sampleAddress |> unwrapAddress) (sampleSSN |> unwrapSSN)
        printfn "Circle: %.2f" (sampleCircle |> unwrapShapeCircle)
        printfn "Triangle: %.2f" (sampleTriangle |> unwrapShapeTriangle)

module PatternMatching =
    type Person = {
        First: string
        Last: string
    }

    type Employee =
        | Engineer of engineer: Person
        | Manager of manager: Person * reports: List<Employee>
        | Executive of executive: Person * reports: List<Employee> * assistant: Employee

    // Function to count the employees in tree below the employee input
    let rec countReports(employee: Employee) = 
        1 + match employee with
            | Engineer(person) -> 0
            | Manager(person, reports) -> reports |> List.sumBy countReports
            | Executive(person, reports, assistant) -> (reports |> List.sumBy countReports) + countReports(assistant)

//module ActivePatterns =
//    let (|Int|_|) = parseInt
//    let (|Double|_|) = parseDouble

//    let printParseResult = function
//        | Int x -> printfn "%d" x
//        | _ -> printfn "Type not found!"

//    let main =
//        printParseResult "1"
//        printParseResult "1.5"

module OptionValues =
    type ZipCode = ZipCode of string
    type Customer = {ZipCode: ZipCode option}
    type IShippingCalculator =
        abstract GetState: ZipCode -> string option
        abstract GetShippingZone: string -> int

    let CustomerShippingZone (calculator: IShippingCalculator, customer: Customer) =
        customer.ZipCode
        |> Option.bind calculator.GetState
        |> Option.map calculator.GetShippingZone



// MAIN //
[<EntryPoint>]
let main argv =
    DiscriminatedUnion.main
    let fibSeq = Seq.unfold (fun (a,b) -> Some(a+b, (b, a+b))) (0,1)
    fibSeq
    |> Seq.iter (fun x -> printfn "%d" x)
    0
