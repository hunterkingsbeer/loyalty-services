namespace Services.Booking

open System
open Services.Account.Types
open Services.Organization.Types

module Types =
    type Booking =
        { AccountUsername: AccountUsername
          OrganizationUsername: OrganizationUsername
          Title: string
          Description: string
          DateCreated: DateTime
          DateBooked: DateTime }
        