(* F# language sample *)
module Chapter3
    let fnInverse x = 1.0 / (x)
    
    let fnSumOfInverse n =
        [1.0 .. n] |> List.map fnInverse |> List.sum

#if CSHARP
(* C# language sample *)
public static class SumOfInverse
{
    public static float Operation(float x)
    {
        return 1.0f / x;
    }

    public static float SumOfOperation(int n)
    {
        float sum = 0;
        for(int i = 1; i <= n; i++)
            sum += Operation((float)i);

        return sum;
    }
}
(* C# language sample with LINQ *)
public static class SumOfInverseWithLINQ
{
   public static int SumOfOperation(int n)
   {
      return Enumerable.Range(1, n).Select(i => 1.0 / i).Sum();
   }
}
#endif