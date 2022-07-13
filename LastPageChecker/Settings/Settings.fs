namespace Settings

open System
    
[<Struct>]  //vhodne pro 16 bytes => 4096 characters
type Common_Settings = 
    {        
       columnIndex: int[] //both for G and R
       path: string  
    }
    static member Default = 
        {
           columnIndex = [| 0..15 |] //Sloupce A,B,C,...P =  0,1,2, ... 15
           path = @"c:\Users\Martina\Kontroly skenu Litomerice\zadni strany - kontrola\"
        }

[<Struct>]
type ReadingDataFromExcel_Settings = 
    {
       path1: string  
       indexOfXlsxSheet: int  
    }
    static member Default = 
        {
           path1 = $@"c:\Users\Martina\Kontroly skenu Litomerice\mustr.xlsx"                   
           indexOfXlsxSheet = 0<1> //Sheet v poradi 0 jako prvni, 1 = druhy, atd.
        } 

 [<Struct>]
type CheckingLastPageColor_Settings = 
    {        
       path: string     
       numberOfScannedFileDigits: int
       prefix: string    
       stringZero: string  
       suffixAndExtLength: int  
    }
    static member Default = 
        {
           path =  $@"c:\Users\Martina\Kontroly skenu Litomerice\rozhazovani\"
           numberOfScannedFileDigits = 5<1>
           prefix = "LT-"
           stringZero = "0"
           suffixAndExtLength = 10<1> //delka retezce _00001.jpg
        }  

    module MySettings = 
    
     //ARCHIV LITOMERICE    
     let rc =  Common_Settings.Default 
     let rcR = ReadingDataFromExcel_Settings.Default
     let rcO = CheckingLastPageColor_Settings.Default               
     

