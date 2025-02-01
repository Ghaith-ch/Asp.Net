# Learning Management System (LMS)

## Overview
The **Learning Management System (LMS)** is a web-based application designed to manage online courses, student enrollments, and user roles. It provides seamless interactions for administrators, instructors, and students to create, manage, and participate in courses. The application follows a clean, layered architecture to ensure scalability, maintainability, and testability.

## Purpose
This application simplifies the following tasks:
- **Instructors** can manage and create courses.
- **Students** can enroll in courses and track their progress.
- **Administrators** can manage users, roles, courses, and enrollments.
- **Secure role-based authentication and authorization** ensures the correct access levels for each type of user.

## Key Features
- **User Management**: Register, login, update, and delete users with role-based access control.
- **Course Management**: Create, update, delete, and view courses.
- **Enrollment Management**: Enroll students in courses, update enrollment details, and track grades.
- **Role-Based Access Control**: Secure endpoints for **Admin**, **Instructor**, and **Student** roles.
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
1. **ApplicationUser**: Represents users with roles (Admin, Instructor, Student).
2. **Course**: Represents courses created by instructors.
3. **Enrollment**: Represents student enrollments in courses.


## API Endpoints

### User Management (`UserController`)
- **POST /api/user/register**: Register a new user with the "Student" role.
- **POST /api/user/login**: Authenticate a user and provide a JWT token.
- **GET /api/user/{userId}**: Retrieve details of a specific user.
- **GET /api/user**: Retrieve all users (requires Admin role).
- **PUT /api/user/{userId}**: Update user details (Admin role required).
- **DELETE /api/user/{userId}**: Delete a user (Admin role required).

### Role Management (`UserManagementController`)
- **POST /api/usermanagement/{userId}/role**: Assign a role to a user (Admin role required).
- **GET /api/usermanagement/roles/{role}**: Retrieve users by role (Admin role required).
- **DELETE /api/usermanagement/{userId}/role**: Remove a role from a user (Admin role required).

### Course Management (`CourseController`)
- **GET /api/course/instructor/{instructorId}/courses**: Retrieve courses taught by a specific instructor (Admin/Instructor roles required).
- **GET /api/course/{courseId}**: Retrieve details of a specific course.
- **GET /api/course**: Retrieve a list of all courses.
- **POST /api/course**: Create a new course (Admin role required).
- **PUT /api/course/{courseId}**: Update an existing course (Admin/Instructor roles required).
- **DELETE /api/course/{courseId}**: Delete a course (Admin role required).

### Enrollment Management (`EnrollmentController`)
- **POST /api/enrollment**: Create a new enrollment (Admin/Instructor roles required).
- **GET /api/enrollment/{enrollmentId}**: Retrieve details of a specific enrollment.
- **GET /api/enrollment**: Retrieve a list of all enrollments.
- **PUT /api/enrollment/{enrollmentId}**: Update enrollment details (Admin/Instructor roles required).
- **DELETE /api/enrollment/{enrollmentId}**: Delete an enrollment (Admin role required).

## Security
- **Authentication**: JWT tokens secure API endpoints.
- **Authorization**: Role-based access control enforces appropriate permissions.
- **Data Protection**: Sensitive data like passwords are hashed.


## How to use it?

### 1. Clone the Repository

### 2. Restore NuGet Packages
Run the following command in the project root to restore required NuGet packages:
dotnet restore

### 3. Configure the Database
- Open the `appsettings.json` file and update the `ConnectionStrings` section to match your SQL Server setup. Example:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=LMSDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

- Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 4. Configure JWT Authentication
- In the `appsettings.json` file, ensure the `JWT` section is correctly configured:
```json
"JWT": {
  "Issuer": "http://localhost:5219",
  "Audience": "http://localhost:5219",
  "SigningKey": "your-signing-key"
}
```
- Replace `your-signing-key` with a secure random string.


## Database Setup

### 5. Apply Migrations and Create the Database
Run the following commands to create the database and apply migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This will create a database named `LMSDB` in your SQL Server with the required schema.

---

### 6. Add Random Test Data
To test the application, insert some seed data into the database using SQL script:

## Running the Application

### 7. Run the App
To run the app, use the following command:
```bash
dotnet build
dotnet run
```

## Testing the API

### 9. Swagger UI
- Navigate to `http://localhost:5219/swagger` to access the Swagger UI for testing endpoints.

### 10. Postman
- Use Postman to test the API endpoints. Use the `/api/User/login` endpoint to get a JWT token for authentication, then pass the token in the `Authorization` header for other API calls.

---

Happy coding!
