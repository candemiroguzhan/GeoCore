# GeoCore 🌍

[![Stars](https://img.shields.io/github/stars/candemiroguzhan/geocore?style=social)](https://github.com/candemiroguzhan/geocore) [![Forks](https://img.shields.io/github/forks/candemiroguzhan/geocore?style=social)](https://github.com/candemiroguzhan/geocore) [![Issues](https://img.shields.io/github/issues/candemiroguzhan/geocore)](https://github.com/candemiroguzhan/geocore/issues) [![Contributors](https://img.shields.io/github/contributors/candemiroguzhan/geocore)](https://github.com/candemiroguzhan/geocore/graphs/contributors) [![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](./LICENSE) 

A reusable Clean Architecture-based geospatial core library for .NET services. GeoCore provides domain models, repository abstractions, EF Core/PostGIS infrastructure, spatial query helpers, geometry utilities, validation, mapping, caching, logging, and resilience support for production-ready geospatial applications.

Built with .NET 10, Entity Framework Core, PostgreSQL/PostGIS, NetTopologySuite, FluentValidation, Mapster, Serilog, Polly, Scrutor, and Redis.

Note: Badge URLs can be configured for your repository, for example:
`https://github.com/candemiroguzhan/geocore`

## ✨ Features

- 🧭 Clean Architecture project structure
- 🗺️ Geometry-aware repository layer
- 🐘 PostgreSQL/PostGIS support with EF Core
- 📐 NetTopologySuite-based geometry operations
- ✅ FluentValidation support for spatial DTOs and geometry rules
- 🔁 Mapster-based entity/DTO mapping
- ⚡ Redis-backed caching abstractions
- 🧱 Deterministic namespaced cache keys
- 🛡️ Polly retry, timeout, and circuit breaker policies
- 📋 Serilog logging configuration extensions
- 🔍 Scrutor-based dependency scanning
- 🧪 Unit tests for repositories, validation, mapping, cache, and spatial services

## 🧪 Technology

- .NET 10 — Target framework
- C# — Core language
- Entity Framework Core — Data access
- PostgreSQL + PostGIS — Spatial database support
- Npgsql EF Core Provider — PostgreSQL integration
- NetTopologySuite — Geometry operations
- FluentValidation — Validation layer
- Mapster — Object mapping
- Serilog — Structured logging
- Polly — Resilience policies
- Scrutor — Dependency scanning
- Redis — Distributed caching
- xUnit — Testing

## 📁 Project Structure
```text
├─ GeoCore.Domain
│  ├─ Entities
│  ├─ Enums
│  ├─ ValueObjects
│  └─ Abstractions
│
├─ GeoCore.Application
│  ├─ DTOs
│  ├─ Repositories
│  ├─ Services
│  ├─ Validators
│  ├─ Mapping
│  └─ Caching
│
├─ GeoCore.Infrastructure
│  ├─ Persistence
│  ├─ Repositories
│  ├─ Services
│  ├─ Caching
│  ├─ Logging
│  ├─ Resilience
│  ├─ Spatial
│  └─ DependencyInjection
│
├─ GeoCore.Shared
│  ├─ Models
│  ├─ Exceptions
│  ├─ Constants
│  └─ Extensions
│
├─ GeoCore.Tests
└─ GeoCore.sln
```
## 📋 Requirements

- .NET 10 SDK
- PostgreSQL with PostGIS extension
- Redis, optional for distributed caching
- Docker, optional for Testcontainers-based integration tests

## ⚙️ Installation

Clone the repository:
```bash
git clone https://github.com/candemiroguzhan/geocore.git
cd geocore
```
Restore dependencies:
```bash
dotnet restore
```
Build the solution:
```bash
dotnet build GeoCore.sln
```
Run tests:
```bash
dotnet test GeoCore.sln
```
## ▶️ Usage

Register GeoCore infrastructure in your .NET service:
```csharp
using GeoCore.Infrastructure.DependencyInjection;

builder.Services.AddGeoCoreInfrastructure(
    builder.Configuration.GetConnectionString("Postgres")!);
```
Enable Redis caching:
```csharp
builder.Services.AddGeoCoreRedisCaching(builder.Configuration);
```
Use geometry repositories:
```csharp
public sealed class PlaceSearchService
{
    private readonly IGeometryRepository<SamplePlace> _places;

    public PlaceSearchService(IGeometryRepository<SamplePlace> places)
    {
        _places = places;
    }

    public Task<IReadOnlyList<SamplePlace>> FindNearbyAsync(
        Geometry geometry,
        double distance,
        CancellationToken cancellationToken)
    {
        return _places.WithinDistanceAsync(
            geometry,
            distance,
            cancellationToken: cancellationToken);
    }
}
```
## ⚙️ Configuration

PostGIS must be enabled in your PostgreSQL database:
```sql
CREATE EXTENSION IF NOT EXISTS postgis;
```
Example Redis configuration:
```json
{
  "ConnectionStrings": {
    "Postgres": "Host=localhost;Database=geocore;Username=postgres;Password=postgres",
    "Redis": "localhost:6379"
  }
}
```
Example Serilog configuration:
```json
{
  "GeoCore": {
    "Logging": {
      "MinimumLevel": "Information",
      "Console": {
        "Enabled": true
      },
      "File": {
        "Path": "logs/geocore-.log",
        "RetainedFileCountLimit": 14
      }
    }
  }
}
```
## 🧩 Core Capabilities

GeoCore includes reusable services for:

- WKT / WKB / GeoJSON conversion
- Geometry validation
- SRID management
- Buffer, union, difference, and intersection operations
- Envelope and bounding box calculation
- Distance, area, and length calculation
- Geometry normalization and simplification
- PostGIS-compatible spatial queries

## 🤝 Contributing

Contributions and improvements are welcome. You can:

- Open issues for bugs or feature requests
- Submit pull requests with fixes or enhancements
- Improve documentation and examples
- Add more spatial service implementations

Please keep the architecture clean and avoid introducing infrastructure dependencies into the domain or shared layers.

## 👤 Author

Oğuzhan CANDEMİR — Geospatial Software Developer

GitHub: `@candemiroguzhan`

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for details.

## ⭐ Support

If you find this project useful:

- Star the repository ⭐
- Share feedback
- Contribute enhancements

Thank you for your support!