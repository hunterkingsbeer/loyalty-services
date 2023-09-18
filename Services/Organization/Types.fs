namespace Services.Organization

module Types =
    type OrganizationType =
        | Unknown = 0
        | Barber = 1
        | Salon = 2
        | Health = 3
        | Food = 4

    [<RequireQualifiedAccess>]
    module OrganizationType =
        let parse (str: string) =
            match str.ToLower() with
            | "barber" -> OrganizationType.Barber
            | "salon" -> OrganizationType.Salon
            | "health" -> OrganizationType.Health
            | "food" -> OrganizationType.Food
            | _ -> OrganizationType.Unknown

    type OrganizationUsername = OrganizationUsername of string

    type Organization =
        { Username: OrganizationUsername
          Type: OrganizationType
          Name: string
          Address: string
          Description: string
          Color: string }

    and Media =
        { Logo: string
          Cover: string
          Images: string list }
