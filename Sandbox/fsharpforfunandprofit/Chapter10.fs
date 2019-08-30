(* Using functions to extract boilerplate code *)
module Chapter10
#if CSHARP
public static int Product(int n)
{
    int product = 1;
    for (int i = 1; i <= n; i++)
    {
        product *= i;
    }
    return product;
}
public static int SumOfOdds(int n)
{
    int sum = 0;
    for (int i = 1; i <= n; i++)
    {
        if (i % 2 != 0) { sum += i; }
    }
    return sum;
}
public static int AlternatingSum(int n)
{
    int sum = 0;
    bool isNeg = true;
    for (int i = 1; i <= n; i++)
    {
        if (isNeg)
        {
            sum -= i;
            isNeg = false;
        }
        else
        {
            sum += i;
            isNeg = true;
        }
    }
    return sum;
}
#endif
    let product n =
        let initialValue = 1
        let action productSoFar x = productSoFar * x
        [1 .. n] |> List.fold action initialValue

    let sumOfOdds n =
        let initialValue = 0
        let action sumSoFar x = if x%2=0 then sumSoFar else sumSoFar+x
        [1 .. n] |> List.fold action initialValue

    let alternatingSum n =
        let initialValue = (true, 0)
        let action (isNeg, sumSoFar) x = if isNeg then (false, sumSoFar-x) else (true, sumSoFar+x)
        [1 .. n] |> List.fold action initialValue |> snd

    (* Example of fold *)
    let sumOfSquaresWithFold n =
        let initialValue = 0
        let action sumSoFar x = sumSoFar + (x*x)
        [1 .. n] |> List.fold action initialValue
#if CSHARP
/* Re-implementation of method using "fold" aggregate in c# */
public static int ProductWithAggregate(int n)
{
    var initialValue = 1;
    Func<int, int, int> action = (productSoFar, x) =>
        productSoFar * x;

    return Enumerable.Range(1, n).Aggregrate(initialValue, action);
}
public static int SumOfOddsWithAggregate(int n)
{
    var initialValue = 0;
    Func<int, int, int> action = (sumSoFar, x) =>
        (x %2 == 0) ? sumSoFar : sumSoFar + x;

    return Enumerable.Range(1, n).Aggregate(initialValue, action);
}
public static int AlternatingSumsWithAggregate(int n)
{
    var initialValue = Tuple.Create(true, 0);
    Func<Tuple<bool, int>, int, Tuple<bool, int>> action =
        (t, x) => t.Item1
            ? Tuple.Create(false, t.Item2 - x)
            : Tuple.Create(true, t.Item2 + x);

    return Enumerable.Range(1, n).Aggregate(initialValue, action).Item2;
}
#endif

#if CSHARP
public class NameAndSize
{
    public string Name;
    public int Size;
}

public static NameAndSize MaxNameAndSize(IList<NameAndSize> list)
{
    if(list.Count() == 0)
    {
        return default(NameAndSize);
    }

    var maxSoFar = list[0];
    foreach (var item in list)
    {
        if (item.Size > maxSoFar.Size)
        {
            maxSoFar = item;
        }
    }

    return maxSoFar;
}
public static NameAndSize MaxNameAndSizeWithLinq(IList<NameAndSize> list)
{
    if (!list.Any())
    {
        return default(NameAndSize);
    }

    var initialValue = list[0];
    Func<NameAndSize, NameAndSize, NameAndSize> action =
        (maxSoFar, x) => x.Size > maxSoFar.Size ? x : maxSoFar;

    return list.Aggregate(initialValue, action);
}
#endif

    
    type NameAndSize = { Name: string; Size: int }

    (* Example of implementation in F# *)
    let maxNameAndSize list = 
        let innerMaxNameAndSize initialValue rest =
            let action maxSoFar x = if maxSoFar.Size < x.Size then x else maxSoFar
            rest |> List.fold action initialValue

        match list with
        | [] -> None
        | head::tail -> 
            let max = innerMaxNameAndSize head tail
            Some max

    (* Re-implementation with None and Some *)
    let maxNameAndSizeRealWorld list =
        match list with
        | [] -> None
        | _ -> 
            let max = list |> List.maxBy ( fun item -> item.Size )
            Some max

    let run =
        let list = [
            { Name = "Alice"; Size = 1 }
            { Name = "Bob"; Size = 2 }
            { Name = "Carol"; Size = 3 }
            { Name = "David"; Size = 4 }
        ]

        (* Using example code *)
        maxNameAndSize list
        maxNameAndSize []

        (* Implementation with Real F# *)
        list |> List.maxBy ( fun item -> item.Size )
        [] |> List.maxBy ( fun item -> item.Size )
    
    
