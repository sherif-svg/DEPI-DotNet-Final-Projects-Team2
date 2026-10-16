"# DEPI-DotNet-Final-Projects-Team2" 
# 🏥 Hospital Inventory & Blood Requests Module

A backend module built using **ASP.NET Core Web API** as part of the Blood Donation & Management System, responsible for handling hospital inventory, blood requests, and lifecycle tracking.

---

## 📋 Detailed Tasks & Features

- **Inventory & Requests Management**: Adding, updating, and managing hospital inventory items and blood supply levels.
- **Blood Request Lifecycle**: Designing and controlling the lifecycle of blood requests (`Blood Request Lifecycle`).
- **DTO Implementation (`BloodRequestDTO`)**: Creating data transfer objects carrying essential fields between layers, including:
  - Hospital ID (`HospitalId`)
  - Required Blood Type (`RequiredBloodType`)
  - Quantity (`Quantity`)
  - Request Status
- **Service Layer (`IBloodRequestService`)**: Implementing core business logic methods such as:
  - `GetPendingRequests`
  - `CreateRequest`

---

## 🛠️ Tech Stack & Architecture

- **Framework**: .NET / ASP.NET Core Web API
- **Architecture**: Clean Architecture / N-Tier (Controllers, Services, DTOs)
- **Database & Tools**: SQL Server, Entity Framework Core, Swagger UI, Visual Studio, Git/GitHub

---

## 👨‍💻 Author & Developer
- **Developer**: Ali Ibrahim Ali
-
