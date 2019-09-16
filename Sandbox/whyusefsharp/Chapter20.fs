module Chapter20
#if CSHARP
enum State { New, Draft, Published, Inactive, Discontinued }
void HandleState(State state)
{
    switch(state)
    {
        case State.New:
            ...
        case State.Draft:
            ...
        default:
            ...
    }
}
#endif
    
    type State = New | Draft | Published | Inactive | Discontinued
    let handleState state = 
        match state with
        | New -> printfn "New"
        | Draft -> printfn "Draft"

    let getFileInfo filePath =
        let fi = new System.IO.FileInfo(filePath)
        if fi.Exists then Some(fi) else None

    let goodFileInfo = getFileInfo "good.txt"
    let badFileInfo = getFileInfo "bad.txt"

    (* Options (Some and None) *)
    match goodFileInfo with
    | Some fileinfo -> printfn "The file %s exists" fileinfo.FullName
    | None -> printfn "The file doesn't exist"

#if CSHARP
public IList<float> MovingAverages(IList<int> list)
{
    var avereges = new List<float>();
    for(int i = 0; i < list.Count; i++)
    {
        var avg = (list[i] + list[i+1]) / 2; // GAP
        averages.Add(avg);
    }
    
    return averages;
}
#endif

    let rec movingAverages list =
        match list with
        | [] -> []
        | h::x::t -> 
            let avg = (h + x) / 2.0
            avg :: movingAverages(x::t)
        | [_] -> []

    type Result<'a, 'b> =
    | Success of 'a
    | Failure of 'b
    | Indeterminate

    type FileErrorReason =
    | FileNotFound of string
    | UnauthorizedAccess of string * System.Exception

    let performActionOnFile action filePath =
        try
            use sr = new System.IO.StreamReader(filePath:string)
            let result = action sr
            sr.Close()
            Success(result)
        with
            | :? System.IO.FileNotFoundException as ex 
                -> Failure (FileNotFound filePath)
            | :? System.Security.SecurityException as ex 
                -> Failure (UnauthorizedAccess (filePath, ex))

    let middleLayerDo action filePath =
        let fileResult = performActionOnFile action filePath
        fileResult

    let topLayerDo action filePath =
        let fileResult = middleLayerDo action filePath
        fileResult

    let printFirstLineOfFile filePath =
        let fileResult = topLayerDo (fun fs->fs.ReadLine()) filePath

        match fileResult with
        | Success result -> printfn "First line is: '%s'" result
        | Failure reason ->
            match reason with
            | FileNotFound file -> printfn "Fail for: '%s'" file
            | UnauthorizedAccess (file, _) -> printfn "No access for: '%s'" file
    let printLengthOfFile filePath =
        let fileResult = topLayerDo (fun fs -> fs.ReadToEnd().Length) filePath

        match fileResult with
        | Success result -> printfn "Length is: %i" result
        | Failure _ -> printfn "Fail for no specific reason"
