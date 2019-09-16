﻿module Chapter14
    type PersonalName = { FirstName: string; LastName: string; }

    #if CSHARP
    class ImmutablePersonalName
    {
        public ImmutablePersonalName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    #endif

    type USAddress = { Street: string; City: string; State: string; Zip: string; }
    type UKAddress = { Street: string; Town: string; PostCode: string; }
    type Address = 
        | US of USAddress
        | UK of UKAddress
    
    type Person = { Name: string; Address: Address; }

    let alice = { Name = "Alice"; Address = US { Street = "Rua xxx"; City = "NY"; State = "CA"; Zip = "0000"; } }
    let bob = { Name = "Bob"; Address = UK { Street = "Rua 0"; Town = "London"; PostCode = "XXX000"; } }

    printfn "Alice is %A" alice
    printfn "Bob is %A" bob

    let alice1 = { FirstName = "Alice"; LastName = "Adams"; }
    let alice2 = { FirstName = "Alice"; LastName = "Adams"; }
    let bob1 = { FirstName = "Bob"; LastName = "Bishop"; }

    printfn "alice1=alice2 is %A" (alice1 = alice2)
    printfn "alive1=bob1 is %A" (alice1 = bob1)


    type Suit = Club | Diamond | Spade | Heart
    type Rank = Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King | Ace

    let compareCard card1 card2 =
        if card1 < card2
        then printfn "%A is greater than %A" card2 card1
        else printfn "%A is greater than %A" card1 card2

    let aceHearts = Heart, Ace
    let twoHearts = Heart, Two
    let aceSpades = Spade, Ace

    compareCard aceHearts twoHearts
    compareCard twoHearts aceSpades

    let hand = [ 
        Club, Ace;
        Heart, Three;
        Heart, Ace;
        Spade, Jack;
        Diamond, Two;
        Diamond, Ace; 
    ]

    List.sort hand |> printfn "sorted hand is (low to high) %A"

    List.max hand |> printfn "high card is %A"
    List.min hand |> printfn "low card is %A"
