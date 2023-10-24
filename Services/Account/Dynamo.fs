namespace Services.Account

open System
open FSharp.AWS.DynamoDB
open FSharp.AWS.DynamoDB.Scripting
open LoyaltyServices.Services
open Microsoft.FSharp.Collections
open Services.Account.Types
open Services.Organization.Types

[<RequireQualifiedAccess>]
module internal Dynamo =
    [<Literal>]
    let private AccountSortKey = "account"

    type private AccountItem =
        { [<HashKey>]
          [<GlobalSecondaryRangeKey(indexName = "sort-key-index")>]
          key: string // Username
          [<RangeKey>]
          [<GlobalSecondaryHashKey(indexName = "sort-key-index")>]
          sort: string // Type of `account`

          firstName: string
          lastName: string
          email: string
          birthday: string
          loyalties: string Set }

    let private accountTable = DynamoCore.initializeTableContext<AccountItem> ()

    let private itemToAccount (accItem: AccountItem) : Account =
        { Username = accItem.key |> AccountUsername
          FirstName = accItem.firstName
          LastName = accItem.lastName
          Email = accItem.email
          Birthday = DateTime.Parse accItem.birthday
          Loyalties = accItem.loyalties |> List.ofSeq |> List.map OrganizationUsername }

    let getAccount (AccountUsername username) : Account option =
        let key = TableKey.Combined(username, AccountSortKey)
        
        try
            Some (accountTable.GetItem key |> itemToAccount)
        with _ ->
            None
            
    let createAccount (account: Account) : String option =
        let (AccountUsername username) = account.Username
        
        match getAccount(account.Username) with
        | Some _ ->
            Console.WriteLine $"ACCOUNT SERVICE: Account creation failed. Reason: username already exists. Username: {username}"
            None
        | None -> 
            let accountItem: AccountItem =
                { key = username.ToLower() // Username
                  sort = AccountSortKey // Type of `account`

                  firstName = account.FirstName.ToLower()
                  lastName = account.LastName.ToLower()
                  email = account.Email.ToLower()
                  birthday = account.Birthday.ToString("u")
                  loyalties = Set.empty }

            try
                let _ = accountTable.PutItem(accountItem)
                Console.WriteLine $"ACCOUNT SERVICE: Account creation successful. Username: {username}"
                
                Some username
            with e ->
                Console.WriteLine $"ACCOUNT SERVICE: Account creation failed. Reason: [{e.Message}]. Stack trace: [{e.StackTrace}]"
                None 

    let getAllAccounts () : Account list =
        accountTable.Query(keyCondition = <@ fun (r: AccountItem) -> r.sort = AccountSortKey @>)
        |> List.ofArray
        |> List.map itemToAccount

    let updateLoyalties (AccountUsername username) (organizations: OrganizationUsername list) =
        let key = TableKey.Combined(username, AccountSortKey)

        let orgSet =
            organizations
            |> List.map (fun (OrganizationUsername username) -> username)
            |> Set.ofList

        accountTable.UpdateItem(key, <@ fun (acc: AccountItem) -> { acc with loyalties = orgSet } @>)
        |> itemToAccount
