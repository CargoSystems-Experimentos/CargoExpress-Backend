@wip
Feature: TS11 - Vehicle data display
    As a logistics management entrepreneur, I want to view vehicles' data to see the information of each one in an orderly manner.

    @TS11 @API @Vehicles @GET
    Scenario: Get all registered vehicles
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are registered vehicles in the system
        When the entrepreneur sends a GET request to obtain all vehicles
            | Endpoint         | Method |
            | /api/v1/vehicles | GET    |
        Then the API responds with status code 200
        And returns the list of all vehicles

    @TS11 @API @Vehicles @GET
    Scenario: Get specific vehicle information by ID
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there is a registered vehicle with ID 1
        When the entrepreneur sends a GET request to obtain the information of a specific vehicle
            | Endpoint           | Method |
            | /api/v1/vehicles/1 | GET    |
        Then the API responds with status code 200
        And returns the requested vehicle data

    @TS11 @API @Vehicles @Error
    Scenario: Error getting vehicle with nonexistent ID
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a GET request for a vehicle that does not exist
            | Endpoint             | Method |
            | /api/v1/vehicles/999 | GET    |
        Then the API responds with status code 404
        And returns a not found error message
