namespace Services.Booking

open Services

module BookingService =
    let getBooking accountUsername = Dynamo.getBooking accountUsername
    let getUsersBookings accountUsername = Dynamo.getUsersBookings accountUsername
    
    let createBooking accountUsername organizationUsername bookingDate title description =
        Dynamo.createBooking accountUsername organizationUsername bookingDate title description
