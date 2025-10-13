# Secrets Sharing API

## Project Overview
The **Secrets Sharing API** allows users to securely share files and text. Users can control whether shared content should be automatically deleted after being accessed.

### Main Features
- User registration and login with JWT authentication.
- File upload (local or S3 storage) with optional auto-delete after download.
- Text upload with optional auto-delete after access.
- Anonymous access to files/text via secure URLs.
- Clean Architecture with Generic Repository, Unit of Work, and MediatR.
- .NET 8, ASP.NET Core, EF Core.

---

## Technical Stack
- **Backend:** .NET 8, ASP.NET Core
- **Database:** Entity Framework Core (SQL Server / any EF-supported provider)
- **Authentication:** JWT
- **Architecture:** Clean Architecture (Application, Domain, Infrastructure, API)
- **Patterns:** Generic Repository, Unit of Work, MediatR
- **Storage:** Local filesystem or AWS S3
- **Containerization:** Docker + docker-compose

Optional:
- Swagger UI for API documentation
- Testing: Yes excluding S3 because I don't have S3 account
- Unit tests: Not yet

---

## Future Improvements

- **Caching**: Implement caching to reduce API load and improve response times.
- **Concurrent Request Handling**: Enhance the handling of multiple simultaneous requests to ensure thread-safety and better performance.
- **Rate Limiting**: Limit the number of requests per user to protect the API.
- **Security Enhancements**: Add advanced security mechanisms such as OAuth2, API keys, or encryption.

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (if using Docker)
- AWS account with S3 bucket (optional, for S3 storage)

### Setup

```
git clone https://github.com/dev-nguyen/FileGateway.git
cd FileGateway
run docker-compose up --build
```
### Swagger UI

```
http://localhost:5000/swagger/index.html
```

## Notes
- By default, the API runs on port **5000** (HTTP). Adjust `docker-compose.yml` if you need a different port.
- Currently, file storage is tested only on the local filesystem. S3 support is planned for future releases.
