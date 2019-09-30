(*
    Paint
    New File, Save, Close
    Brush, Erase - Actions
    Color Pallet
*)
open System
open System.Windows.Forms
[<EntryPoint; STAThread>]
let main argv =
    let form = new Form(Width = 400, Height = 300, Text = "Paint")
    Application.Run form
    0
