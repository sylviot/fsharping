module Chapter28
    (* Using TryParse and TryGetValue *)
    let (i1success, i1) = System.Int32.TryParse("123")
    if i1success then printfn "parsed as %i" i1 else printfn "parse failed"

    let (d1success, d1) = System.DateTime.TryParse("1/1/1980")

    let dict = new System.Collections.Generic.Dictionary<string, string>()
    dict.Add("A", "Alô")
    let (e1success, e1) = dict.TryGetValue("A")

    (* Named arguments to help type inference*)
    //let createReader filename = new System.IO.StreamReader(filename) -- error FS 0041 unique overload
    let createReader1 (filename:string) = new System.IO.StreamReader(filename)
    let createReader2 filename = new System.IO.StreamReader(path = filename)

    (* Active patterns for .NET functions *)
    let (|Digit|Letter|Whitespace|Other|) ch =
        if System.Char.IsDigit(ch) then Digit
        elif System.Char.IsLetter(ch) then Letter
        elif System.Char.IsWhiteSpace(ch) then Whitespace
        else Other

    let printChar ch =
        match ch with
        | Digit -> printfn "%c is a Digit" ch
        | Letter -> printfn "%c is a Letter" ch
        | Whitespace -> printfn "%c is a Whitespace" ch
        | _ -> printfn "%c is other" ch


    (* open System.Data.SqlClient

    let (|ConstraintException|ForeignKeyException|Other|) (ex:SqlException) =
        if ex.Number = 2061 then ContraintException
        elif ex.Number = 2627 then ContraintException
        elif ex.Number = 547 then ForeignKeyException
        else Other

    let executaNonQuery (sqlCommand:SqlCommand) =
        try
            let result = sqlCommand.ExecuteNonQuery()
        with
        | :? SqlException as sqlException
            match sqlException with
                | ConstraintException -> // handle constraint error
                | ForeignKeyException -> // handle FK error
                | _ -> reraise() // don't handle any other cases
    *)

    (* Creating objects directly from an interface *)
    let makeResource name = {
        new System.IDisposable with
            member this.Dispose() = printfn "%s disposed" name
    }

    let useAndDisposeResources =
        use r1 = makeResource "first resource"
        printfn "using first resource"
        for i in [1 .. 3] do
            let resourceName = sprintf "\t inner resource %d" i
            use temp = makeResource resourceName
            printfn "\tdo something with %s" resourceName
        use r2 = makeResource "second resource"
        printfn "using second resource"
        printfn "done."

    (* Mixing .NET interfaces with pure F# types *)
    type IAnimal =
        abstract member MakeNoise : unit -> string

    let showTheNoiseAnAnimalMakes (animal: IAnimal) =
        animal.MakeNoise() |> printfn "Making noise %s"

    type Cat = Felix | Socks
    type Dog = Butch | Lassie

    type Cat with
        member this.AsAnimal = {
            new IAnimal with
                member a.MakeNoise() = "Meow"
        }

    type Dog with
        member this.AsAnimal = {
            new IAnimal with
                member a.MakeNoise() = "Woof"
        }

    let dog = Lassie
    showTheNoiseAnAnimalMakes (dog.AsAnimal)

    let cat = Felix
    showTheNoiseAnAnimalMakes (cat.AsAnimal)

    (* Using reflection to example F# types *)
    open System.Reflection
    open Microsoft.FSharp.Reflection

    type Account = { Id: int; Name: string; }
    
    let fields =
        FSharpType.GetRecordFields(typeof<Account>)
        |> Array.map (fun propInfo -> propInfo.Name, propInfo.PropertyType.Name)

    type Choices = | A of int | B of string

    let choices =
        FSharpType.GetUnionCases(typeof<Choices>)
        |> Array.map (fun choiceInfo -> choiceInfo.Name)
