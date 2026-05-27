# ACME CargoExpress
## Summary
ACME CargoExpress API Application, made with Microsoft C#, ASP.NET Core, Entity Framework Core and MySQL persistence. It also illustrates open-api documentation configuration and integration with Swagger UI.

## Features
- RESTful API
- OpenAPI Documentation
- Swagger UI
- ASP.NET Framework
- Entity Framework Core
- Audit Creation and Update Date
- Custom Route Naming Conventions
- Custom Object-Relational Mapping Naming Conventions.
- MySQL Database
- Domain-Driven Design

## Bounded Contexts
This version of ACME CargoApp is divided into two bounded contexts: Registration, and User.

### Registration Context

The Registration Context is responsible for managing the registrations made by entrepreneur users. It includes the following features:

- Create a new trip, expense, driver or vehicle.
- Get a trip, expense, driver or vehicle by id.
- Get all trips, expenses, drivers or vehicles.

### User Context

The User Context is responsible for managing the users. It includes the following features:

- Create a new user (client or entrepreneur)
- Get an user by id.
- Get a client by id.
- Get an entrepreneur by id.
- Get all users.
- Get all clients.
- Get all entrepreneurs.
