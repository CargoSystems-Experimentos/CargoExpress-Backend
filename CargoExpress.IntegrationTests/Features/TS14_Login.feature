@wip
Feature: TS14 - Login
    As a user, I want to access my registered account to use the application's functions.

    @TS14 @API @Authentication @POST
    Scenario: Login with valid credentials successfully
        Given the user has a registered account in the system
        When the user sends a POST request with their login credentials to the API
            | Endpoint                       | Method | Body                                                      |
            | /api/v1/authentication/sign-in | POST   | {"username": "existingUser", "password": "SecurePass1!"}  |
        Then the API responds with status code 200
        And returns a valid authentication token

    @TS14 @API @Authentication @GET
    Scenario: Get user role after login
        Given the user has a registered account in the system
        And there is a registered user with ID 1
        When the user sends a GET request to obtain their role information
            | Endpoint             | Method |
            | /api/v1/users/1/role | GET    |
        Then the API responds with status code 200
        And returns the user role information

    @TS14 @API @Authentication @Error
    Scenario: Error login with invalid credentials
        Given the user has access to the API documentation
        When the user sends a POST request with invalid login credentials
            | Endpoint                       | Method | Body                                            |
            | /api/v1/authentication/sign-in | POST   | {"username": "wrongUser", "password": "wrong"}  |
        Then the API responds with status code 401
        And returns an authentication error message
