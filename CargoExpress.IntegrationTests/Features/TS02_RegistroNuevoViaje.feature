@wip
Feature: TS02 - Registration of a new trip
    As a logistics management entrepreneur, I want to register the data of a new trip to have a saved record and show transparency to my clients.

    @TS02 @API @Trips @POST
    Scenario: Register a new trip successfully
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        When the entrepreneur sends a POST request with the trip data to the API
            | Endpoint      | Method | Body                                                                                                                                                                                                                                                                                             |
            | /api/v1/trips | POST   | {"name": "Lima to Callao", "type": "Electronics", "weight": 100, "addressFrom": "Av. Lima 123", "scheduledDepartureDate": "2024-01-01T09:00:00", "addressTo": "Av. Peru 456", "scheduledArrivalDate": "2024-01-01T12:00:00", "driverId": 1, "vehicleId": 1, "clientId": 1, "entrepreneurId": 1} |
        Then the API responds with status code 200
        And the trip is correctly added to the database

    @TS02 @API @Trips @GET
    Scenario: Get registered trip information
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered trip with ID 1
        When the entrepreneur sends a GET request to the API to obtain the information of a specific trip
            | Endpoint        | Method |
            | /api/v1/trips/1 | GET    |
        Then the API responds with status code 200
        And returns the requested trip data

    @TS02 @API @Trips @Error
    Scenario: Error registering trip with invalid data
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a POST request with invalid trip data
            | Endpoint      | Method | Body                                                        |
            | /api/v1/trips | POST   | {"name": "", "type": "", "weight": -1, "entrepreneurId": 0} |
        Then the API responds with status code 400
        And returns a validation error message
