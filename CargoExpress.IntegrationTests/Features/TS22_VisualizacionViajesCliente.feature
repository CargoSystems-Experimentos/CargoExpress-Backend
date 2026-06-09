@wip
Feature: TS22 - Customer Journey Visualization
    As a logistics company client, I want to view all my registered shipments to track my dispatches and access the details of each one.

    @TS22 @API @Clients @GET
    Scenario: Get all shipments for a client
        Given the client has access to the API documentation and the necessary credentials for integration
        And there are registered trips associated with client ID 1
        When the client sends a GET request to obtain all their shipments
            | Endpoint                | Method |
            | /api/v1/clients/1/trips | GET    |
        Then the API responds with status code 200
        And returns the list of shipments for the client

    @TS22 @API @Clients @GET
    Scenario: Get specific client information by ID
        Given the client has access to the API documentation and the necessary credentials for integration
        And there is a registered client with ID 1
        When the client sends a GET request to obtain their profile information
            | Endpoint          | Method |
            | /api/v1/clients/1 | GET    |
        Then the API responds with status code 200
        And returns the requested client data

    @TS22 @API @Clients @Error
    Scenario: Error getting shipments for a nonexistent client
        Given the client has access to the API documentation
        When the client sends a GET request for shipments of a nonexistent client
            | Endpoint                  | Method |
            | /api/v1/clients/999/trips | GET    |
        Then the API responds with status code 200
        And returns an empty list of shipments
