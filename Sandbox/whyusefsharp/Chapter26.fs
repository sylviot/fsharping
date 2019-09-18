module Chapter26
    open System
    open System.Threading

    let createTimer timerInterval eventHandler =
        let timer = new System.Timers.Timer(float timerInterval)
        timer.AutoReset <- true
        timer.Elapsed.Add eventHandler

        async {
            timer.Start()

            printfn "tick"
            do! Async.Sleep 5000

            timer.Stop()
        }

    let basicHandler _ = printfn "tick %A" DateTime.Now
    let basicTimer1 = createTimer 1000 basicHandler

    Async.RunSynchronously basicTimer1


    let createTimerAndObservable timerInterval =
        let timer = new System.Timers.Timer(float timerInterval)
        timer.AutoReset <- true
        let observable = timer.Elapsed
        let task = async {
            timer.Start()

            do! Async.Sleep 5000

            timer.Stop()
        }
        (task, observable)

    let basicTimer2, timerEventStream = createTimerAndObservable 1000

    timerEventStream
    |> Observable.subscribe (fun _ -> printfn "tick %A" DateTime.Now)
    |> ignore

    Async.RunSynchronously basicTimer2


    type ImperativeTimerCount() =
        let mutable count = 0

        member this.handlerEvent _ =
            count <- count + 1
            printfn "timer ticked with count %i" count

    let handler = new ImperativeTimerCount()
    let timerCount1 = createTimer 500 handler.handlerEvent

    Async.RunSynchronously timerCount1

    let timerCount2, timerEventStream2 = createTimerAndObservable 500

    timerEventStream
    |> Observable.scan (fun count _ -> count + 1) 0
    |> Observable.subscribe (fun count -> printfn "timer ticked with count %i" count)
    |> ignore

    Async.RunSynchronously timerCount2

    type FizzBuzzEvent = { label: int; time: DateTime; }

    let areSimultaneous (earlierEvent, laterEvent) =
        let { label = _; time = t1; } = earlierEvent
        let { label = _; time = t2; } = laterEvent
        t2.Subtract(t1).Milliseconds < 50

    type ImperativeFizzBuzzHandler() =
        let mutable previousEvent: FizzBuzzEvent option = None
        let printEvent thisEvent =
            let { label = id; time = t; } = thisEvent
            printf "[%i] %i.%03i " id t.Second t.Millisecond
            let simultaneous = previousEvent.IsSome && areSimultaneous (previousEvent.Value, thisEvent)
            if simultaneous then printfn "FizzBuzz"
            elif id = 3 then printfn "Fizz"
            elif id = 5 then printfn "Buzz"
        member this.handleEvent3 eventArgs =
            let event = { label = 3; time = DateTime.Now; }
            printEvent event
            previousEvent <- Some event
        member this.handleEvent5 eventArgs =
            let event = { label = 5; time = DateTime.Now; }
            printEvent event
            previousEvent <- Some event


    let handlerFB = new ImperativeFizzBuzzHandler()

    (* Imperative
    let timer3 = createTimer 300 handlerFB.handleEvent3
    let timer5 = createTimer 500 handlerFB.handleEvent5

    [ timer3; timer5; ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
    *)

    let timer3, timerEventStream3 = createTimerAndObservable 300
    let timer5, timerEventStream5 = createTimerAndObservable 500

    let eventStream3 =
        timerEventStream3
        |> Observable.map (fun _ -> { label = 3; time = DateTime.Now; })

    let eventStream5 =
        timerEventStream5 
        |> Observable.map (fun _ -> { label = 5; time = DateTime.Now; })

    let combinedStream =
        Observable.merge eventStream3 eventStream5

    let pairwiseStream =
        combinedStream |> Observable.pairwise

    let simultaneousStream, nonSimultaneousStream =
        pairwiseStream |> Observable.partition areSimultaneous

    let fizzStream, buzzStream =
        nonSimultaneousStream
        |> Observable.map (fun (ev1, _) -> ev1)
        |> Observable.partition (fun { label = id; } -> id = 3)

    combinedStream
    |> Observable.subscribe (fun { label = id; time = t; } -> printf "[%i] %i.%03i" id t.Second t.Millisecond)
    |> ignore

    simultaneousStream
    |> Observable.subscribe (fun _ -> printfn "FizzBuzz")
    |> ignore

    fizzStream
    |> Observable.subscribe (fun _ -> printfn "Fizz")
    |> ignore

    buzzStream
    |> Observable.subscribe (fun _ -> printfn "Buzz")
    |> ignore

    [timer3; timer5;]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
