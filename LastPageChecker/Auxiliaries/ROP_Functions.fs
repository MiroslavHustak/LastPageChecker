module ROP_Functions

open System
open System.IO
open System.Drawing
open System.Drawing.Imaging

open Errors
open DiscriminatedUnions
 
    module MyFunctions =
        
        // let tryCatch f exnHandler x = jsem pro svoje pokusy upravil takto...
        let tryWith f1 f2 x =
            try
                try
                   f1 x |> Success
                finally
                   f2 x
            with
            | ex -> Failure ex.Message  
        
        // dalsi muj kod pro moje pokusy
        let deconstructor  =  //FOR TESTING PURPOSES ONLY (testing railway oriented programming features)
            function
            | Success x  ->
                          x                                                       
            | Failure ex -> 
                          ex |> error2
                          Map.empty    
                            
        let deconstructor1 error =  //FOR TESTING PURPOSES ONLY (testing railway oriented programming features)
            function
            | Success x  -> ()                                                                         
            | Failure ex -> ex |> error

        let deconstructor2  =  //FOR TESTING PURPOSES ONLY (testing railway oriented programming features)
            function
            | Success x  ->
                          x                                                       
            | Failure ex -> 
                          ex |> error2
                          Array.empty    
        
        let deconstructor3  =  //FOR TESTING PURPOSES ONLY (testing railway oriented programming features)
            function
            | Success x  ->
                          x                                                       
            | Failure ex ->
                          ex |> error2
                          NotCorrect, new Bitmap(0, 0) //whatever of this particular type 
        
        let deconstructor4  =  //FOR TESTING PURPOSES ONLY (testing railway oriented programming features)
            function
            | Success x  -> 
                          x                                                       
            | Failure ex -> 
                          ex |> error2
                          false  //whatever of this particular type 

        let optionToString param =            
            match param with 
            | Some value ->
                          match value with                             
                          | "" -> "0"
                          | _  -> value               
            | None       -> 
                          do error14()                             
                          String.Empty
        
        let optionToString1 str x = 
            match x with 
            | Some value -> value 
            | None       -> 
                            do error17 str
                            String.Empty        
        
        let optionToArray str (x: 'a[] option) = 
                   match x with 
                   | Some value -> 
                                 value 
                   | None       -> 
                                 do error17 str
                                 Array.empty       

        let optionToEnumerable str (x: Collections.Generic.IEnumerable<string> option) = 
            match x with 
            | Some value -> 
                          value
            | None       -> 
                          do error17 str  
                          Seq.empty
        
        let optionToBitmap str x = 
            match x with 
            | Some value -> 
                          value 
            | None       -> 
                          do error17 str
                          new Bitmap(0, 0) //whatever of this particular type     
                            
        let optionToGraphics str bitmap x = 
            match x with 
            | Some value -> 
                          value 
            | None       -> 
                          do error17 str   
                          Graphics.FromImage(bitmap) //whatever of this particular type    
        
        let optionToEncoder str enc x = 
            match x with 
            | Some value ->
                          value 
            | None       -> 
                          do error17 str
                          new EncoderParameter(enc, 0L) //whatever of this particular type                         
                                 
        (*
        let optionToEnumerable str (x: seq<'a> option) = 
            match x with 
            | Some value -> value
            | None       -> 
                            do error17 str  
                            Seq.empty        

        let optionToFileInfo str (x: FileInfo option) = 
            match x with 
            | Some value -> value
            | None       -> 
                            do error17 str 
                            new FileInfo(String.Empty) //whatever of FileInfo type
       *)