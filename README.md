Support Form Management Panel Application

Project Details

The project is designed on an N-Tier Architecture.
API documentation is provided through Swagger.
Authentication is handled via JWT (JSON Web Tokens).
Database operations are managed with Entity Framework Core.
The project is built on the ASP.NET Core Web API infrastructure.
Usage

The project is managed through Swagger; after performing database migrations, a "superadmin" is seeded as initial data.
After accessing the panel, a login is required. The "superadmin" user's password, which is stored hashed in the database, is set to "superadmin".
Upon successful login, a token is issued to the user. This token should be entered with the "Bearer" prefix in Swagger's "Authorize" button at the top right. Once authorized, the user can access administrative endpoints in the application.
Features

There are three user types: Superadmin, Admin, and Customer, with role-based access control applied throughout the project.

The Superadmin has the highest level of access and can reach all endpoints.

The Admin can access all endpoints except those specifically reserved for Superadmin privileges.

The Customer has restricted access, limited to managing their own support requests and accessing their personal settings.

The token assigned to the user is automatically invalidated after 15 minutes.
