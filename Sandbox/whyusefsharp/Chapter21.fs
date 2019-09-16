module Chapter21
    type EmailAddress = EmailAddress of string

    let sendEmail (EmailAddress email) =
        printfn "Sent an email to '%s'" email

    let aliceEmail = EmailAddress "alice@email.com"
    sendEmail aliceEmail

    // sendEmail "email@email.com" // Error porque não é uma string
    let printingExample =
        printf "an int %i" 2
        //printf "an int %i" 2.0 // Error
    
    (* val printAString : string -> unit *)
    let printAString x = printf "%s" x

    (* val printAString : string -> unit *)
    let printAInt x = printf "%i" x

    [<Measure>]
    type cm

    [<Measure>]
    type inches

    [<Measure>]
    type feet =
        static member toInches(feet : float<feet>) : float<inches> =
            feet * 12.0<inches/feet>

    let meter = 100.0<cm>
    let yard = 3.0<feet>

    let yardInInches = feet.toInches(yard)

    // yard + meter
    [<Measure>]
    type GBP

    [<Measure>]
    type USD

    let gbp10 = 10.0<GBP>
    let usd10 = 10.0<USD>

    let gbpTotal = gbp10 + gbp10  // allowed: same currency
    // let gbpusd = gbp10 + usd10 // not allowed
    // let gbpfloat = gbp10 + 1.0
    let gbpunderscore = gbp10 + 5.0<_>

#if CSHARP
using System;
var obj = new Object();
var ex = new Exception();
var equals = (object == ex);
#endif

    [<NoEquality; NoComparison>]
    type CustomerAccount = { CustomerAccountId: int }

    let x = { CustomerAccountId = 1 }
    // x = x // Error
    // x.CustomerAccountId = x.CustomerAccountId
