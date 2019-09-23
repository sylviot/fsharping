module PrimitiveType
    let sampleInteger = 1
    let sampleDecimal = 3.14M
    let sampleFloat = 3.123456789f
    
    let sampleArrayRange = [| 0 .. 99 |]
    let sampleArray = [| 1; 2; |]

    let sampleListRange = [0..99]
    let sampleList = [1; 2; 3]

    let sampleListInt_Int = [for i in 0..99 -> (i, i*i)]
