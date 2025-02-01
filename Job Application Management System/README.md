# Job Application Management System (JAMS)

## Overview
The **Job Application Management System (JAMS)** is a web-based application designed to streamline the process of job postings, job applications, and role-based user management. The system is designed for three types of users: **Administrators**, **Recruiters**, and **Applicants**.

## Purpose
This application simplifies the following tasks:
- **Recruiters** can post jobs, review applications, and manage job postings.
- **Applicants** can browse jobs, apply to them with a resume, and track the status of their applications.
- **Administrators** can manage users, roles, jobs, and applications across the platform.
- **Secure role-based authentication and authorization** ensures the correct access levels for each type of user.

## Key Features
- **User Management**: Register, login, update, and delete users with role-based access control.
- **Job Management**: Create, update, delete, and view job postings.
- **Application Management**: Apply for jobs, update application status, and delete applications.
- **Role-Based Access Control**: Secure endpoints for **Admin**, **Recruiter**, and **Applicant** roles.
- **JWT Authentication**: Secure API endpoints with JSON Web Tokens (JWT).
- **AutoMapper**: Simplifies object-to-object mapping between models and DTOs.
- **Swagger Documentation**: Interactive API documentation for easy testing.

## Technologies Used
- **ASP.NET Core**: Framework for building the application.
- **Entity Framework Core**: ORM for interacting with the database.
- **SQL Server**: Relational database for data storage.
- **JWT (JSON Web Tokens)**: Provides secure authentication and role-based authorization.
- **AutoMapper**: Library for object mapping.
- **Swagger**: API documentation and testing tool.

## Database Schema
The application uses the following entities:
1. **ApplicationUser**: Represents users with roles (Admin, Recruiter, Applicant).
2. **Job**: Represents job postings created by Recruiters or Admins.
3. **Application**: Represents job applications submitted by Applicants.

## API Endpoints

### User Management (`UserController`)
- **POST /api/user/register**: Register a new user with the "Applicant" role.
- **POST /api/user/login**: Authenticate a user and provide a JWT token.
- **GET /api/user/{userId}**: Retrieve details of a specific user.
- **GET /api/user**: Retrieve all users (Admin role required).
- **PUT /api/user/{userId}**: Update user details (Admin role required).
- **DELETE /api/user/{userId}**: Delete a user (Admin role required).
- **POST /api/user/assign-role**: Assign a role to a user (Admin role required).

### Job Management (`JobController`)
- **GET /api/job/{jobId}**: Retrieve details of a specific job.
- **GET /api/job**: Retrieve all job postings.
- **POST /api/job**: Create a new job posting (Recruiter/Admin roles required).
- **PUT /api/job/{jobId}**: Update an existing job posting (Recruiter/Admin roles required).
- **DELETE /api/job/{jobId}**: Delete a job posting (Recruiter/Admin roles required).

### Application Management (`ApplicationController`)
- **GET /api/application/{id}**: Retrieve details of a specific application.
- **GET /api/application/job/{jobId}**: Retrieve applications for a specific job (Recruiter/Admin roles required).
- **POST /api/application**: Apply for a job (Applicant role required).
- **PUT /api/application/{id}**: Update application status (Recruiter/Admin roles required).
- **DELETE /api/application/{id}**: Delete an application (Admin/Applicant roles required).

## Security
- **Authentication**: JWT tokens secure API endpoints.
- **Authorization**: Role-based access control enforces appropriate permissions.
- **Data Protection**: Sensitive data like passwords are hashed.

## Getting Started

### Prerequisites
- **.NET 6.0 SDK**: [Download .NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- **SQL Server**: Ensure SQL Server is installed and running.
- **Visual Studio 2022** (optional but recommended).

## How to use it?

### 1. Clone the Repository
Clone this repository to your local machine.

### 2. Restore NuGet Packages
Run the following command in the project root to restore required NuGet packages:
```bash
dotnet restore
```

### 3. Configure the Database
- Open the `appsettings.json` file and update the `ConnectionStrings` section to match your SQL Server setup. Example:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=JobApplicationDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
- Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 4. Configure JWT Authentication
- In the `appsettings.json` file, ensure the `JWT` section is correctly configured:
```json
"JWT": {
  "Issuer": "http://localhost:5152",
  "Audience": "http://localhost:5152",
  "SigningKey": "your-signing-key"
}
```
- Replace `your-signing-key` with a secure random string.

---

## Database Setup

### 5. Apply Migrations and Create the Database
Run the following commands to create the database and apply migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This will create a database named `JobApplicationDB` in your SQL Server with the required schema.

---

### 6. Add Random Test Data
To test the application, insert seed data into the database using SQL scripts.

## Running the Application

### 7. Run the App
To run the app, use the following commands:
```bash
dotnet build
dotnet run
```

## Testing the API

### 8. Swagger UI
- Navigate to `http://localhost:5152/swagger` to access the Swagger UI for testing endpoints.

### 9. Postman
- Use Postman to test the API endpoints. Use the `/api/User/login` endpoint to get a JWT token for authentication, then pass the token in the `Authorization` header for other API calls.

---

Happy coding!
