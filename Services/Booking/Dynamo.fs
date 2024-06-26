namespace Services.Booking

open System
open FSharp.AWS.DynamoDB
open FSharp.AWS.DynamoDB.Scripting
open LoyaltyServices.Services
open Services.Account.Types
open Services.Booking.Types
open Services.Organization.Types

[<RequireQualifiedAccess>]
module internal Dynamo =
    [<Literal>]
    let private BookingSortKey = "booking"

    [<Literal>]
    let private DateBookedSortKey = "dateBooked"

    let private toBookingSort (orgUser: String) (dateBooked: DateTime) =
        let date = dateBooked.ToString("u")
        $"{BookingSortKey}#{orgUser}/{DateBookedSortKey}#{date}"

    type private BookingItem =
        { [<HashKey>]
          [<GlobalSecondaryRangeKey(indexName = "sort-key-index")>]
          key: string // Account Username
          [<RangeKey>]
          [<GlobalSecondaryHashKey(indexName = "sort-key-index")>]
          sort: string // booking#org_username/dateBooked#2023-11-20T12:15:00+0000

          title: string
          description: string
          dateCreated: string }

    let private bookingTable = DynamoCore.initializeTableContext<BookingItem> ()

    let private itemToBooking (bookingItem: BookingItem) : Booking =
        let bookedOrg =
            DynamoCore.getSortKeyValue bookingItem.sort BookingSortKey
            |> OrganizationUsername

        let bookedDate =
            DynamoCore.getSortKeyValue bookingItem.sort DateBookedSortKey |> DateTime.Parse

        { AccountUsername = bookingItem.key |> AccountUsername
          OrganizationUsername = bookedOrg
          Title = bookingItem.title
          Description = bookingItem.description
          DateCreated = DateTime.Parse bookingItem.dateCreated
          DateBooked = bookedDate }

    let getBooking (AccountUsername username) (OrganizationUsername orgUser) (dateBooked: DateTime) : Booking option =
        let sort = toBookingSort orgUser dateBooked
        let key = TableKey.Combined(username, sort)

        try
            Some(bookingTable.GetItem key |> itemToBooking)
        with _ ->
            None

    let createBooking
        (AccountUsername accountUsername)
        (OrganizationUsername organizationUsername)
        (dateBooked: DateTime)
        title
        description
        =
        let sort = toBookingSort (organizationUsername.ToLower()) dateBooked
        
        let bookingItem: BookingItem =
            { key = accountUsername.ToLower() // Username
              sort = sort  // Type of `booking`

              title = title
              description = description
              dateCreated = DateTime.UtcNow.ToString("u") }

        try
            let _ = bookingTable.PutItem(bookingItem)
            Console.WriteLine $"BOOKING SERVICE: Booking creation successful. Info: {sort}"

            Some true
        with e ->
            Console.WriteLine
                $"BOOKING SERVICE: Booking creation failed. Reason: [{e.Message}]. Stack trace: [{e.StackTrace}]"

            None

    let getUsersBookings (AccountUsername accountUsername) : Booking list =
        bookingTable.Scan(<@ fun (r: BookingItem) -> r.sort.StartsWith BookingSortKey && r.key = accountUsername @>)

        |> List.ofArray
        |> List.map itemToBooking
        
    let deleteBooking (AccountUsername username) (OrganizationUsername orgUser) (bookingDate: DateTime) =
        let sort = toBookingSort orgUser bookingDate
        let key = TableKey.Combined(username, sort)
        
        let result = bookingTable.DeleteItem key
        match result with
        | Some _ -> true
        | None -> false 
        
