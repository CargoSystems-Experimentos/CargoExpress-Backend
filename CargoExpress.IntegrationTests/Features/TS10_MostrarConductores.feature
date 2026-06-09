@wip
Feature: TS10 - Driver data display
    As a logistics management entrepreneur, I want to view drivers' data to see the information of each one in an orderly manner.

    @TS10 @API @Drivers @GET
    Scenario: Get all registered drivers
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are registered drivers in the system
        When the entrepreneur sends a GET request to obtain all drivers
            | Endpoint        | Method |
            | /api/v1/drivers | GET    |
        Then the API responds with status code 200
        And returns the list of all drivers

    @TS10 @API @Drivers @GET
    Scenario: Get specific driver information by ID
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered driver with ID 1
        When the entrepreneur sends a GET request to obtain the information of a specific driver
            | Endpoint          | Method |
            | /api/v1/drivers/1 | GET    |
        Then the API responds with status code 200
        And returns the requested driver data

    @TS10 @API @Drivers @Error
    Scenario: Error getting driver with nonexistent ID
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a GET request for a driver that does not exist
            | Endpoint            | Method |
            | /api/v1/drivers/999 | GET    |
        Then the API responds with status code 404
        And returns a not found error message
