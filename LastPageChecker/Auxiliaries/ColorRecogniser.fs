module ColorRecogniser

open System
open System.Drawing
open System.Drawing.Imaging

open Errors
open Records
open Settings.MySettings
open DiscriminatedUnions
open ROP_Functions.MyFunctions

let private cropImage (source: Bitmap) (section: Rectangle) : Bitmap =     
    let bitmap = new Bitmap(section.Width, section.Height)
                 |> Option.ofObj
                 |> optionToBitmap "new Bitmap(section.Width, section.Height)"
    use g = Graphics.FromImage(bitmap) 
            |> Option.ofObj 
            |> optionToGraphics "Graphics.FromImage(bitmap)" bitmap
    do g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel) 
    bitmap

let private getEncoderInfo(mimeType: String): ImageCodecInfo =
    let encoders : ImageCodecInfo[] = ImageCodecInfo.GetImageEncoders() 
                                      |> Option.ofObj
                                      |> optionToArray "ImageCodecInfo"
    encoders |> Array.tryFind (fun item -> item.MimeType = mimeType)
    |> function
        | Some value -> value
        | None       -> error17 "ImageCodecInfo"
                        encoders |> Array.item 0 //whatever of ImageCodecInfo type                              

//*********** main function 1 *********************
let bitmapCreator (path: string) = //nevydedukoval...
    (*
    Bitmap is part of System.Drawing which was included in .Net Framework.
    It is no longer included with .net core and must be added manually via NuGet.               
    *)
    
    let myFunction x = 
        let source: Bitmap = new Bitmap(path) 
                             |> Option.ofObj
                             |> optionToBitmap "new Bitmap(path)"
        let x = source.Width
        let y = source.Height

        //let section = new Rectangle(new Point(x/2 + x/8, y/4), new Size(x/4, y/2))                
        let section = new Rectangle(new Point(x/2 + x/6, y/4), new Size(350, 350))
        let croppedImage: Bitmap = cropImage source section  

        let auxRGB (x: char) low high = 
            [| 0..croppedImage.Height - 1 |] 
            |> Array.map (fun itemH ->
                                     [| 0..croppedImage.Width - 1 |] 
                                     |> Array.map (fun itemW ->  
                                                              let now_color = croppedImage.GetPixel(itemW, itemH)
                                                      
                                                              let y = 
                                                                  match x with 
                                                                  | 'r' -> now_color.R
                                                                  | 'g' -> now_color.G
                                                                  | 'b' -> now_color.B  
                                                                  |  _  -> 0uy
                                                    
                                                              let myRecord = 
                                                                  match y = 0uy with 
                                                                  | false -> 
                                                                           match (y >= low) && (y <= high) with
                                                                           | true  ->
                                                                                    {
                                                                                        itemBool = true
                                                                                        myInt = 1
                                                                                        myInt_0 = 0
                                                                                    }                                                                                
                                                                           | false -> 
                                                                                    {
                                                                                        itemBool = false
                                                                                        myInt = 0
                                                                                        myInt_0 = 0
                                                                                    }              
                                                                  | true  ->
                                                                           {
                                                                               itemBool = true
                                                                               myInt = 0
                                                                               myInt_0 = 1
                                                                           }         
                                                              myRecord                                                         
                                                  ) 
                         )

        let everythingIsCorrect = 
            let auxR = auxRGB 'r' 18uy 75uy |> Array.concat //Nelze tasks, bo je to vse k jednomu objektu
            let auxG = auxRGB 'g' 18uy 75uy |> Array.concat //Nelze tasks, bo je to vse k jednomu objektu
            let auxB = auxRGB 'b' 18uy 75uy |> Array.concat //Nelze tasks, bo je to vse k jednomu objektu
            let concatAux = Array.append auxR auxG |> Array.append auxB                  
            let result = concatAux
                        |> Array.forall (fun item -> item.itemBool = true)                
            let counter = concatAux
                            |> Array.map (fun item -> item.myInt)
                            |> Array.fold (+) 0
            let counter_0 = concatAux
                            |> Array.map (fun item -> item.myInt_0)
                            |> Array.fold (+) 0
            match result with
            | false -> 
                     let formula = 0.7 * float (3 * croppedImage.Height * croppedImage.Width - counter_0)
                     match float counter > formula with 
                     | true  -> Correct                 
                     | false -> 
                                match abs (float counter - formula) < float (counter / 5) with
                                | true  -> printfn "Informace pro následující potencionální chybu:" 
                                           printfn "formula = %A" formula
                                           printfn "counter = %A" counter
                                           PotentiallyNotCorrect
                                | false -> NotCorrect
            | true  -> Correct //tj je same true    
     
        (* 
        let result = concatAux 
                     |> List.tryFind (fun item -> 
                                                let itemBool, myInt, myInt_0 = item
                                                itemBool = false 
                                     )  //pokud polozka s takovou podminkou existuje, vraci se prislusna polozka (je jedno, co tam je, ale je to hodnota Some), jinak vraci None 
        
        let everythingIsCorrect = 
            match result with
            | Some value -> 
                            let formula = 0.7 * float (3 * croppedImage.Height * croppedImage.Width - counter_0)
                            match float counter > formula with 
                            | true  -> true                                  
                            | false -> 
                                       match abs (float counter - formula) < float (counter / 10) with
                                       | true  -> printfn "Informace pro následující potencionální chybu:" 
                                                  printfn "formula = %A" formula
                                                  printfn "counter = %A" counter
                                       | false -> ()
                                       false                       
            | None       -> true //tj je same true    
        *)

        let croppedImage = 
            match everythingIsCorrect with
            | PotentiallyNotCorrect -> let section = new Rectangle(new Point(0, 0), new Size(x, y))
                                       cropImage source section 
            | _                     -> croppedImage
        everythingIsCorrect, croppedImage    
    
    let ropResults = tryWith myFunction (fun x -> ()) (fun ex -> failwith)  
    ropResults |> deconstructor3 
 
 //*********** main function 2 *********************

let saveImage (image: Bitmap) (fileName: string) = 

    let myFunction x = 
        let myImageCodecInfo: ImageCodecInfo = getEncoderInfo("image/jpeg")
        let myEncoder: System.Drawing.Imaging.Encoder = System.Drawing.Imaging.Encoder.Quality 
        let myEncoderParameters: EncoderParameters = new EncoderParameters(1) 
        let myEncoderParameter = new EncoderParameter(myEncoder, 50L) //50% kvalita 
                                 |> Option.ofObj 
                                 |> optionToEncoder "new EncoderParameter()" myEncoder
        myEncoderParameters.Param[0] <- myEncoderParameter   
        let path = sprintf"%s%s%s" 
                   <| rc.path
                   <| fileName 
                   <| ".jpg" //@$"c:\Users\Martina\Kontroly skenu Litomerice\zadni strany - kontrola\{fileName}.jpg"
        image.Save(path, myImageCodecInfo, myEncoderParameters)      
    
    let ropResults = tryWith myFunction (fun x -> image.Dispose()) (fun ex -> failwith)
    ropResults |> deconstructor1 error17
       