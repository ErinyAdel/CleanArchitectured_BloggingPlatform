## 🧩 Architecture Overview

This project follows **Clean Architecture** principles to ensure separation of concerns, high testability, and long-term maintainability. The solution is structured into clear layers:

- **Domain**  
  Contains entities, value objects, enums, domain rules, and core business logic.

- **Application**  
  Contains use cases, CQRS commands & queries, MediatR handlers, DTOs, and validation rules.

- **Infrastructure / Persistence**  
  Contains database access code, repositories, and integrations with external systems.

- **Web API**  
  The presentation layer that exposes endpoints to the outside world.

---

## Design Patterns & Techniques Used

### ⚡ Clean Architecture
A layered architecture approach where:
- Inner layers have no dependencies on outer layers.
- Business rules are isolated from infrastructure and frameworks.
- Code remains maintainable and testable over time.

### ⚡ CQRS (Command–Query Responsibility Segregation)
Reads and writes are separated into:
- **Commands** for state-changing operations  
- **Queries** for read-only data retrieval  
This improves scalability and clarity of intent.

### ⚡ Mediator Pattern (via MediatR)
The **Mediator pattern** is implemented using **MediatR**:
- Controllers send commands/queries through Mediator
- Handlers execute the logic
- Decouples request senders from business logic executors

### ⚡ Validation Pipeline (FluentValidation)
- All commands & queries have validation rules using FluentValidation
- A MediatR pipeline behavior runs validators automatically before handlers  
  This ensures consistent, centralized validation.

### ⚡ Dependency Injection
- All services, handlers, repositories, and validators are registered through the built-in .NET DI container.

### ⚡ Repository / Data Access Abstraction
- Data access concerns are abstracted behind interfaces in the Application layer
- Actual EF Core / data logic lives in the Persistence layer  

---

## Technologies & Frameworks

| Technology / Library | Usage |
|----------------------|--------|
| **.NET / C#** | Core Development Platform |
| **ASP.NET Core Web API** | HTTP API Layer |
| **MediatR** | Command/Query Dispatching, Mediator Pattern |
| **FluentValidation** | Strongly-typed Validation Rules |
| **Entity Framework Core** | Data Access & ORM |
| **SQL Server** | Database |
| **Docker** | Containerization & Environment Consistency |
| **CI/CD & Deployment** | GitHub Actions & Self-Hosted Windows Runner & IIS (Internet Information Services) |

---
# 🚀 CI/CD Pipeline for .NET Application (GitHub Actions → IIS on Windows)

This repository includes a fully automated **CI/CD pipeline** that builds and deploys the .NET WebAPI to **IIS** on a Windows machine using a **self-hosted GitHub Actions runner**.  
Deployment triggers automatically whenever changes are pushed to the `main` branch.

---

## 🔄 CI/CD Workflow Summary

Whenever code is pushed or merged into the **main** branch:

1. GitHub Actions Starts The `build-and-deploy` Workflow.
2. The Workflow Runs on a Windows **self-hosted runner**.
3. The Runner:
   - Restores Dependencies  
   - Builds The Project  
   - Publishes The App  
   - Copies The Published Files To The IIS Site Folder  
4. IIS Automatically Reloads The App When Files Change.

---

## 🖥️ Setting Up the Self-Hosted Runner (Windows)

### 1️⃣ Create a runner directory
```powershell
mkdir "D:\GitHubRunner"
cd "D:\GitHubRunner"

