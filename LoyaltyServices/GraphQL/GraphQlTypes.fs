namespace LoyaltyServices.GraphQL

open System
open HotChocolate
open Services.Organization.Types

[<RequireQualifiedAccess>]
module GraphQlTypes =
    [<GraphQLName(nameof (Organization))>]
    type Organization =
        { Username: string
          Type: OrganizationType
          Name: string
          Address: string
          Description: string
          Color: string
          Media: OrganizationMedia }

    and OrganizationMedia =
        { Logo: string
          Cover: string
          Images: string list }

    [<GraphQLName(nameof (Employee))>]
    type Employee =
        { Username: string
          OrganizationUsername: string
          FirstName: string
          LastName: string
          Description: string
          Position: string
          DateStarted: DateTime
          Media: EmployeeMedia }

    and EmployeeMedia =
        { ProfileImage: string
          Images: string list }

    [<GraphQLName(nameof (Account))>]
    type Account =
        { Username: string
          FirstName: string
          LastName: string
          Email: string
          Birthday: DateTime
          Loyalties: string list }

        static member Default =
            { Username = ""
              FirstName = ""
              LastName = ""
              Email = ""
              Birthday = DateTime.MinValue
              Loyalties = [] }

    type CreateAccountInput =
        { Username: string
          FirstName: string
          LastName: string
          Email: string }
