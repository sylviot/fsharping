#if CSHARP
public class NaiveShoppingCart<TItem>
{
    private List<TItem> items;
    private decimal paidAmount;

    public NaiveShoppingCart()
    {
        this.items = new List<TItem>();
        this.paidAmount = 0;
    }

    public bool IsPaidFor
    {
        get => this.paidAmount > 0;
    }

    public IEnumerable<TItem> Items
    {
        get => this.items;
    }

    public void AddItem(TItem item)
    {
        if (!this.IsPaidFor)
        {
            this.items.Add(item);
        }
    }

    public void RemoveItem(TItem item)
    {
        if (!this.IsPaidFor)
        {
            this.items.Remove(item);
        }
    }

    public void Pay(decimal amount)
    {
        if (!this.IsPaidFor)
        {
            this.paidAmount = amout;
        }
    }
}
#endif

module Chapter22
    type CartItem = string

    type EmptyState = NoItems 
    type ActiveState = { UnpaidItems : CartItem list; }
    type PaidForState = { PaidItems : CartItem list; Payment : decimal; }

    type Cart =
    | Empty of EmptyState
    | Active of ActiveState
    | PaidFor of PaidForState

    (* Operations on empty state *)
    let addToEmptyState item =
        Cart.Active { UnpaidItems = [item]; }

    (* Operations on active state *)
    let addToActiveState state itemToAdd =
        let newList = itemToAdd :: state.UnpaidItems
        Cart.Active { state with UnpaidItems = newList; }

    let removeFromActiveState state itemToRemove =
        let newList = state.UnpaidItems |> List.filter ( fun i -> i <> itemToRemove )

        match newList with
        | [] -> Cart.Empty NoItems
        |  _ -> Cart.Active { state with UnpaidItems = newList; }

    let payForActiveState state amount =
        Cart.PaidFor { PaidItems = state.UnpaidItems; Payment = amount; }


    type EmptyState with
        member this.Add = addToEmptyState

    type ActiveState with
        member this.Add = addToActiveState this
        member this.Remove = removeFromActiveState this
        member this.Pay = payForActiveState this

    let addItemToCart cart item =
        match cart with
        | Empty state -> state.Add item
        | Active state -> state.Add item
        | PaidFor state ->
            printfn "ERROR: The cart is paid for"
            cart

    let removeItemFromCart cart item =
        match cart with
        | Empty state ->
            printfn "ERROR: The cart is empty"
            cart // return
        | Active state ->
            state.Remove item
        | PaidFor state ->
            printfn "ERROR: The cart is paid for"
            cart // return

    let displayCart cart =
        match cart with
        | Empty state ->
            printfn "The cart is empty"
        | Active state ->
            printfn "The cart contains %A unpaid items" state.UnpaidItems
        | PaidFor state ->
            printfn "The cart contains %A paid items. Amount paid: %f" state.PaidItems state.Payment

    type Cart with
        static member NewCart = Cart.Empty NoItems
        member this.Add = addItemToCart this
        member this.Remove = removeItemFromCart this
        member this.Display = displayCart this

    let run =
        (* Testing the design Cart *)
        let emptyCart = Cart.NewCart
        printfn "New cart: "; emptyCart.Display

        let cartA = emptyCart.Add "Item 1"
        printfn "Cart A : "; cartA.Display

        let cartAB = cartA.Add "Item 2"
        printfn "Cart AB : "; cartAB.Display

        let cart1 = emptyCart.Remove "Item 2"
        printfn "Cart empty : "; cart1.Display

        let cartAPaid =
            match cartA with
            | Empty _ | PaidFor _ -> cartA
            | Active state -> state.Pay 100m

        printfn "cartAPaid="; cartAPaid.Display
