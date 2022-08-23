namespace FSharpIrfanViewOpener

open System
open System.IO

open Errors
open Helpers
open Settings
open Helpers.Process
open GettingInputValuesEP
open Settings.MySettings
open ReadingDataFromExcelEP
open CheckingLastNameColor
open ROP_Functions.MyFunctions

module Start =   
                                           
    [<EntryPoint>]
    let main argv =
    
        //******* 1) FIXING PROBLEM WITH ENCODING
        do System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)
        
        Console.BackgroundColor <- ConsoleColor.Blue 
        Console.ForegroundColor <- ConsoleColor.White 
        Console.InputEncoding   <- System.Text.Encoding.Unicode
        Console.OutputEncoding  <- System.Text.Encoding.Unicode
      
        //******* 2) CLOSING ANY EXCEL FILE AND DELETING/CREATING TEMPORARY DIRECTORY       
        let excelKiller x = 
            do ExcelKiller.ExcelKiller.SaveAndCloseExcel()  
                           
        let result = //tady je fn aji pro finally
            let ropResults = tryWith excelKiller (fun x -> do KillSingleProcess(String.Empty, "ERROR006", true)) (fun ex -> failwith)                                                     
            ropResults |> deconstructor1 error12  
        result

        let myFunction x =         
            do Directory.Delete(rc.path, true) 
            do Directory.CreateDirectory(rc.path) |> ignore 
            let sourceDir = @"c:\Users\User\source\repos\LastPageChecker\LastPageChecker\";
            let backupDir = @"e:\E\Mirek po osme hodine a o vikendech\Kontroly skenu\zadni strany - kontrola\";
            let fileName = "Graphicloads-Food-Drink-Teapot-1.jpg"       
            do File.Copy(Path.Combine(sourceDir, fileName), Path.Combine(backupDir, fileName), true) 

        let result = 
           let ropResults = tryWith myFunction (fun x -> ()) (fun ex -> failwith)                                                     
           ropResults |> deconstructor1 error17 
        result        
        
        //****** 3) PIPING INPUT VALUES FROM EXCEL INTO RELEVANT FUNCTIONS   
        
        let str = "HH:mm:ss"

        let processStart() =     
            let processStartTime = $"Začátek procesu: {DateTime.Now.ToString(str)}"
            printfn "%s" processStartTime 

        let processEnd() =     
            let processEndTime = $"Konec procesu: {DateTime.Now.ToString(str)}"
            printfn "%s" processEndTime
        
        processStart()
        printfn("Starting processing ...")
          
        let directoryPaths x = Directory.GetDirectories(rcO.path, rcO.prefix + "*", SearchOption.TopDirectoryOnly) 
                               |> Option.ofObj
                               |> optionToArray "Directory.GetDirectories()"  
        
        let directoryPaths = 
            let ropResults = tryWith directoryPaths (fun x -> ()) (fun ex -> failwith)                                                     
            ropResults |> deconstructor2  
        
        //let readDataFromExcel = readDataFromExcel() //aby se to nevyhodnocovalo v cyklu
        let readDataFromExcelEP = readDataFromExcelEP() //aby se to nevyhodnocovalo v cyklu

        let lastPageChecker x = 
            directoryPaths 
            |> Array.Parallel.iter (fun item -> 
                                              let lastDir = (new DirectoryInfo(item)).Name  //c:\Users\Martina\Kontroly skenu Litomerice\rozhazovani\LT-21731-21744\
                                                            |> Option.ofObj 
                                                            |> optionToString1 "new DirectoryInfo()"
                                              match lastDir.Length = 14 with //14 je delka typoveho retezce LT-21731-21744
                                              | true  -> 
                                                       //let lastDir = (new DirectoryInfo(item)).Name 
                                                       //let low = Parsing.parseMe(lastDir.Substring(3, 5))   
                                                       let low = Parsing.parseMe(lastDir.[3..7])
                                                            
                                                       //let lastDir = (new DirectoryInfo(item)).Name //je treba znovu quli substring
                                                       //let high = Parsing.parseMe(lastDir.Substring(9, 5))
                                                       let high = Parsing.parseMe(lastDir.[9..13])
                                                           
                                                       //princip roboty programu
                                                       (getExcelValuesEP <| readDataFromExcelEP) |> compareColorInLastPage <| (createListInDirWithIncorrNoOfFilesEP low high) <| low <| high |> ignore 
                                              | false -> 
                                                       do printfn "Adresář [%s] nebyl kontrolován." lastDir
                                    )
        
        let lastPageChecker() = 
           let ropResults = tryWith lastPageChecker (fun x -> ()) (fun ex -> failwith)                                                     
           ropResults |> deconstructor1 error17 
        lastPageChecker()

        processEnd() 
        do printfn "%s" "The work has been done ... Press any key to end this app."                                 
        do Console.ReadKey() |> ignore
                                                  
        0//konec programu
      
        
