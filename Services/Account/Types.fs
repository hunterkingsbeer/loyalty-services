namespace Services.Account

open System
open Services.Organization.Types

module Types =
    type AccountUsername = AccountUsername of string
    type Account =
        { Username: AccountUsername
          FirstName: string
          LastName: string
          Email: string
          Birthday: DateTime
          Loyalties: OrganizationUsername list }
        