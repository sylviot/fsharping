#if CSHARP
interface ICalculator
{
    int Calculate(int input);
}

class AddingCalculator: ICalculator
{
    public int Calculate(int input) { return input + 1; }
}

class LoggingCalculator: ICalculator
{
    ICalculator _innerCalculator;

    LoggingCalculator(ICalculator innerCalculator)
    {
        this._innerCalculator = innerCalculator;
    }

    public int Calculate(int input)
    {
        var result = this._innerCalculator.Calculate(input);
        Console.WriteLine("Input {0} with result {1}", input, result);

        return result;
    }
}
#endif


module Chapter15
    let addingCalculator input = input + 1

    let loggingCalculator innerCalculator input =
        let result = innerCalculator input
        printfn "Input %A with result %A" input result
        result

    let add1 input = input + 1
    let times2 input = input * 2

    let genericLogger anyFunc input =
        let result = anyFunc input
        printfn "Input %A with result %A" input result
        result

    let add1WithLogging = genericLogger add1
    let times2WithLogging = genericLogger times2

    let genericTimer anyFunc input =
        let stopwatch = System.Diagnostics.Stopwatch()
        stopwatch.Start()

        let result = anyFunc input
        printfn "Input %A with result %A" input result
        result

    let add1WithTimer = genericTimer add1WithLogging

    (* Strategy - Design Pattern *)
    type Animal(noiseMakingStrategy) =
        member this.MakeNoise =
            noiseMakingStrategy() |> printfn "Making noise %s"

    let meowing() = "Meoww"
    let cat = Animal(meowing)
    cat.MakeNoise

    let woofOrBark() = if (System.DateTime.Now.Second % 2 = 0) then "Woof" else "Bark"

    let dog = Animal(woofOrBark)
    dog.MakeNoise
    dog.MakeNoise
