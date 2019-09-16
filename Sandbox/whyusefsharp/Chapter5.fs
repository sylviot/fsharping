module Chapter5
    open System
    open System.Net
    open System.IO

    let myCallback (reader:IO.StreamReader) url =
        let html = reader.ReadToEnd()
        printfn "Downloaded %s" url
        printfn "Html %s" (html.Substring(0, 84))
        html

    let fetchUrl callback url =
        let request = WebRequest.Create(Uri(url))
        use response = request.GetResponse()
        use stream = response.GetResponseStream()
        use reader = new StreamReader(stream)
        callback reader url

    let run =
        let sites = ["http://google.com"; "http://www.bing.com"]
        sites |> List.map (fetchUrl myCallback)
