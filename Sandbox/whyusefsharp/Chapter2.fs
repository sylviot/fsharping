(*
    @author: sylviot
    @year: 2019
    @description: Chapter 2 - fsharp in 60 seconds overview
*)

module Chapter2
    // The "let" keyword defines an (immutable) value //
    let sampleInt = 5
    let sampleFloat = 3.14
    let sampleString = "sample string"

    let sample2to5 = [2; 3; 4; 5]
    let sample1to5 = 1 :: sample2to5
    let sample0to10 = [0 ; 1] @ sample2to5 @ [6 .. 10]

    // The "let" keyword also define a named function //
    let fnSquare x = x * x
    let fnAdd x y = x + y

    let fnEvens list = 
        let isEven x = (x % 2 = 0)
        List.filter isEven list

    let fnSumOfSquares list =
        List.sum( List.map fnSquare list )

    // The "|>" piped syntax work right to left read //
    let fnSumOfSquaresPiped list =
        list |> List.map fnSquare |> List.sum

    // Can define Lambdas (anonymous functions) using "fun" //
    let fnSumOfSquaresWithFn list =
        list |> List.map (fun x -> fnSquare x) |> List.sum

    let simplePatternMatch =
        let x = "a"
        match x with
        | "a" -> printfn "x is a"
        | "b" -> printfn "x is b"
        | _ -> printfn "x is something else" (* underscore matches anything *)

    // Some(..) and None are roughly analogous to Nullable wrappers //
    let validValue = Some(999)
    let invalidValue = None

    // In this example, match..with matches the "Some" and the "None" //
    let optionPatternMatch input =
        match input with
        | Some i -> printfn "input is an int=%d" i
        | None -> printfn "input is missing"

    // ==== Complex Data Types ==== //
    let twoTuple = 1, 2
    let threeTuple = "a", 2, true

    // Record types have named fields with semicolons are separators //
    type Person = { First: string; Last: string }
    let person1 = { First = "FirstName"; Last = "LastName" }

    // Union types have choices. Vertical bars are separators //
    type Temp =
        | DegreesC of float
        | DegreesF of float

    let temp = DegreesF 98.6

    type Employee =
        | Worker of Person
        | Manager of Employee list
    
    let worker1 = { First = "FirstName"; Last = "LastName" }
    let worker = Worker worker1
    let managers = Manager [Worker { First = "Employee1"; Last = "LastEmployee2"}; Worker { First = "Employee1"; Last = "LastEmployee2"} ]

    (* Results *)
    let run =
        printfn "Sum of Square in a list: %d" (fnSumOfSquares sample0to10)
        optionPatternMatch validValue