# E-Commerce Web Application

## Overview

The **E-Commerce Web Application** is a comprehensive platform designed to facilitate online shopping experiences. It enables users to browse products, manage their shopping carts, and place orders, while providing administrators with tools to manage products, categories, and orders. The application is built using ASP.NET Core, adhering to clean architecture principles to ensure scalability, maintainability, and testability.

## Purpose

This application simplifies the process of:
- Providing a seamless shopping experience for customers.
- Offering administrative functionalities for managing products, categories, and orders.
- Ensuring secure user authentication and authorization using JWT tokens.

It is ideal for businesses and organizations seeking to establish an online presence and streamline their e-commerce operations.

## Architectural Design

The application uses a layered architecture with the following components:

### 1. **Controllers** (API Layer)
- Handle HTTP requests and responses.
- Act as the entry point for the client to interact with the application.
- Delegate business logic to services.

### 2. **Services** (Business Logic Layer)
- Contain the core logic of the application.
- Interact with repositories to fetch or modify data.
- Enforce rules and conditions before data is saved or returned.

### 3. **Repositories** (Data Access Layer)
- Abstract database operations, providing a clean interface for the services.
- Use Entity Framework Core to query and manipulate the database.

### 4. **Models**
- Represent the entities in the database, such as `Product`, `Category`, `Cart`, `Order`, and `User`.
- Define relationships between entities (e.g., a product belongs to a category, and an order contains multiple order items).

### 5. **DTOs (Data Transfer Objects)**
- Used to transfer data between layers or between the server and client.
- Ensure only necessary fields are exposed in API responses.

### 6. **AutoMapper**
- Simplifies mapping between Models and DTOs.
- Reduces boilerplate code by automating object-to-object mapping.

## Technologies and Their Roles

- **ASP.NET Core**: The framework for building the application.
- **Entity Framework Core**: Used as an ORM for interacting with the SQL Server database.
- **SQL Server**: Stores persistent data for products, categories, orders, carts, and users.
- **AutoMapper**: Automates the mapping between Models and DTOs.
- **JWT**: Provides secure authentication and authorization.
- **Swagger**: Facilitates API documentation and testing.

## Key Features and Routes

### Product Management (ProductController)
- **GET /api/products**: Retrieve a list of all products.
- **GET /api/products/{id}**: Retrieve details of a specific product by ID.
- **POST /api/products**: Create a new product (Admin access required).
- **PUT /api/products/{id}**: Update an existing product (Admin access required).
- **DELETE /api/products/{id}**: Delete a product (Admin access required).

### Category Management (CategoryController)
- **GET /api/categories**: Retrieve a list of all categories.
- **GET /api/categories/{id}**: Retrieve details of a specific category by ID.
- **POST /api/categories**: Create a new category (Admin access required).
- **PUT /api/categories/{id}**: Update an existing category (Admin access required).
- **DELETE /api/categories/{id}**: Delete a category (Admin access required).

### Cart Management (CartController)
- **GET /api/carts/user/{userId}**: Retrieve the cart for a specific user.
- **POST /api/carts**: Create a new cart.
- **DELETE /api/carts/{cartId}**: Delete a cart and restore stock.

### Order Management (OrderController)
- **GET /api/orders**: Retrieve all orders for the authenticated user.
- **GET /api/orders/{orderId}**: Retrieve details of a specific order by ID.
- **POST /api/orders**: Create a new order for the authenticated user.
- **PATCH /api/orders/{orderId}/status**: Update the status of an order (Admin access required).
- **DELETE /api/orders/{orderId}**: Delete an order.

### User Management (UserController)
- **POST /api/user/register**: Register a new user.
- **POST /api/user/login**: Authenticate a user and issue a JWT token.
- **GET /api/user/profile**: Retrieve the authenticated user’s profile.
- **GET /api/user/{userId}/verify-email**: Verify a user’s email using a token.

### Authentication (AuthController)
- **POST /api/auth/login**: Authenticate a user and provide a JWT token.
- **POST /api/auth/password-reset**: Request a password reset.
- **POST /api/auth/reset-password**: Reset a user’s password using a token.

## Why This Architecture?

- **Separation of Concerns**: Each layer (Controller, Service, Repository) has a single responsibility, making the system easier to maintain and scale.
- **Reusability**: Business logic in services and data access in repositories can be reused across multiple controllers.
- **Testability**: Layers are loosely coupled, allowing for easier unit testing of individual components.
- **Scalability**: The architecture supports adding new features with minimal changes to existing components.

By structuring the application this way, it achieves both flexibility and robustness, ensuring it meets current needs while being adaptable for future requirements.

