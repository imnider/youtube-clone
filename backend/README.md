# Backend
El Backend de este Bootcamp es desarrollado con .NET

## Requisitos
- Visual Studio Community
- Postman

## Recomendaciones
### Generales
- Siempre usar Soluciones para los proyectos.
- Los Proyectos dentro de una Solucion deben tener la estructura SolutionName.ProjectName.

## Flujo recomendado para crear un proyecto
### Capas
- Crear un ProjectName.WebApp como ASP.NET Core Web App
- Crear un ProjectName.Application como Library
- Crear un ProjectName.Shared como Library
- Crear un ProjectName.Domain como Library

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

### Referencias de Proyecto
- Capa Application: Shared
- Capa WebApp: Application y Domain

### Instalar Paquetes NuGet
- Capa Domain
	- Microsoft.EntityFrameworkCore
	- Microsoft.EntityFrameworkCore.SqlServer
	- Microsoft.EntityFrameworkCore.Tools
- Capa WebApp
	- Microsoft.EntityFrameworkCore.Design

### Archivos (Orden sugerido)
1. Cache.cs - `Shared/`
2. DataTimeHelper.cs - `Shared/Helpers/`
3. ValidationConstants.cs - `Shared/Constants/`
4. GenericResponse.cs - `Application/Models/Responses/`
5. ResponsesHelper.cs - `Application/Helpers/`
6. EntityDto.cs - `Application/Models/DTOs/`
7. Requests - `Application/Models/Requests/EntityName/`
8. IService - `Application/Interface/Services/`
9. Service - `Application/Services/`
10. Controller - `WebApp/Controllers/`

## Enlaces Útiles
- https://http.cat
- https://refactoring.guru/design-patterns
- https://www.uuidgenerator.net
- https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/models-data/validation-with-the-data-annotation-validators-cs
- https://github.com/alenj0x1/net-ef
