namespace LoyaltyServices

#nowarn "20"

open Configuration

open Serilog
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Hosting

[<RequireQualifiedAccess>]
module ExitCode =
    let Success = 0
    let Failure = 1

module Program =
    let safeMain (host: IHost) =
        try
            host.Run()
        with e ->
            Log.Logger.Fatal e.Message
            
    [<EntryPoint>]
    let main args =
        let appName = "Chores API"
        let host = Startup.createHostBuilder(args).Build()

        Log.Logger.Information $"Starting up: {appName}"
        safeMain host

        ExitCode.Success
