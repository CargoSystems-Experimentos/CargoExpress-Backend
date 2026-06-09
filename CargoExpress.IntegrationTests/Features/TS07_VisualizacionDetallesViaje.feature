@wip
Feature: TS07 - Viewing trip details
    As a logistics management entrepreneur, I want to access specific details about the delivery of my shipment to have a complete understanding of the delivery process.

    @TS07 @API @Trips @GET
    Scenario: View complete trip details by ID
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered trip with ID 1
        When the entrepreneur sends a GET request to obtain the complete details of a trip
            | Endpoint        | Method |
            | /api/v1/trips/1 | GET    |
        Then the API responds with status code 200
        And returns the complete trip details

    @TS07 @API @Trips @GET
    Scenario: View ongoing trip information for a trip
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered ongoing trip linked to trip with ID 1
        When the entrepreneur sends a GET request to obtain the ongoing trip information
            | Endpoint                      | Method |
            | /api/v1/trips/1/ongoing-trips | GET    |
        Then the API responds with status code 200
        And returns the ongoing trip data

    @TS07 @API @Trips @Error
    Scenario: Error viewing trip details with nonexistent ID
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a GET request for a trip that does not exist
            | Endpoint          | Method |
            | /api/v1/trips/999 | GET    |
        Then the API responds with status code 404
        And returns a not found error message
