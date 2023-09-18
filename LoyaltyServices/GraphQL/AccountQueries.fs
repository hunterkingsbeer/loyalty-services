namespace LoyaltyServices.GraphQL

open System
open HotChocolate
open HotChocolate.Types

open LoyaltyServices.GraphQL
open Services.Account
open Services.Account.Types
open Services.Organization.Types

module AccountQueries =
    let toAccount (e: Account) : GraphQlTypes.Account =
        let (AccountUsername username) = e.Username

        { Username = username
          FirstName = e.FirstName
          LastName = e.LastName
          Email = e.Email
          Birthday = e.Birthday
          Loyalties = e.Loyalties |> List.map (fun (OrganizationUsername org) -> org) }

    [<ExtendObjectType("Query")>]
    type AccountQueries() =
        member this.getAccount accountUsername =
            match AccountService.getAccount (AccountUsername accountUsername) with
            | Some account -> account |> toAccount
            | None -> raise (GraphQLException "ACCOUNT_NOT_FOUND")

    [<ExtendObjectType("Mutation")>]
    type AccountMutations() =
        member this.updateLoyalties accountUsername loyalties =
            let loyaltiesList = loyalties |> List.ofSeq |> List.map OrganizationUsername

            AccountService.updateLoyalties (AccountUsername accountUsername) loyaltiesList

            |> ignore

            true
            
        member this.createAccount (input: GraphQlTypes.CreateAccountInput) =
            let account: Account =
                { Username = AccountUsername input.Username
                  Email = input.Email
                  FirstName = input.FirstName
                  LastName = input.LastName
                  Birthday = DateTime.Now
                  Loyalties = [] }
                
            
            match AccountService.createAccount account with
            | Some username -> username
            | None -> raise (GraphQLException "ACCOUNT_CREATION_FAILED")
