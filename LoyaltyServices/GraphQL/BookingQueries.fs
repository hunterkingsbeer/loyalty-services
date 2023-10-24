namespace LoyaltyServices.GraphQL

open System
open System.Globalization
open HotChocolate
open HotChocolate.Types

open LoyaltyServices.GraphQL
open Services.Account.Types
open Services.Booking
open Services.Booking.Types
open Services.Organization.Types

module BookingQueries =
    let toBooking (b: Booking) : GraphQlTypes.Booking =
        let (AccountUsername accUsername) = b.AccountUsername
        let (OrganizationUsername orgUsername) = b.OrganizationUsername

        { AccountUsername = accUsername
          OrganizationUsername = orgUsername
          Title = b.Title
          Description = b.Description
          DateCreated = b.DateCreated
          DateBooked = b.DateBooked }


    [<ExtendObjectType("Query")>]
    type BookingQueries() =
        member this.getBooking accountUsername organizationUsername bookingDate =
            let result =
                BookingService.getBooking
                    (AccountUsername accountUsername)
                    (OrganizationUsername organizationUsername)
                    (DateTime.Parse bookingDate)

            match result with
            | Some booking -> booking |> toBooking
            | None -> raise (GraphQLException "BOOKING_NOT_FOUND")

        member this.getUsersBookings accountUsername =
            BookingService.getUsersBookings (AccountUsername accountUsername)
            |> List.map toBooking

    [<ExtendObjectType("Mutation")>]
    type BookingMutations() =
        member this.createBooking accountUsername organizationUsername bookingDate title description =
            let result =
                BookingService.createBooking
                    (AccountUsername accountUsername)
                    (OrganizationUsername organizationUsername)
                    (DateTime.Parse bookingDate)
                    title
                    description

            match result with
            | Some _ -> true
            | None -> false

        member this.deleteBooking accountUsername organizationUsername (bookingDate: string) =
            let date = DateTime.Parse(bookingDate, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal)
            BookingService.deleteBooking
                (AccountUsername accountUsername)
                (OrganizationUsername organizationUsername)
                date
