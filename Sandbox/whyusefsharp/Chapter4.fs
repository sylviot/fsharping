module Chapter4
    let rec quicksort list =
        match list with
        | [] -> []
        | head::tail ->
            let fn op h t = t |> List.filter( (op) h)
            List.concat [
                quicksort( fn (>) head tail)
                [head];
                quicksort( fn (<=) head tail)
            ]

(* Version 2
    quicksort( tail |> List.filter( (>) head ) );
    quicksort( tail |> List.filter( (<=) head ) );
*)

(* Version 1
    let smallerElements =
        quicksort( tail |> List.filter( (>) head ) )
    let largerElements =
        quicksort( tail |> List.filter( (<=) head ) ) // fun e -> e >= head | head <= x'
    List.concat [smallerElements; [head]; largerElements]
*)