namespace FSharpIrfanViewOpener

open FSharp.Interop.Excel

type DataTypesTest = ExcelFile<"e:\E\Mirek po osme hodine a o vikendech\Kontroly skenu\mustr1.xls", "List1">

module ReadingDataFromExcelEP =
       
    let readDataFromExcelEP() =          
        
        let myMap: Map<string, int> = 

            let file = new DataTypesTest()
            let rows = file.Data |> Seq.toArray
            let epRowsCount = rows.Length     
            let listRange = [ 0 .. epRowsCount - 1 ]
            let rec loop list acc i =  
                match list with 
                | []        -> acc
                | _ :: tail -> let result =
                                   let finalMap = Map.add (string rows.[i].A) (int rows.[i].P) acc
                                   loop <| tail <| finalMap <| i + 1                                                                         
                               result   
            loop listRange Map.empty 0           
        myMap 
        






