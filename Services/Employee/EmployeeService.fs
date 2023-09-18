namespace Services.Employee

open Services.Employee
open Services.Employee.Types
open Services.Organization.Types

[<RequireQualifiedAccess>]
module EmployeeService =
    let getEmployee (employeeUsername: EmployeeUsername) =
        Dynamo.getEmployee employeeUsername

    let getEmployeesOfOrganization (organizationUsername: OrganizationUsername) =
        Dynamo.getEmployeesOfOrganization organizationUsername

    let getProfileImage (employeeUsername: EmployeeUsername) (orgUsername: OrganizationUsername) =
        S3.getProfileImage orgUsername employeeUsername
        
    let getImages (employeeUsername: EmployeeUsername) (orgUsername: OrganizationUsername) =
        S3.getImages orgUsername employeeUsername []
        // TODO CONNECT DYNAMO EMPLOYEE IMAGES TO FUNCTION
        