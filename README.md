# English Learning Website - Backend

This is the backend source code for an English learning platform, built with ASP.NET Core Web API using **Clean Architecture**. It provides RESTful APIs for features like user management, course content, quizzes, and learning progress, backed by a PostgreSQL database.

---

## 🧱 Clean Architecture Layers

/Api --> ASP.NET Core Web API (entry point)
/Application --> Application logic (use cases, interfaces, DTOs)
/Domain --> Enterprise business rules (entities, enums, interfaces)
/Infrastructure --> External concerns (EF Core, PostgreSQL, file storage, etc.)

Each layer follows the Dependency Inversion Principle – outer layers depend on inner layers, but not vice versa.

---

## 🚀 Features

- Clean Architecture (separation of concerns)
- RESTful API design
- User registration & JWT-based authentication
- Role-based authorization
- Course, Lesson & Quiz management
- Student progress tracking
- PostgreSQL with Entity Framework Core
- Swagger for API testing

---

## 🛠️ Tech Stack

- **ASP.NET Core Web API (.NET 9)**
- **C#**
- **PostgreSQL**
- **Entity Framework Core (EF Core)**
- **Clean Architecture**
- **JWT Authentication**
- **Swagger / Swashbuckle**
- **Custom Result Pattern**
- **Global ErrorHandlingMiddleware**

---
This architecture ensures that:

- Domain logic is isolated and reusable
- Infrastructure is swappable (e.g. DB, storage)
- Application layer depends only on abstractions
- Testing and scaling are much easier

---

## ⚠️ Error Handling & Result Pattern

### ✅ Result Pattern

All use cases return a `Result<T>` or `Result`, avoiding exceptions for business errors.

