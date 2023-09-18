namespace Services.Organization

open Services.Organization.Types

[<RequireQualifiedAccess>]
module OrganizationService =
    let getOrganization (orgUsername: OrganizationUsername) = Dynamo.getOrganization orgUsername
    let getOrganizations () = Dynamo.getOrganizations ()
    let getOrganizationsOfType orgType = Dynamo.getOrganizationsOfType orgType

    let getMedia (orgUsername: OrganizationUsername) =
        { Logo = S3.getLogo orgUsername
          Cover = S3.getCover orgUsername
          Images = S3.getImages orgUsername }
