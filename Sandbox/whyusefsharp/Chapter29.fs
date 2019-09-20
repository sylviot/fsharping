module Chapter29
    (* Classes and interfaces *)
    type IEnumerator<'a> =
        abstract member Current: 'a
        abstract MoveNext : unit -> bool

    [<AbstractClass>]
    type Shape() =
        (* Readonly properties *)
        abstract member Height : int with get
        abstract member Width : int with get
        (* Non virtual method *)
        member this.BoundingArea = this.Height * this.Width
        abstract member Print : unit -> unit
        default this.Print() = printfn "I'm a shape"

    type Rectangle(height:int, width:int) =
        inherit Shape()
        override this.Width = width
        override this.Height = height
        override this.Print () = printfn "I'm a Rectangle"

    let r = Rectangle(2, 3)
    printfn "The height is %i and width is %i" r.Height r.Width
    printfn "The area is %i" r.BoundingArea
    r.Print()

    (* Multiple constructor *)
    type Circle(rad:int) =
        inherit Shape()

        let mutable radius = rad

        override this.Height = radius * 2
        override this.Width = radius * 2

        (* Contructor with default value *)
        new() = Circle(10)

        member this.Radius 
            with get() = radius
            and set(value) = radius <- value
    

    let c1 = Circle()
    printfn "The height is %i and width is %i" c1.Height c1.Width
    let c2 = Circle(2)
    printfn "The height is %i and width is %i" c2.Height c2.Width
    c2.Radius <- 3
    printfn "The height is %i and width is %i" c2.Height c2.Width

    (* Generics *)
    type KeyValuePair<'a, 'b>(key: 'a, value: 'b) =
        member this.Key = key
        member this.Value = value

    type Container<'a, 'b when 'a: equality and 'b :> System.Collections.ICollection> (name: 'a, values: 'b) =
        member this.Name = name
        member this.Values = values

    (* Structs *)
    type Point2D =
        struct
            val x: float
            val y: float
            new(x: float, y: float) = { x = x; y = x; }
        end

    let p = Point2D()
    let p2 = Point2D(2.0, 3.0)

    (* Exception *)
    exception MyError of string
    try
        let e = MyError("Ooops!")
        raise e
    with
        | MyError msg -> printfn "MyError Exception error was %s" msg
        | _ -> printfn "Some error exception"

    (* Extensions methods *)
    type System.String with
        member this.StartsWithA = this.StartsWith "A"

    let s = "Alice"
    printfn "'%s' starts with an 'A' = %A" s s.StartsWithA

    type System.Int32 with
        member this.IsEven = this % 2 = 0

    let i = 20
    if i.IsEven then printfn  "'%i' is even" i

    (* Parameter arrays *)
    open System
    type MyConsole() =
        member this.WriteLine([<ParamArray>] args: Object[]) =
            for arg in args do
                printfn "%A" arg

    let cons = new MyConsole()
    cons.WriteLine("abc", 42, 3.14, true)

    (* Events *)
    type MyButton() =
        let clickEvent = new Event<_>()

        [<CLIEvent>]
        member this.OnClick = clickEvent.Publish

        member this.TestEvent(arg) =
            clickEvent.Trigger(this, arg)

    let myButton = new MyButton()
    myButton.OnClick.Add(fun (sender, arg) -> printfn "Click event with args=%O" arg)
    myButton.TestEvent("Hello Event World!")

    (* Delegates *)
    type MyDelegate = delegate of int -> int
    let f = MyDelegate (fun x -> x * x)
    let result = f.Invoke(5)

    (* Enums *)
    type Color = | Red = 1 | Green = 2 | Blue = 3

    let color1 = Color.Red
    let color2:Color = enum 2

    (* ':?>' is a downcast *)
    let color3 = System.Enum.Parse(typeof<Color>, "Green") :?> Color

    [<System.FlagsAttribute>]
    type FileAccess = | Read = 1 | Write = 2 | Execute = 4
    let fileaccess = FileAccess.Read ||| FileAccess.Write

    printfn "%A" fileaccess

    (*
        open System.Windows.Forms
        let form = new Form(Width = 400, Height = 300)
        form.Click.Add (fun args -> printfn "The form was clicked")
        form.Show()
    *)
