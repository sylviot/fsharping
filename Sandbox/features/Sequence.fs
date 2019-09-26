(*
    Feature: Sequences
    Description: A sequence is a logical series of elements "all of one type". 
    Sequence are particularly useful when you have a large, ordered collection of data but do not necessarily expect to use all of the elements.
*)
module Sequence
    let sampleSeqEmpty = Seq.empty
    let sampleSeqLazy = seq { yield "a"; yield "b"; }
    let sampleSeqRange = seq { 1 .. 99 }
    let sampleSeqFor = seq { for i in 1 .. 10 -> i * i }

    let (height, width) = (10, 10)
    let setFor2Yield = seq {
        for row in 0 .. width - 1 do
            for col in 0 .. height - 1 do
                yield (row, col, row * width + col)
    }
    
    let isprime n = n % 2 = 0
    let seqForYield = seq { for n in 1 .. 100 do if isprime n then yield n }
    Seq.iter (fun e -> printf "%d " e) seqForYield

    (* Sequences using yield! *)
    type Tree<'a> =
        | Tree of 'a * Tree<'a> * Tree<'a>
        | Leaf of 'a

    let rec inorder tree = seq {
        match tree with
            | Tree(x, left, right) ->
                yield! inorder left
                yield x
                yield! inorder right
            | Leaf x -> yield x
    }

    let subtree = Tree(6, Leaf(1), Leaf(3))
    let mytree = Tree(6, subtree, Leaf(9))
    let seqtree = inorder mytree
    printfn "%A" seqtree