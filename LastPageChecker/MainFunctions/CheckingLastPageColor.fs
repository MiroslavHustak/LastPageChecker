module CheckingLastNameColor

open Helpers
open Settings
open ColorRecogniser
open Settings.MySettings
open DiscriminatedUnions
open ROP_Functions.MyFunctions

//******* DEFINITIONS OF FREQUENTLY CALLED FUNCTIONS       
//function 1
let private stringChoice x = MyString.GetString((rcO.numberOfScannedFileDigits - String.length (x |> string)), rcO.stringZero)

//function 2
let private myKey =  
    let key x y = sprintf "%s%s%s" 
                  <| rcO.prefix
                  <| string x 
                  <| string y  
    stringChoice >> key 

//******* DEFINITIONS OF TWO SUBMAIN FUNCTIONS
//1
//impure kvuli vystupu na console
let private getLists low high (myMap: Map<string, int>) = 
          
    let getOption i =        
        let aux = high - low + 1 
        (<) (i + 1) aux       
        |> function  
            | true  -> myMap 
                       |> Map.tryFind (myKey <| low + 1 <| low + (i + 1)) //viz IrfanViewOpener                                 
                       |> Option.bind (fun value -> Some (value, (i + 1)))  
            | false -> None     
    
    let numberOfFilesList = List.unfold getOption (-1)     //viz IrfanViewOpener     
                                           
    let endFilesToOpenList = numberOfFilesList |> List.scan (+) 0 |> List.skip 1
                                               
    numberOfFilesList, endFilesToOpenList        

//2
//impure kvuli vystupu na console a spusteni IrfanView
let private compareColorOfLastScannedImages (listOfFiles: string list) ((numberOfFilesList, endFilesToOpenList): int list * int list) low high =  //NEVYDEDUKOVAL :-)
       
    let listOfFilesLength = listOfFiles.Length     
    (*   
     Breaking from a cycle
     For example, tryFind function returns the first value from a sequence for which a given predicate returns true, which lets you write something like this:

     seq { 0 .. 100 } |> Seq.tryFind (fun i ->
                                             printfn "%d" i
                                             i = 66 //condition
                                      )
    *)
    
    endFilesToOpenList 
    |> List.tryFind (fun item ->           
                              let pathWithArgument() =  //legacy name
                                  match (item - 1) >= listOfFilesLength with
                                  | true -> 
                                          let path = string <| listOfFiles.Item (listOfFilesLength - 1) 
                                          path 
                                  | false -> 
                                          let path = string <| listOfFiles.Item (item - 1)
                                          path      
                                          
                              let myFunction x = 
                                 
                                  let ((compare: ResultStatus), (image: System.Drawing.Bitmap)) = 
                                     let pathWithArgument = pathWithArgument()  //legacy name
                                     bitmapCreator pathWithArgument 
                                  let image = image
                                              |> Option.ofObj                                          
                                              |> optionToBitmap "bitmapCreator"                                  
                                  
                                  let result = 
                                      let fileName = sprintf "%s-%s page %i" (string low) (string high) item
                                      
                                      //napsani do konzole vysledky kontroly barvy  
                                      match compare with
                                      | Correct               -> do saveImage image fileName 
                                                                 false //tryFind nalezne false, takze pokracuje dale
                                      | NotCorrect            -> do printfn "-----------------------------------------------------"
                                                                 do printfn "V intervalu %s%s-%s je chyba!!!" CheckingLastPageColor_Settings.Default.prefix (string low) (string high) 
                                                                 do printfn "-----------------------------------------------------"
                                                                 true   //tryFind nalezne true, takze cyklus konci  
                                      | PotentiallyNotCorrect -> do printfn "Potencionální chyba může být v intervalu LT-%s-%s, vzorek byl uložen jako %s." (string low) (string high) fileName
                                                                 do saveImage image fileName
                                                                 false      
                                  result
                              tryWith myFunction (fun x -> ()) (fun ex -> ()) |> deconstructor4 
                             
                    ) |> ignore  //List.tryFind vraci jednu nalezenou hodnotu - item (tady int) - pro kterou je podminka true, v tomhle hacku ji ale nepotrebujeme
                        
//******* MAIN FUNCTION DEFINITION - OPENING IRFANVIEW WITH LAST FILES IN THEIR RESPECTIVE FOLDERS => vystupni funkce

let private (>>=) condition nextFunc =
    match condition with
    | false -> 0
    | true  -> nextFunc() 
       
type private MyPatternBuilder = MyPatternBuilder with            
    member _.Bind(condition, nextFunc) = (>>=) <| condition <| nextFunc 
    member _.Return x = x

let compareColorInLastPage myMap createdList low high =  
   
     MyPatternBuilder    
         {             
            let! _ = (<>) createdList List.Empty            
            let! _ = String.length <| myKey low low = (+) (rcO.prefix |> String.length) rcO.numberOfScannedFileDigits                  
            let! _ =  myMap |> Map.containsKey (myKey low low) //argumenty fce su v takovem poradi: Map.containsKey key table, takze bez |> bude Map.containsKey (myKey low low) myMap          
                       
            do compareColorOfLastScannedImages 
               <| createdList 
               <| (getLists <| low <| high <| myMap)                   
               <| low
               <| high            
                                                     
            return 0
         }