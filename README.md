# 🛠️ TitanHelp

TitanHelp is a layered help desk ticket management system built for the CEN 4031 course at St. Petersburg College. It streamlines the process of creating, tracking, and resolving support tickets using a clean architecture and modern .NET technologies.

## 📦 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)

---

## ✨ Features

- Create, view, and manage support tickets
- Layered architecture: Presentation, Business Logic, Data Access
- ASP.NET Core MVC frontend
- Entity Framework Core with SQLite backend
- DTOs and service interfaces for clean separation
- Client-side and server-side validation

---

## 🏗️ Architecture

TitanHelp follows a **three-layered architecture**:

Presentation Layer (ASP.NET Core MVC)  
↓  
Business Logic Layer (Services, DTOs)  
↓  
Data Access Layer (EF Core, Repositories)  


- **Presentation Layer**: Handles user interaction via controllers and views.
- **Business Logic Layer**: Contains services and validation logic.
- **Data Access Layer**: Manages database operations using EF Core and SQLite.

---

## 🧰 Tech Stack

| Layer              | Technology             |
|-------------------|------------------------|
| Framework          | .NET                   |
| Presentation       | ASP.NET Core MVC       |
| ORM                | Entity Framework Core  |
| Database           | SQLite                 |
| Testing            | MSTest                 |
| Mocking            | Moq                    |

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) or VS Code

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/gorbs27/titanhelp-gorbet.git
   cd titanhelp-gorbet
   
2. Restore dependencies:
   dotnet restore

3. Build solution:
   dotnet build

4. Apply migrations and update the database:
   dotnet ef database update

5. Run the application:
   dotnet run

Use your web browser to navigate to http://localhost:5000
