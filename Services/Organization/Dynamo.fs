namespace Services.Organization

open FSharp.AWS.DynamoDB
open FSharp.AWS.DynamoDB.Scripting

open LoyaltyServices.Services
open Services.Organization.Types

[<RequireQualifiedAccess>]
module internal Dynamo =
    [<Literal>]
    let private OrganizationSortKey = "organization"

    type private OrganizationItem =
        { [<HashKey>]
          [<GlobalSecondaryRangeKey(indexName = "sort-key-index")>]
          key: string // Username
          [<RangeKey>]
          [<GlobalSecondaryHashKey(indexName = "sort-key-index")>]
          sort: string // Type `organization`

          name: string
          orgType: string
          description: string
          address: string
          dateCreated: string
          color: string }

    let private organizationTable =
        DynamoCore.initializeTableContext<OrganizationItem> ()

    let private itemToOrganization (orgItem: OrganizationItem) =
        { Username = orgItem.key |> OrganizationUsername
          Type = orgItem.orgType |> OrganizationType.parse
          Name = orgItem.name
          Address = orgItem.address
          Description = orgItem.description
          Color = orgItem.color }

    let getOrganization (OrganizationUsername username) : Organization =
        let key = TableKey.Combined(username, OrganizationSortKey)
        organizationTable.GetItem(key) |> itemToOrganization

    let getOrganizations () : Organization list =
        organizationTable.Query(keyCondition = <@ fun (r: OrganizationItem) -> r.sort = OrganizationSortKey @>)
        |> List.ofArray
        |> List.map itemToOrganization

    let getOrganizationsOfType (orgType: OrganizationType) : Organization list =
        let capitalize (sentence: string) =
            match sentence with
            | "" -> ""
            | s when s.[0] >= 'a' && s.[0] <= 'z' -> sentence.Remove(0, 1).Insert(0, string (char (int s.[0] - 32)))
            | _ -> sentence

        let orgTypeStr = capitalize (orgType.ToString())

        organizationTable.Query(
            keyCondition = <@ fun (r: OrganizationItem) -> r.sort = OrganizationSortKey @>,
            filterCondition = <@ fun (r: OrganizationItem) -> r.orgType = orgTypeStr @>
        )
        |> List.ofArray
        |> List.map itemToOrganization
