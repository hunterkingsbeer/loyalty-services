namespace Services.Employee

open System
open Services.Organization.Types

module Types =
    type EmployeeUsername = EmployeeUsername of string

    type Employee =
        { Username: EmployeeUsername
          OrganizationUsername: OrganizationUsername
          FirstName: string
          LastName: string
          Description: string
          Position: string
          DateStarted: DateTime }
