(* Four Key Concepts 
    - Function-oriented
    - Expressions
    - Algebric types
    - Pattern matching
*)
module Chapter6
    (* Function-oriented rather than object-oriented *)
    let square x = x * x

    (* function as values *)
    let squareclone = square
    let result = [1..10] |> List.map squareclone

    (* functions taking other functions as parameters *)
    let execFunction aFunc aParam = aFunc aParam
    let result2 = execFunction square 12

    (* Expressions rather than statements *)
    (* 
        SELECT EmployeeName
        FROM Employees
        WHERE EmployeeID IN
            (SELECT DISTINCT ManagerID FROM Employees) -- subquery
    *)

    (* Algebric types are 2 differents way - Product and Sum (disjoint union) *)
    type IntAndBool = { intPart: int; boolPart: bool}
    let x = { intPart = 1; boolPart = false }

    type IntOrBool =
        | IntChoice of int
        | BoolChoice of bool

    let y = IntChoice 42
    let z = BoolChoice false

    (* Pattern-matching 
        match boolExpression with
        | true -> --true branch
        | false -> --false branch

        match aList with
        | [] -> --empty case
        | head::tail -> --first element and rest of the list
    *)
    
    type Shape =
        | Circle of radius:int
        | Rectangle of height:int * width:int
        | Point of x:int * y:int
        | Polygon of pointList:(int * int) list

    let draw shape =
        match shape with
        | Circle radius -> printfn "Circle: with radius %d" radius
        | Rectangle (height, width) -> printfn "Rectangle: %d height and %d width" height width
        | Polygon points -> printfn "Polygon: with points %A" points
        | _ -> printfn "No shape!"

    let circle = Circle(10)
    let rect = Rectangle(2, 2)
    let point = Point(2, 3)
    let polygon = Polygon([(1,1); (1,3); (3, 3); (3, 1)])

    let run =
        [circle; rect; point; polygon] |> List.iter draw
        
