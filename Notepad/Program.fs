// Learn more about F# at http://fsharp.org

open System
open System.Windows.Forms

[<EntryPoint; STAThread>]
let main argv =
    let form = new Form(Width = 400, Height = 300, Text = "Notepad")
    let windowNewMenu_Click = (fun (obj:Object) (x:EventArgs) -> printfn "Clicked")
    let arquivoFechar_Click = (fun (obj:Object) (x:EventArgs) -> form.Close())

    let ms = new MenuStrip()
    let arquivo = new ToolStripMenuItem("Arquivo")
    let arquivoNovo = new ToolStripMenuItem("Novo", null, new EventHandler(windowNewMenu_Click))
    let arquivoSalvar = new ToolStripMenuItem("Salvar", null, new EventHandler(windowNewMenu_Click))
    let arquivoFechar = new ToolStripMenuItem("Fechar", null, new EventHandler(arquivoFechar_Click))
    arquivo.DropDownItems.Add(arquivoNovo) |> ignore
    arquivo.DropDownItems.Add(arquivoSalvar) |> ignore
    arquivo.DropDownItems.Add(arquivoFechar) |> ignore

    ms.Dock <- DockStyle.Top
    ms.Items.Add(arquivo) |> ignore
    

    let textbox = new TextBox()
    textbox.AcceptsTab <- true
    textbox.Dock <- DockStyle.Fill
    textbox.Location <- Drawing.Point(0, 500)
    textbox.Multiline <- true
    textbox.ScrollBars <- ScrollBars.Vertical

    form.SuspendLayout()

    form.Controls.Add(textbox)
    form.Controls.Add(ms)
    
    form.ResumeLayout(false)
    form.PerformLayout()
    form.Show()

    Application.Run form
    0
