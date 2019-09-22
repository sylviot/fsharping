#nowarn "20"

open System
open System.Windows.Forms
open System.IO

let mutable file = None
let openFile filename = File.ReadAllText(filename)
let saveFile filename contents = File.WriteAllText(filename, contents)

let fnMenu() = (
    new ToolStripMenuItem("Arquivo")
)
let fnMenuChildren(title, fn) = (
    new ToolStripMenuItem(title, null, new EventHandler(fn))
)
let fnTextbox() = (
    let textbox = new TextBox()
    textbox.AcceptsTab <- true
    textbox.Dock <- DockStyle.Fill
    textbox.Location <- Drawing.Point(0, 500)
    textbox.Multiline <- true
    textbox.ScrollBars <- ScrollBars.Vertical
    textbox.Visible <- false
    textbox
)

[<EntryPoint; STAThread>]
let main argv =
    let form = new Form(Width = 400, Height = 300, Text = "Notepad")
    let menu = new MenuStrip()
    let menuFile = fnMenu()

    let textbox = fnTextbox()
    let openFileDialog = new OpenFileDialog()
    let saveFileDialog = new SaveFileDialog()

    openFileDialog.CheckFileExists <- true
    openFileDialog.CheckPathExists <- true

    saveFileDialog.AddExtension <- true
    saveFileDialog.DefaultExt <- ".txt"

    let fnShowTextbox() =
        textbox.Text <- ""
        textbox.Visible <- true
        textbox.Focus()
        |>  ignore

    let fnEdit(filename:string) =
        file <- Some filename
        textbox.Text <- openFile filename
        fnShowTextbox()

    let newFile_click = (fun (obj:Object) (x:EventArgs) -> 
        fnShowTextbox()
    )
    let openFile_click = (fun (obj:Object) (x:EventArgs) ->
        match openFileDialog.ShowDialog() with
            | DialogResult.OK -> fnEdit openFileDialog.FileName
            | _ -> printfn "Nothing"
    )
    let trySave() = 
        match saveFileDialog.ShowDialog() with
            | DialogResult.OK -> saveFile saveFileDialog.FileName textbox.Text
            | _ -> printfn "Nothing"

    let saveFile_click = (fun (obj:Object) (x:EventArgs) ->
        match file with
        | Some file -> saveFile file textbox.Text |> ignore
        | None -> trySave() |> ignore
    )
    let close_click = (fun (obj:Object) (e:EventArgs) -> form.Close())
    
    form.SuspendLayout()

    fnMenuChildren ("Novo", newFile_click) |> menuFile.DropDownItems.Add
    fnMenuChildren ("Abrir", openFile_click) |> menuFile.DropDownItems.Add
    fnMenuChildren ("Salvar", saveFile_click) |> menuFile.DropDownItems.Add
    fnMenuChildren ("Fechar", close_click) |> menuFile.DropDownItems.Add

    menu.Items.Add(menuFile)
    form.Controls.Add(textbox)
    form.Controls.Add(menu)
    
    form.ResumeLayout(false)
    form.PerformLayout()
    form.Show()

    Application.Run form
    0
