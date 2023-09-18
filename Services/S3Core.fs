namespace Services

open Amazon.S3

[<RequireQualifiedAccess>]
module S3Core =
    let client: IAmazonS3 = new AmazonS3Client(AmazonS3Config())

    // TODO: DONT HARDCODE VALUES
    let loyaltyBucketName = "loyalty-app-organizations"

    let loyaltyBucketUrl =
        $"https://{loyaltyBucketName}.s3.ap-southeast-2.amazonaws.com"
