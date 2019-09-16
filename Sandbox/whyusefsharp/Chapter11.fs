module Chapter11
    let add2 x = x + 2
    let mult3 x = x * 3
    let square x = x * x

    [1 .. 10] |> List.map add2 |> printfn "%A"
    [1 .. 10] |> List.map mult3 |> printfn "%A"
    [1 .. 10] |> List.map square |> printfn "%A"

    let add2ThenMult3 = add2 >> mult3
    let mult3ThenSquare = mult3 >> square

    // let add2ThenMult3 x = mult3 ( add2 x )
    // let mult3ThenSquare x = square ( mult3 x )

    let logMsg msg x = printf "%s%i" msg x; x
    let logMsgN msg x = printfn "%s%i" msg x; x
    let mult3ThenSquareLogged =
        logMsg "before="
        >> mult3
        >> logMsg " after mult3="
        >> square
        >> logMsgN " result="

    mult3ThenSquare 5
    [1 .. 10] |> List.map mult3ThenSquareLogged

    let listOfFunctions = [
        mult3;
        square;
        add2;
        logMsgN "result=";
    ]

    let allFunctions = List.reduce (>>) listOfFunctions

    (* Domain-specific laguages DSL's *)
    type DateScale = Hour | Hours | Day | Days | Week | Weeks
    type DateDirection = Ago | Hence

    let getDate interval scale direction =
        let absHours = match scale with 
            | Hour | Hours -> 1 * interval
            | Day | Days -> 24 * interval
            | Week | Weeks -> 24 * 7 * interval
        let signedHours = match direction with
            | Ago -> -1 * absHours
            | Hence -> absHours

        System.DateTime.Now.AddHours(float signedHours)

    let example1 = getDate 1 Days Ago
    let example2 = getDate 1 Hour Hence

#if CSHARP
FluentShape.Default
    .SetColor("red")
    .SetLabel("box")
    .OnClick( s => Console.Write("clicked") );
#endif

    type FluentShape = {
        label : string;
        color : string;
        onClick : FluentShape->FluentShape
    }

    let defaultShape =
        { label = ""; color = ""; onClick = fun shape->shape }

    let click shape =
        shape.onClick shape

    let display shape =
        printfn "My label=%s and my color=%s" shape.label shape.color
        shape

    let setLabel label shape =
        { shape with FluentShape.label = label }
    let setColor color shape =
        { shape with FluentShape.color = color }
    let appendClickAction action shape =
        { shape with FluentShape.onClick = shape.onClick >> action }

    printfn "==== set color box ===="
    let setRedBox = setColor "red" >> setLabel "box"
    let setBlueBox = setRedBox >> setColor "blue"
    let changeColorOnClick color = appendClickAction ( setColor color )

    let redBox = defaultShape |> setRedBox
    let blueBox = defaultShape |> setBlueBox

    redBox
        |> display
        |> changeColorOnClick "green"
        |> click
        |> display

    printfn "------"
    
    blueBox
        |> display
        |> appendClickAction ( setLabel "box2" >> setColor "green" )
        |> click
        |> display
    printfn "==== set color box ===="

    let run =
        let rainbow =
            ["red";"orange";"yellow";"green";"blue";"indigo";"violet"]
        let showRainbow =
            let setColorAndDisplay color = setColor color >> display
            rainbow
                |> List.map setColorAndDisplay
                |> List.reduce (>>)

        defaultShape |> showRainbow
