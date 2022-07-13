﻿module ClosingFunctions

open System
open Helpers.Process
          
let private finalNotice() =           
    do printfn "\nZadané hodnoty neumožnily cokoliv dělat, sorry..."
    do printfn "\nStiskni ENTER pro pokračování nebo ukončení programu..."
    do pressEnterToContinue()  
    do Console.Clear()
    0//konec programu
    
let closeApp x = //nahrazena rekurzivni funkci primo v modulu Start
    do printfn "Stiskni ENTER pro ukončení programu..."
    do pressEnterToContinue()
    do KillSingleProcess(x , "ERROR009", true)//rc.imageViewer
    0//konec programu  