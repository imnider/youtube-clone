# Backend
El Backend de este Bootcamp es desarrollado con .NET

## Requisitos
- Visual Studio Community
- Postman

## Recomendaciones
### Generales
- Siempre usar Soluciones para los proyectos.
- Los Proyectos dentro de una Solucion deben tener la estructura SolutionName.ProjectName.
- Si se trabaja con base de datos, no olvidar modificar el appsettings.json del WebApi.

## Flujo recomendado para crear un proyecto
### Capas
- Crear un ProjectName.WebApp como ASP.NET Core Web App
- Crear un ProjectName.Application como Library
- Crear un ProjectName.Shared como Library
- Crear un ProjectName.Domain como Library
- Crear un ProjectName.Infrastructure como Library

### Carpetas de la Capa Application
- Helpers
- Interfaces
	- Services
- Models
	- DTOs
	- Requests
	- Responses
- Services

### Carpetas de la Capa Shared
- Constants
- Helpers

### Carpetas de la Capa Domain
- Database
	- SqlServer
		- Context
		- Entities
		- Repositories
- Exceptions
- Interfaces
	- Repositories

### Carpetas de la Capa Infrastructure
- Persistence
	- SqlServer
		- Repositories

### Carpetas de la Capa WebApp
- Extensions
- Middlewares

### Referencias de Proyecto
- Capa Application: Shared y Domain
- Capa Infrastructure: Domain
- Capa WebApp: Application, Domain e Infrastructure

### Instalar Paquetes NuGet
- Capa Domain
	- Microsoft.EntityFrameworkCore
	- Microsoft.EntityFrameworkCore.SqlServer
	- Microsoft.EntityFrameworkCore.Tools
- Capa WebApp
	- Microsoft.EntityFrameworkCore.Design

### Comandos en la Terminal
- Scaffolding: `dotnet ef dbcontext scaffold "Server=localhost,1433;User=sa;Password=Admin1234@;Database=YoutubeClone;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project YoutubeClone.Domain --startup-project YoutubeClone.WebApp --context-dir Database/SqlServer/Context --output-dir Database/SqlServer/Entities`

### Archivos (Orden sugerido)
1. Cache.cs - `Shared/` (Solo para caché)
2. DataTimeHelper.cs - `Shared/Helpers/`
3. ValidationConstants.cs - `Shared/Constants/`
4. ResponseConstants.cs - `Shared/Constants/`
5. GenericResponse.cs - `Application/Models/Responses/`
6. ResponsesHelper.cs - `Application/Helpers/`
7. BaseRequest.cs `Application/Requests`
8. DTOs - `Application/Models/DTOs/`
9. Requests - `Application/Models/Requests/EntityName/`
10. IServices - `Application/Interface/Services/`
11. Services - `Application/Services/`
12. Exceptions - `Domain/Exceptions`
13. ErrorHandlerMiddleware.cs `WebApp/Middlewares`
14. Controllers - `WebApp/Controllers/`

## Enlaces Útiles
- https://http.cat
- https://refactoring.guru/design-patterns
- https://www.uuidgenerator.net
- https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/validation-with-the-data-annotation-validators-cs
- https://github.com/alenj0x1/net-ef
