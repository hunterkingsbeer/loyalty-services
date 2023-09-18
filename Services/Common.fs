namespace Services

[<RequireQualifiedAccess>]
module Common =
    module Patterns = 
        // match obj with
        // | NotNull -> do something with obj
        // | _ -> do something with null obj  
        let (|NotNull|_|) value = 
          if obj.ReferenceEquals(value, null) then None 
          else Some()
