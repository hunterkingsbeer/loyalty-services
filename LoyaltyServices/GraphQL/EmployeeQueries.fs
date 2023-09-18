namespace LoyaltyServices.GraphQL

open HotChocolate
open HotChocolate.Types

open Services.Employee
open Services.Employee.Types
open Services.Organization.Types

module EmployeeQueries =
    let toEmployee (e: Employee) : GraphQlTypes.Employee =
        let EmployeeUsername username as eUsername = e.Username
        let (OrganizationUsername orgUsername) = e.OrganizationUsername

        { Username = username
          OrganizationUsername = orgUsername
          FirstName = e.FirstName
          LastName = e.LastName
          Description = e.Description
          Position = e.Position
          DateStarted = e.DateStarted 
          Media =
            { ProfileImage = EmployeeService.getProfileImage eUsername e.OrganizationUsername
              Images = [] } }
        
    [<ExtendObjectType("Organization")>]
    type OrganizationExtensions() =
        member this.employees([<Parent>] org: GraphQlTypes.Organization) =
            EmployeeService.getEmployeesOfOrganization (OrganizationUsername(org.Username))
            |> List.map toEmployee
