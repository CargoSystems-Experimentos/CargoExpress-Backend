@wip
Feature: TS09 - Entrepreneur's Travel Visualization
    As a logistics management entrepreneur, I want to view all my registered trips to have a detailed record and access the information of each one at any time.

    @TS09 @API @Trips @GET
    Scenario: Get all trips for an entrepreneur
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are registered trips associated with entrepreneur ID 1
        When the entrepreneur sends a GET request to obtain all their trips
            | Endpoint                      | Method |
            | /api/v1/entrepreneurs/1/trips | GET    |
        Then the API responds with status code 200
        And returns the list of trips for the entrepreneur

    @TS09 @API @Trips @GET
    Scenario: Get all trips returns complete trip list
        Given the entrepreneur has access to the API documentation and the necessary credentials for integration
        And there are multiple registered trips in the system
        When the entrepreneur sends a GET request to obtain all trips
            | Endpoint      | Method |
            | /api/v1/trips | GET    |
        Then the API responds with status code 200
        And returns the complete list of trips

    @TS09 @API @Trips @Error
    Scenario: Error getting trips for a nonexistent entrepreneur
        Given the entrepreneur has access to the API documentation
        When the entrepreneur sends a GET request for trips of a nonexistent entrepreneur
            | Endpoint                        | Method |
            | /api/v1/entrepreneurs/999/trips | GET    |
        Then the API responds with status code 200
        And returns an empty list of trips
