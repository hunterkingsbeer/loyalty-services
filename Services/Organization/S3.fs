namespace Services.Organization

open Amazon.S3.Model
open Services
open Services.Organization.Types

module S3 =
    let private orgBucket (OrganizationUsername username) = $"{S3Core.loyaltyBucketUrl}/{username}"
    let getLogo username = $"{orgBucket username}/logo.jpg"
    let getCover username = $"{orgBucket username}/cover.jpg"

    let getImages (OrganizationUsername username) =
        let request = ListObjectsRequest()
        request.BucketName <- S3Core.loyaltyBucketName
        request.Prefix <- $"{username}/media/"

        let response =
            S3Core.client.ListObjectsAsync request
            |> Async.AwaitTask
            |> Async.RunSynchronously

        if response.S3Objects.Count <= 0 then
            []
        else 
            response.S3Objects
            |> List.ofSeq
            |> List.tail
            |> List.map (fun image -> $"{S3Core.loyaltyBucketUrl}/{image.Key}")
