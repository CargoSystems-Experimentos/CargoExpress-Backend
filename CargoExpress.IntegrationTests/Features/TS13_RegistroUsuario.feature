@wip
Feature: TS13 - User Registration
    As a user, I want to register in the application to have authorized and personalized access.

    @TS13 @API @Authentication @POST
    Scenario: Register a new user successfully
        Given the user has access to the API documentation
        When the user sends a POST request with their registration data to the API
            | Endpoint                       | Method | Body                                                                                               |
            | /api/v1/authentication/sign-up | POST   | {"username": "newUser123", "password": "SecurePass1!", "phone": "987654321", "isEntrepreneur": true} |
        Then the API responds with status code 200
        And the user is correctly registered in the system

    @TS13 @API @Authentication @GET
    Scenario: Get user information after registration
        Given the user has access to the API documentation and the necessary credentials for integration
        And there is a registered user with ID 1
        When the user sends a GET request to obtain the information of a specific user
            | Endpoint        | Method |
            | /api/v1/users/1 | GET    |
        Then the API responds with status code 200
        And returns the requested user data

    @TS13 @API @Authentication @Error
    Scenario: Error registering user with invalid data
        Given the user has access to the API documentation
        When the user sends a POST request with invalid registration data
            | Endpoint                       | Method | Body                                              |
            | /api/v1/authentication/sign-up | POST   | {"username": "", "password": "", "phone": ""}     |
        Then the API responds with status code 400
        And returns a validation error message
