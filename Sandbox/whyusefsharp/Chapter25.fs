module Chapter25
    let printerAgent = MailboxProcessor.Start(fun inbox ->
        let rec messageLoop() = async{
            let! msg = inbox.Receive()

            printfn "message is: %s" msg

            return! messageLoop()
        }

        messageLoop()
    )

    let run() =
        printerAgent.Post "hello"
        printerAgent.Post "hello again"
        //printerAgent.PostAndReply(fun a -> "reply") |> printfn "reply: %s"

    open System
    open System.Threading
    open System.Diagnostics

    type Utility() =
        static let rand = new Random()

        static member RandomSleep() =
            let ms = rand.Next(1, 10)
            Thread.Sleep ms

    type LockedCounter () =
        static let _lock = new Object()

        static let mutable count = 0
        static let mutable sum = 0

        static let updateState i =
            sum <- sum + i
            count <- count + 1
            printfn "Count is : %i with sum: %i" count sum
            Utility.RandomSleep()
         
        static member Add i =
            let stopwatch = new Stopwatch()
            stopwatch.Start()

            lock _lock (fun () ->
                stopwatch.Stop()

                printfn "Client waited %i ms" stopwatch.ElapsedMilliseconds

                updateState i
            )

    type MessageBasedCounter () =
        static let updateState (count, sum) msg =
            let newSum = sum + msg
            let newCount = count + 1

            printfn "Count is: %i. Sum is: %i" newCount newSum

            Utility.RandomSleep()

            (newCount, newSum)

        static let agent = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop oldState = async{
                let! msg = inbox.Receive()

                let newState = updateState oldState msg
                printfn "Agent state %A" newState

                return! messageLoop newState
            }

            messageLoop (0, 0)
        )

        static member Add i = agent.Post i

    let run2() = 
        printfn "Start LockedCount"
        LockedCounter.Add 4
        LockedCounter.Add 5

        let makeCountingTask addFunction taskId = async {
            printfn "Task%i" taskId
            
            for i in [1..3] do
                addFunction i
            }

        let task = makeCountingTask LockedCounter.Add 1
        Async.RunSynchronously task

        let lockedSample =
            [1 .. 20]
            |> List.map (fun i -> makeCountingTask LockedCounter.Add i)
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore
        
        printfn "exit LockedCount"

        printfn "Start MessageBased LockedCount"
        MessageBasedCounter.Add 4
        MessageBasedCounter.Add 5

        let task = makeCountingTask MessageBasedCounter.Add 1
        Async.RunSynchronously task

        let messageSample =
            [1 .. 20]
            |> List.map (fun i -> makeCountingTask MessageBasedCounter.Add i)
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore

        printfn "exit MessageBased LockedCount"

        //Async.RunSynchronously lockedSample
    let slowConsoleWrite msg =
        msg |> String.iter (fun ch ->
            Thread.Sleep(1)
            Console.Write ch
        )
        Console.WriteLine()
     
    slowConsoleWrite "abc"

    let makeTask logger taskId = async {
        let name = sprintf "Task %i" taskId
        for i in [1 .. 3] do
            let msg = sprintf "-%s: Loop%i-" name i
            logger msg
    }

    let task = makeTask slowConsoleWrite 1
    Async.RunSynchronously task

    type UnserializedLogger() =
        member this.Log msg = slowConsoleWrite msg

    let runUnserialized =
        printfn "Unserialized"
        let unserializedLogger = UnserializedLogger()
        unserializedLogger.Log "hello"

    let unserializedSample =
        let logger = new UnserializedLogger()
        [1 .. 5]
        |> List.map (fun i -> makeTask logger.Log i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

    // Ugly output

    type SerializedLogger() =
        let agent = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop () = async {
                let! msg = inbox.Receive()

                slowConsoleWrite msg

                return! messageLoop ()
            }

            messageLoop ()
        )
        member this.Log msg = agent.Post msg

    let runSerialized () =
        printfn "Serialized"
        let serializedLogger = SerializedLogger()
        serializedLogger.Log "hello"

        let serializedSample  =
            let logger = new SerializedLogger()
            [1 .. 5]
            |> List.map (fun i -> makeTask logger.Log i)
            |> Async.Parallel
            |> Async.RunSynchronously
            |> ignore

        serializedLogger |> ignore

