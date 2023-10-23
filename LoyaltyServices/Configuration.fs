module LoyaltyServices.Configuration

open LoyaltyServices.GraphQL
open LoyaltyServices.GraphQL.AccountQueries
open LoyaltyServices.GraphQL.BookingQueries
open LoyaltyServices.GraphQL.EmployeeQueries
open LoyaltyServices.GraphQL.OrganizationQueries

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Serilog
open Services.Account.Types

module Logging =
    let ConfigureLogger =
        let appName = "Chores App Services"

        let logger =
            LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProperty("ServiceType", appName)
                .Enrich.WithProperty("service", appName)
                .WriteTo.Console()
                .MinimumLevel.Information()
                .CreateLogger()

        logger

type Startup() =
    member this.ConfigureServices(services: IServiceCollection) =
        services
            .AddGraphQLServer()
            .ModifyRequestOptions(fun opt -> opt.IncludeExceptionDetails <- true)

            .AddQueryType()
            .AddType<OrganizationQueries>()
            .AddType<OrganizationExtensions>()
            
            .AddType<AccountQueries>()
            .AddType<AccountMutations>()
            
            .AddType<BookingQueries>()
            .AddType<BookingMutations>()

            .AddMutationType()
            
        |> ignore

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app
            .UseRouting()
            .UseEndpoints(fun endpoints -> endpoints.MapGraphQL().AllowAnonymous() |> ignore)
        |> ignore

module Startup =
    let createHostBuilder args =
        Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(fun webBuilder -> webBuilder.UseStartup<Startup>() |> ignore)
            .UseSerilog(Logging.ConfigureLogger)
