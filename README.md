# EcommerceMariaDB
## Overview
This project is a backend service for an e-commerce platform built using .NET Core and MariaDB. It provides RESTful APIs to manage products, users, orders, and other related functionalities. The service is designed to be scalable, secure, and easy to maintain.

## Features
* User Authentication and Authorization (JWT)
* Product Management
* Order Management
* Category Management
* Shopping Cart
* Payment Integration
* Logging and Error Handling

## Requirements
* .NET Core 7.0 or later
* MariaDB 11.3 or later
* Visual Studio 2019 or later / Visual Studio Code
* Postman (optional, for API testing)

## Getting Started
### Installation
1. Clone the repository:
   ```
   git clone https://github.com/Jatin-1602/EcommerceBackend.git`
2. Set up the database:
   * Install MariaDB and create a new database :
   ```
    CREATE DATABASE ecommerce_db;
3. Configure the connection string in `appsettings.json`:
   ```json
   {
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=ecommerce_db;User=user_name;Password=your_password;"
     }
   }
4. Apply migrations to set up the database schema:
   ```
   dotnet ef database update
5. Build and run the application:
   ```
   dotnet build
   dotnet run

## Usage
### API Endpoints
The API endpoints are documented using Swagger. Once the application is running, navigate to http://localhost:5000/swagger to view the API documentation and test the endpoints.
