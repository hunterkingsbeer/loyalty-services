namespace Services.Account

open Services

module AccountService =
    let getAccount accountUsername = Dynamo.getAccount accountUsername
    let createAccount account = Dynamo.createAccount account
    let getAllAccounts () = Dynamo.getAllAccounts ()

    let updateLoyalties username organizations =
        Dynamo.updateLoyalties username organizations
