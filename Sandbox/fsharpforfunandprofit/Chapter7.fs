(* Type inference *)
module Chapter7
    let Where source predicate =
        Seq.filter predicate source

    let GroupBy source keySelector =
        Seq.groupBy keySelector source

#if CSHARP
    public IEnumerable<TSource> Where<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return source.Wrere(predicate);
    }

    public IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return source.GroupBy(keySelector);
    }
#endif

    let i = 1
    let s = "hello"
    let tuple = s,i
    let s2,i2 = tuple
    let list = [s2]

    (* string list -> int *)
    let sumLengths strList =
        strList |> List.map String.length |> List.sum
