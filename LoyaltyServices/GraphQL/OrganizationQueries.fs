namespace LoyaltyServices.GraphQL

open HotChocolate.Types

open Services.Organization
open Services.Organization.Types

module OrganizationQueries =
    let toOrganization (org: Organization) : GraphQlTypes.Organization =
        let OrganizationUsername username as orgUser = org.Username

        let orgMedia: GraphQlTypes.OrganizationMedia =
            let media = OrganizationService.getMedia orgUser

            { Logo = media.Logo
              Cover = media.Cover
              Images = media.Images }

        { Username = username
          Type = org.Type
          Name = org.Name
          Address = org.Address
          Description = org.Description
          Color = org.Color
          Media = orgMedia }

    [<ExtendObjectType(OperationTypeNames.Query)>]
    type OrganizationQueries() =
        member this.getOrganization organizationUsername =
            OrganizationService.getOrganization (OrganizationUsername organizationUsername)
            |> toOrganization

        member this.getAllOrganizations() =
            OrganizationService.getOrganizations () |> List.map toOrganization

        member this.getOrganizationsOfType organizationType =
            OrganizationService.getOrganizationsOfType organizationType
            |> List.map toOrganization
