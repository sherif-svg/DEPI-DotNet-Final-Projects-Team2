# LifePulse - Smart Blood Donation & Emergency Matching Platform

**DEPI Graduation Project — Team 2**

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0-512BD4?logo=entityframework)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)](https://getbootstrap.com/)

## Executive Summary

LifePulse is an ASP.NET Core MVC platform that connects hospitals, verified donors, and urgent blood requests in real time. It applies ABO/Rh compatibility, a 56-day donor eligibility window, ranked geo-matching, and hospital inventory controls so the right unit reaches the right patient faster.

The solution uses Identity roles (`Admin`, `Hospital`, `Donor`), Entity Framework Core against SQL Server, and a medical-tech Bootstrap 5 UI.

## Architecture Overview

```
                     ┌──────────────────────────────┐
                     │     Browser / Bootstrap UI    │
                     │  Home · Requests · Donors     │
                     │  Inventory · Identity Pages   │
                     └──────────────┬───────────────┘
                                    │ MVC + Razor Pages
                     ┌──────────────▼───────────────┐
                     │         Controllers          │
                     │  BloodRequests · Donors      │
                     │  Hospitals · Home            │
                     └──────────────┬───────────────┘
                                    │
              ┌─────────────────────┼─────────────────────┐
              ▼                     ▼                     ▼
     ┌────────────────┐   ┌─────────────────┐   ┌──────────────────┐
     │ MatchingService│   │  DonorService   │   │ Inventory + Req. │
     │ ABO/Rh + rank  │   │ 56-day gap rule │   │ lifecycle + stock│
     └────────┬───────┘   └────────┬────────┘   └────────┬─────────┘
              │                    │                     │
              └────────────────────┼─────────────────────┘
                                   ▼
                     ┌──────────────────────────────┐
                     │     ApplicationDbContext      │
                     │  Users · Hospitals · Donors   │
                     │  Inventory · BloodRequests    │
                     └──────────────┬───────────────┘
                                    ▼
                     ┌──────────────────────────────┐
                     │         SQL Server            │
                     │      BloodDonationDb          │
                     └──────────────────────────────┘
```

## Core Business Logic & ABO/Rh Compatibility

### Compatibility matrix (who can donate **to** a recipient)

| Recipient | Compatible donor types                           |
|-----------|--------------------------------------------------|
| O-        | O-                                               |
| O+        | O-, O+                                           |
| A-        | O-, A-                                           |
| A+        | O-, O+, A-, A+                                   |
| B-        | O-, B-                                           |
| B+        | O-, O+, B-, B+                                   |
| AB-       | O-, A-, B-, AB-                                  |
| AB+       | O-, O+, A-, A+, B-, B+, AB-, AB+                 |

- **O-** is the universal donor.
- **AB+** is the universal recipient.
- Matches are ranked by **eligibility**, then **compatibility score**, then **distance (km)**.

### Donor eligibility

A donor may give blood only if `LastDonationDate` is null **or** at least **56 days** have passed, and the donor is marked available.

### Request lifecycle

`Pending` → `Matching` → `Fulfilled`  
`Pending` / `Matching` → `Cancelled`

On **fulfillment**:
1. Hospital inventory for the requested blood type is deducted (never below zero).
2. The matched donor’s `LastDonationDate` is set to UTC now.

### Hospital inventory

- Stock adjustments that would go negative are rejected.
- Units **below 5** are flagged as low-stock alerts on the dashboard.

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB or full instance)
- Connection string in `BloodDonationPlatform/appsettings.json`

Default connection:

```json
Server=.;Database=BloodDonationDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

### Database & run

```bash
cd BloodDonationPlatform
dotnet restore
dotnet ef database update
dotnet run
```

Browse to the URL printed in the console (typically `http://localhost:5207`).

## Default Seeded Credentials

| Role     | Email                         | Password       |
|----------|-------------------------------|----------------|
| Admin    | admin@blooddonation.com       | Admin@123456   |
| Hospital | cairo.hospital@health.gov.eg  | Hosp@123456    |
| Donor    | donor1@gmail.com              | Donor@123456   |

## Solution Map

| Area     | Path                            |
|----------|---------------------------------|
| Web app  | `BloodDonationPlatform/`        |
| Domain   | `Models/`, `Models/Enums/`      |
| Services | `Services/Implementations/`     |
| MVC views| `Views/`                        |
| Identity | `Areas/Identity/Pages/Account/` |

## Team

DEPI Graduation Project — Team 2 · 2026
