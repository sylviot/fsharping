#if CSHARP
public List<int> MakeList()
{
    return new List<int> {1,2,3,4,5,6,7,8,9,10};
}

public List<int> OddNumbers(List<int> list) { }

public List<int> EvenNumbers(List<int> list) { }

public void Test()
{
    var odds = OddNumbers(MakeList());
    var evens = EvenNumbers(MakeList());
}

public void RefactoredTest()
{
    var list = MakeList();
    var odds = OddNumbers(list);
    var evens = EvenNumbers(list);
}
#endif

module Chapter19
    let list = [1;2;3;4]
    type PersonalName = { FirstName: string; LastName: string; }
    let john = { FirstName = "John"; LastName = "Doe"; }
    let alice = { john with FirstName = "Alice"; }

    let list1 = [1;2;3;4]
    let list2 = 0::list1
    let list3 = list2.Tail

    System.Object.ReferenceEquals(list1, list3)
