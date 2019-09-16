module Chapter16
    let add x y = x + y
    
    let normalUseAdd = add 1 2

    let add42 = add 42

    let genericLogger anyFunc input =
        printfn "input is %A" input
        let result = anyFunc input
        printfn "result %A" result
        result

    let genericLogger2 before after anyFunc input =
        before input
        let result = anyFunc input
        after result
        result

    let add1 input = input + 1

    genericLogger2
        (fun x -> printf "started with=%i" x)
        (fun x -> printfn " ended with=%i" x)
        add1
        2

    let add1WithConsoleLogging =
        genericLogger2
            (fun x -> printf "input=%i. " x)
            (fun x -> printfn " result=%i" x)
            add1
            // Last parameter NOT defined here yet!

    let result1 = add1WithConsoleLogging 1
    let resultList = [1 .. 5] |> List.map add1WithConsoleLogging
    
    #if CSHARP
    public class GenericLoggerHelper<TInput, TResult>
    {
        public TResult GenericLogger(Action<TInput> before, Action<TResult> after, Func<TInput, TResult> aFunc, TInput input)
        {
            before(input);
            var result = aFunc(input);
            after(result);

            return result;
        }
    }

    [Test]
    public void TesteGenericLogger()
    {
        var sut = new GenericLoggerHelper<int, int>();
        sut.GenericLogger(
            x => Console.Write("input={0}", x),
            x => Console.WriteLine(" result={0}", x),
            x => x + 1,
            1
        );
    }
    #endif
