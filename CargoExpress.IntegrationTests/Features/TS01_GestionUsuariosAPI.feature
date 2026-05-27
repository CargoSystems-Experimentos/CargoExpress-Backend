@wip
Feature: TS01 - Uso de nuestra API para gestionar usuarios
    As a developer I want to integrate an API to manage user information in the database

    @TS01 @API @Usuarios @POST
    Scenario: Add client/entrepreneur data to the database
        Given the developer has access to the API documentation and the necessary credentials for integration
        When the developer sends a POST request with the client/entrepreneur data to the API
            | Endpoint    | Method | Body                                           |
            | /api/v1/users| POST  | {"username": "newUser", "password": "pass"}   |
        Then the API responds with status code 200
        And the user is correctly added to the database

    @TS01 @API @Usuarios @GET
    Scenario: Get client/entrepreneur information
        Given the developer has access to the API documentation and the necessary credentials for integration
        And there is a registered user with ID 1
        When the developer sends a GET request to the API to obtain the information of a specific client/entrepreneur
            | Endpoint     | Method |
            | /api/v1/users/1 | GET  |
        Then the API responds with status code 200
        And returns the requested user data

    @TS01 @API @Usuarios @Error
    Scenario: Error adding user with invalid data
        Given the developer has access to the API documentation
        When the developer sends a POST request with invalid data
            | Endpoint    | Method | Body                             |
            | /api/v1/users| POST  | {"username": "", "password": "a"}|
        Then the API responds with status code 400
        And returns a validation error message