module GettingInputValuesEP

open System
open System.IO
open System.Data

open Errors
open Helpers
open Settings
open Settings.MySettings
open ROP_Functions.MyFunctions

//******* DEFINITIONS OF FREQUENTLY CALLED FUNCTIONS ********************       
//function 1
let private stringChoice x = MyString.getString((rcO.numberOfScannedFileDigits - String.length (x |> string)), rcO.stringZero)

//******* MAIN FUNCTIONS ****************************

let getExcelValuesEP readFromExcel =        
                                        
    let myTaskFunction x = readFromExcel  //legacy name, task not used in this app        
                   
    let rcInitVal = 
        let result = 
            let ropResults() = tryWith myTaskFunction (fun x -> ()) (fun ex -> ()) 
            ropResults() |> deconstructor 
        result
           
    rcInitVal        
   
//createList 
//impure, bo Directory.EnumerateFiles a printfn
let createListInDirWithIncorrNoOfFilesEP low high = // = adresar, ktery kontroluji, tj. adresar s potencialnimi chybami, v teto app to jsou vsechny
    
    let folderItem = sprintf "%s%s%s-%s%s" //u sprintf je typova kontrola  
                     <| rcO.prefix
                     <| stringChoice low 
                     <| string low 
                     <| stringChoice high 
                     <| string high   
            
    let dirWithIncorrNoOfFiles = sprintf "%s%s" 
                                 <| string rcO.path
                                 <| folderItem     
                 
    try   //vyzkouseni si .NET exceptions
        try                            
            //2x staticka trida System.IO.Directory...., nebot nelze objekt dirInfo vyuzit 2x            
            let mySeq = Directory.EnumerateFiles(dirWithIncorrNoOfFiles, "*.jpg")
                        |> Option.ofObj   
                        |> optionToEnumerable "Directory.EnumerateFiles()"     
                        
            match Directory.Exists(dirWithIncorrNoOfFiles) with   
            | true  -> List.ofSeq(mySeq)                                                 
            | false -> dirWithIncorrNoOfFiles |> error5   
                       List.Empty 

        finally
        () //zatim nepotrebne
    with  
    | :? System.IO.DirectoryNotFoundException as ex -> ex.Message |> error3
                                                       List.Empty                                                                                        
    | :? System.IO.IOException as                ex -> ex.Message |> error4 
                                                       List.Empty
    | _ as                                       ex -> ex.Message |> error1 //System.Exception
                                                       List.Empty           
                                
    
     
   
    (*       
       System.IO.File provides static members related to working with files, whereas System.IO.FileInfo represents a specific file and contains non-static members for working with that file.          
       Because all File methods are static, it might be more efficient to use a File method rather than a corresponding FileInfo instance method if you want to perform only one action. All File methods 
       require the path to the file that you are manipulating.    
       The static methods of the File class perform security checks on all methods. If you are going to reuse an object several times, consider using the corresponding 
       instance method of FileInfo instead, because the security check will not always be necessary.  
    *)
