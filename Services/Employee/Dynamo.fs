namespace Services.Employee

open System
open Amazon.DynamoDBv2.DocumentModel
open FSharp.AWS.DynamoDB
open FSharp.AWS.DynamoDB.Scripting

open Services.Employee.Types
open LoyaltyServices.Services
open Services.Organization.Types

[<RequireQualifiedAccess>]
module internal Dynamo =
    [<Literal>]
    let private EmployeeSortKey = "employee"

    let employeeSortKey (EmployeeUsername username) = $"{EmployeeSortKey}#{username}"

    type private EmployeeItem =
        { [<HashKey>]
          [<GlobalSecondaryRangeKey(indexName = "sort-key-index")>]
          key: string // Org Username
          [<RangeKey>]
          [<GlobalSecondaryHashKey(indexName = "sort-key-index")>]
          sort: string // Type of `employee#employee_user_name`

          first: string
          last: string
          description: string
          position: string
          dateStarted: string }

    let private employeeTable = DynamoCore.initializeTableContext<EmployeeItem> ()

    let private itemToEmployee (eItem: EmployeeItem) : Employee =
        { Username = EmployeeUsername(eItem.sort.Substring(EmployeeSortKey.Length + 1))
          OrganizationUsername = OrganizationUsername(eItem.key)
          FirstName = eItem.first
          LastName = eItem.last
          Description = eItem.description
          Position = eItem.position
          DateStarted = DateTime.Parse eItem.dateStarted }

    let getEmployeesOfOrganization (OrganizationUsername username) : Employee list =
        let employees =
            employeeTable.Scan <@ fun (r: EmployeeItem) -> r.key = username && r.sort.StartsWith "employee" @>

        employees |> List.ofArray |> List.map itemToEmployee

    let getEmployee (username: EmployeeUsername) : Employee =
        let sortUsername = employeeSortKey username

        let employee =
            employeeTable.Query(keyCondition = <@ fun (r: EmployeeItem) -> r.sort = sortUsername @>)
            |> Array.head

        employee |> itemToEmployee
