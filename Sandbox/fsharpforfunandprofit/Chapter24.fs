
module Chapter24
    open System
    open Microsoft.FSharp.Control

    let userTimerWithCallback =
        let event = new System.Threading.AutoResetEvent(false)

        let timer = new System.Timers.Timer(2000.0)
        timer.Elapsed.Add(fun _ -> event.Set() |> ignore)

        printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
        timer.Start()

        printfn "Doing something useful while waiting for event"

        event.WaitOne() |> ignore

        printfn "Timer ticked at %O" DateTime.Now.TimeOfDay


    let userTimerWithAsync =
        let timer = new System.Timers.Timer(2000.0)
        let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore

        printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
        timer.Start()

        printfn "Doing something useful while waiting for event"

        Async.RunSynchronously timerEvent

        printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

    let fileWriteWithAsync =
        use stream = new System.IO.FileStream("test.txt", System.IO.FileMode.Create)

        printfn "starting async write"
        let asyncResult = stream.BeginWrite(Array.empty, 0, 0, null, null)

        let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

        printfn "Doing something useful while waiting for write to complete"

        Async.RunSynchronously async

        printfn "Async write completed"

    let sleepWorkFlow = async{
        printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
        do! Async.Sleep 2000
        printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
    }

    Async.RunSynchronously sleepWorkFlow

    let nestedWorkFlow = async{
        printfn "Starting parent"
        let! childWorkFlow = Async.StartChild sleepWorkFlow

        do! Async.Sleep 100
        printfn "Doing something useful while waiting"

        let! result = childWorkFlow

        printfn "Finished parent"
    }

    Async.RunSynchronously nestedWorkFlow

    let testLoop = async{
        for i in [1 .. 100] do
            printf "%i before" i

            do! Async.Sleep 10
            printfn "..after"
    }

    Async.RunSynchronously testLoop

    let cancellationSource = new System.Threading.CancellationTokenSource()

    Async.Start(testLoop, cancellationSource.Token)

    System.Threading.Thread.Sleep(200)

    cancellationSource.Cancel()

    let sleepWorkflowMs ms = async{
        printfn "%i ms workflow started" ms
        do! Async.Sleep ms
        printfn "%i ms workflow finished" ms
    }

    let workflowInSeries = async{
        let! sleep1 = sleepWorkflowMs 1000
        printfn "Finished one"

        let! sleep2 = sleepWorkflowMs 2000
        printfn "Finished two"
    }

//#time
    Async.RunSynchronously workflowInSeries
//#time

    let sleep1 = sleepWorkflowMs 1000
    let sleep2 = sleepWorkflowMs 2000

    (* run then in parallel *)
    [sleep1; sleep2;]
        |> Async.Parallel
        |> Async.RunSynchronously

    (* sync version *)
    open System.Net
    open System.IO

    let fetchUrl url =
        let req = WebRequest.Create(Uri(url))
        use resp = req.GetResponse()
        use stream = resp.GetResponseStream()
        use reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
        printfn "Finished dawnloading %s" url

    let sites = [
        "https://www.bing.com"
    ]

    let result = sites |> List.map fetchUrl

    (* async version *)
    open Microsoft.FSharp.Control.CommonExtensions

    let fetchUrlAsync url =
        async {
            let req = WebRequest.Create(Uri(url))
            use! resp = req.AsyncGetResponse()
            use stream = resp.GetResponseStream()
            use reader = new StreamReader(stream)
            let html = reader.ReadToEnd()

            printfn "finished download %s" url
        }

    sites
    |> List.map fetchUrlAsync
    |> Async.Parallel
    |> Async.RunSynchronously


    let childTask() =
        for i in [1 .. 1000] do
            for i in [1 .. 1000] do
                do "Hello".Contains("H") |> ignore

    childTask()

    let parentTask =
        childTask
        |> List.replicate 20
        |> List.reduce (>>)

    parentTask()

    let childTaskAsync = async{ return childTask() }

    let parentTaskAsync =
        childTaskAsync
        |> List.replicate 20
        |> Async.Parallel


    parentTaskAsync
    |> Async.RunSynchronously

