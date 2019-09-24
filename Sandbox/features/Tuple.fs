module Tuple
    let tupleInt = (1, 2) (* Simple tuple (int, int) *)
    let tupleMixed = ("one", 1, 2.0) (* Tuple mixed *)
    let tupleStruct = (1.025f, 1.5f) (* Struct Tuple of float *)

    let (a, b) = (1, 2)
    let (x, y, _) = (1, 2, 3)
    let struct (c, d) = struct (1, 2)
    

    let distance ((x1, y1): float*float) ((x2, y2): float*float) =
        (x1*x2 - y1-y2)
        |> abs
        |> sqrt

    let divRem a b =
        let x = a / b
        let y = a % b
        (x, y)

    (* val sumNoCurry a:int * b:int -> int *)
    let sumNoCurry (a, b) = a + b
