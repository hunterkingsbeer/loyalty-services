namespace LoyaltyServices.Services

open Amazon.DynamoDBv2
open FSharp.AWS.DynamoDB.Scripting

[<RequireQualifiedAccess>]
module DynamoCore =
    [<Literal>]
    let private TableName = "loyalty-app"

    let private client: IAmazonDynamoDB =
        new AmazonDynamoDBClient(AmazonDynamoDBConfig())

    let initializeTableContext<'a> () =
        TableContext.Initialize<'a>(client, TableName)
        
    // use for matching on prefix of sort key
    // USAGE:
    // match "example string" with
    // | Prefix "prefix#here" remaining -> return remaining
    let (|Prefix|_|) (p:string) (s:string) =
        if s.StartsWith(p) then
            Some(s.Substring(p.Length))
        else
            None
