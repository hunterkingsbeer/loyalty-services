namespace Services.Employee

open Services
open Services.Employee.Types
open Services.Organization.Types

module S3 =
    let private employeeBucket (OrganizationUsername org) (EmployeeUsername employee) = $"{S3Core.loyaltyBucketUrl}/{org}/{employee}"
    
    let getProfileImage org employee = $"{employeeBucket org employee}/profile.jpg"

    let getImages org employee (images: string list) =
        images |> List.map(fun image -> $"{employeeBucket org employee}/{image}.jpg")
