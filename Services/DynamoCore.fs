namespace LoyaltyServices.Services

open System
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
    let (|Prefix|_|) (p: string) (s: string) =
        if s.StartsWith(p) then
            Some(s.Substring(p.Length))
        else
            None

    let getSortKeyValue (sortKey: string) (specifier: string) =
        let substringValue =
            sortKey.Substring(sortKey.IndexOf(specifier) + specifier.Length + 1)

        // find closest separator 
        let hashIndex = substringValue.IndexOf("#")
        let slashIndex = substringValue.IndexOf("/")

        if hashIndex < slashIndex then
            substringValue[0..hashIndex - 1]
        else if slashIndex < hashIndex then
            substringValue[0..slashIndex - 1]
        else
            substringValue
