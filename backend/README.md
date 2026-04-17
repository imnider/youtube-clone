# 🖥️ Backend — `with-database`

Backend completo del proyecto YoutubeClone. Implementa una API REST con **ASP.NET Core 9**, **Entity Framework Core**, **autenticación JWT**, **caché en memoria**, **logging con Serilog** y una arquitectura limpia en 4 capas siguiendo los principios de **Clean Architecture**.

---

## 📁 Estructura del proyecto

```
backend/with-database/YoutubeClone/
├── YoutubeClone.Application/       # Capa de aplicación (servicios, interfaces, modelos)
├── YoutubeClone.Domain/            # Capa de dominio (entidades, repositorios, contexto EF)
├── YoutubeClone.Infrastructure/    # Capa de infraestructura (implementación de repositorios)
├── YoutubeClone.Shared/            # Utilidades y constantes compartidas
├── YoutubeClone.WebApp/            # Capa de presentación (controladores, middlewares, DI)
└── YoutubeClone.slnx               # Archivo de solución
```

---

## 🏗️ Arquitectura de capas

### Dependencias entre capas

```
WebApp  ──►  Application  ──►  Domain
  │                │                ▲
  └──►  Shared ◄───┘                │
              │                     │
              └─────────────────────┘
Infrastructure ──► Domain
```

- **WebApp** depende de Application, Domain, Infrastructure y Shared.
- **Application** depende de Domain y Shared.
- **Infrastructure** depende de Domain.
- **Domain** no tiene dependencias internas.
- **Shared** no depende de ninguna otra capa.

---

## 📦 Dependencias NuGet por proyecto

### `YoutubeClone.WebApp`
- `Microsoft.AspNetCore.Authentication.JwtBearer` — autenticación JWT
- `Microsoft.AspNetCore.OpenApi` — generación de documentación OpenAPI
- `Serilog.AspNetCore` — integración de Serilog con ASP.NET Core
- `Serilog.Sinks.MSSqlServer` — escritura de logs en SQL Server
- `Serilog.Sinks.File` — escritura de logs en archivos

### `YoutubeClone.Application`
- `Microsoft.EntityFrameworkCore` — acceso base a EF Core
- `Microsoft.Extensions.Caching.Memory` — caché en memoria
- `Microsoft.IdentityModel.Tokens` — manejo de tokens JWT
- `System.IdentityModel.Tokens.Jwt` — generación/validación de JWT

### `YoutubeClone.Domain`
- `Microsoft.EntityFrameworkCore.SqlServer` — proveedor SQL Server para EF Core

### `YoutubeClone.Infrastructure`
- `Microsoft.EntityFrameworkCore` — acceso base a EF Core

### `YoutubeClone.Shared`
- Sin dependencias externas (solo .NET base)

---

## ⚙️ Configuración (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "Database": "Server=localhost,1433;User=sa;Password=Admin1234@;Database=YoutubeClone;TrustServerCertificate=True;"
  },
  "TokenConfiguration": {
    "Issuer": "YoutubeClone",
    "Audience": "YoutubeClone",
    "SecretKey": "...",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

> La clave `Database` es leída por `AddSqlServer<YoutubeCloneContext>()` en `ServiceCollectionExtension`.  
> Los valores de `TokenConfiguration` son leídos por `TokenHelper.Configuration()`.

---

## 🗂️ Capa: `YoutubeClone.Shared`

Utilidades y constantes transversales a toda la aplicación. No depende de ninguna otra capa.

### `Cache.cs`
Clase estática que define las claves de caché usadas en la aplicación, organizadas como constantes de string. Centraliza el nombrado de claves para evitar errores tipográficos.

### `Hasher.cs`
Implementa hashing de contraseñas usando **PBKDF2 con SHA-256**:
- `Hash(string password)` — genera un hash salteado de la contraseña.
- `Verify(string password, string hash)` — verifica si una contraseña en texto plano coincide con su hash.

### `Generate.cs`
Contiene métodos de generación de valores aleatorios usados en la capa de autenticación:
- `GenerateRefreshToken()` — genera un refresh token seguro usando `RandomNumberGenerator`.

### `Helpers/DateTimeHelper.cs`
Proveedor de fecha/hora UTC. Abstrae `DateTime.UtcNow` para facilitar pruebas y estandarizar el uso de UTC en todo el sistema.

### `Constants/ClaimsConstants.cs`
Define las claves de los claims JWT usados en los tokens de acceso (ej. `UserId`, `UserName`).

### `Constants/ConfigurationConstants.cs`
Define las claves de las secciones de `appsettings.json` usadas para leer configuración de token, base de datos y logging.

### `Constants/ResponseConstants.cs`
Mensajes de respuesta estandarizados para todos los endpoints:
- Mensajes de éxito/error para operaciones de usuario, autenticación y caché.
- Ejemplos: `USER_CREATED`, `AUTH_TOKEN_NOT_FOUND`, `USER_NOT_FOUND`.

### `Constants/ValidationConstants.cs`
Mensajes y límites usados en las validaciones de los modelos de request:
- Longitudes mínimas/máximas de campos como `UserName`, `Password`, `Email`.
- Mensajes de error de validación localizados.

---

## 🗂️ Capa: `YoutubeClone.Domain`

Define el núcleo del negocio: entidades, interfaces de repositorios y el contexto de base de datos. No depende de ninguna capa de la aplicación.

### `Database/SqlServer/Context/YoutubeCloneContext.cs`
Contexto de Entity Framework Core que mapea las entidades C# al esquema SQL Server. Generado mediante reverse engineering (`Scaffold-DbContext`) y ajustado manualmente.

**DbSets expuestos:**
- `Channels`, `Comments`, `CreatorTypes`, `Playlists`, `ReactionTypes`
- `Subscriptions`, `Tags`, `UserAccounts`, `Videos`, `VideoAccessibilities`
- `VideoReactions`, `ViewHistories`

**Configuraciones relevantes en `OnModelCreating`:**
- Claves primarias nombradas con el nombre de la constraint SQL (`PK__...`).
- Índices únicos en `Channel.Handle`, `UserAccount.Email`, `UserAccount.UserName`.
- Valores por defecto con `HasDefaultValueSql("(newid())")` para GUIDs y `(sysutcdatetime())` para fechas.
- Restricciones de longitud máxima en campos de texto.
- Relaciones con `HasForeignKey`, `OnDelete(DeleteBehavior.ClientSetNull)` para evitar cascadas automáticas.
- Tablas de unión `PlaylistVideos` y `VideoTags` configuradas con `UsingEntity` y clave compuesta.

### `Database/SqlServer/Entities/`
Entidades C# que representan las tablas de SQL Server:

| Entidad | Tabla SQL | Descripción |
|---|---|---|
| `UserAccount` | `UserAccount` | Usuario registrado. Navega a: Channels, Comments, Playlists, Subscriptions, VideoReactions, ViewHistories |
| `Channel` | `Channel` | Canal de un usuario. Navega a: User, Videos, Playlists, Subscriptions |
| `Video` | `Video` | Video de un canal. Navega a: Channel, VideoAccessibility, Comments, Tags (M:N), VideoReactions, ViewHistories, Subscriptions, Playlists (M:N) |
| `Comment` | `Comment` | Comentario en un video. Soporta auto-referencia para replies (`ParentCommentId` → `InverseParentComment`) |
| `Tag` | `Tag` | Etiqueta. Navega a Videos (M:N) |
| `VideoReaction` | `VideoReaction` | Reacción de un usuario a un video. Navega a: Video, User, ReactionType |
| `VideoAccessibility` | `VideoAccessibility` | Tipo de visibilidad. Navega a: Videos |
| `ReactionType` | `ReactionType` | Tipo de reacción (Like/Dislike). Navega a: VideoReactions |
| `Subscription` | `Subscription` | Suscripción de un usuario a un canal |
| `ViewHistory` | `ViewHistory` | Historial de vistas con `CompletionRate` |
| `Playlist` | `Playlist` | Playlist de usuario o canal. Navega a: Videos (M:N) |
| `CreatorType` | `CreatorType` | Tipo de creador de playlist |
| `LogEvent` | `LogEvents` | Tabla autogenerada por Serilog para almacenar logs |

### `Database/IUnitOfWork.cs`
Interfaz del patrón **Unit of Work**:
```csharp
public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
```
Abstrae el guardado de cambios del contexto EF, permitiendo que los repositorios no dependan directamente de `DbContext`.

### `Domain/Interfaces/Repositories/IUserRepository.cs`
Contrato del repositorio de usuarios:
- `GetByIdAsync(Guid id)` — obtiene un usuario por ID.
- `GetByUserNameAsync(string userName)` — busca por nombre de usuario.
- `CreateAsync(UserAccount user)` — crea un nuevo usuario.
- `UpdateAsync(UserAccount user)` — actualiza un usuario existente.

### `Domain/Exceptions/`
Excepciones de dominio personalizadas que son capturadas por el middleware de manejo de errores:

| Clase | Hereda de | HTTP Status |
|---|---|---|
| `BadRequestException` | `Exception` | 400 |
| `NotFoundException` | `Exception` | 404 |
| `UnauthorizedException` | `Exception` | 401 |

---

## 🗂️ Capa: `YoutubeClone.Infrastructure`

Implementaciones concretas de los contratos definidos en `Domain`. Depende exclusivamente de `Domain`.

### `Persistence/SqlServer/UnitOfWork.cs`
Implementa `IUnitOfWork`. Inyecta el `YoutubeCloneContext` y delega el `SaveChangesAsync()` al contexto de EF Core.

### `Persistence/SqlServer/Repositories/UserRepository.cs`
Implementa `IUserRepository` usando LINQ sobre el `YoutubeCloneContext`:

- `GetByIdAsync(Guid id)` — consulta `UserAccounts` filtrando por `UserId`, excluyendo usuarios con `DeletedAt` poblado (borrado lógico).
- `GetByUserNameAsync(string userName)` — busca por `UserName` ignorando mayúsculas/minúsculas, excluyendo borrados.
- `CreateAsync(UserAccount user)` — agrega la entidad al contexto y llama `SaveChangesAsync` vía `IUnitOfWork`.
- `UpdateAsync(UserAccount user)` — marca la entidad como modificada en el contexto y guarda los cambios.

---

## 🗂️ Capa: `YoutubeClone.Application`

Contiene la lógica de negocio, los contratos de servicios, los modelos de request/response y los helpers.

### `Interfaces/Services/`

#### `IUserService.cs`
Contrato del servicio de usuarios:
- `CreateFirstUser()` — crea el primer usuario del sistema si no existe (llamado al iniciar la app).
- `CreateUser(CreateUserRequest request)` — registra un nuevo usuario.
- `GetAllUsers(FilterUserRequest request)` — lista usuarios con filtrado.
- `UpdateUser(Guid id, UpdateUserRequest request)` — actualiza datos de un usuario.

#### `IAuthServices.cs`
Contrato del servicio de autenticación:
- `Login(LoginAuthRequest request)` — autentica un usuario y devuelve access token + refresh token.
- `RenewToken(RenewAuthRequest request)` — renueva el access token usando un refresh token válido.

#### `ICacheService.cs`
Contrato del servicio de caché:
- `Get<T>(string key)` — recupera un valor del caché.
- `Set<T>(string key, T value)` — guarda un valor en caché.
- `Remove(string key)` — elimina una entrada del caché.

---

### `Services/`

#### `UserService.cs`
Implementa `IUserService`. Orquesta la lógica de negocio de usuarios:

- **`CreateFirstUser()`** — comprueba si ya existe el usuario con `userName = "neider01"`. Si no existe, lo crea con la contraseña hasheada. Este método se llama al iniciar la aplicación desde `ServiceCollectionExtension.Initialize()`.

- **`CreateUser(CreateUserRequest request)`**:
  1. Valida control parental (`ParentalControlHelper`) — si el usuario es menor de 13 años, lanza `BadRequestException`.
  2. Verifica si el `UserName` ya está en uso mediante `IUserRepository.GetByUserNameAsync`.
  3. Hashea la contraseña con `PasswordHelper.Hash`.
  4. Crea la entidad `UserAccount` y la persiste via `IUserRepository.CreateAsync`.
  5. Invalida el caché de lista de usuarios.
  6. Devuelve un `GenericResponse` con `UserDto`.

- **`GetAllUsers(FilterUserRequest request)`**:
  1. Intenta recuperar la lista desde caché (`ICacheService.Get`).
  2. Si no está en caché, consulta todos los usuarios desde el repositorio.
  3. Aplica filtros opcionales: `UserName`, rango de fechas de creación.
  4. Almacena el resultado en caché.
  5. Devuelve la lista como `GenericResponse<List<UserDto>>`.

- **`UpdateUser(Guid id, UpdateUserRequest request)`**:
  1. Busca el usuario por ID; lanza `NotFoundException` si no existe.
  2. Actualiza únicamente los campos provistos en el request (patrón de actualización parcial).
  3. Si se cambia la contraseña, la hashea previamente.
  4. Persiste los cambios via `IUserRepository.UpdateAsync`.
  5. Invalida el caché.

#### `AuthService.cs`
Implementa `IAuthServices`. Maneja autenticación y renovación de tokens:

- **`Login(LoginAuthRequest request)`**:
  1. Busca al usuario por `UserName` mediante `IUserRepository.GetByUserNameAsync`.
  2. Verifica la contraseña usando `Hasher.Verify`.
  3. Si es válido, genera un access token JWT y un refresh token usando `TokenHelper`.
  4. Almacena el refresh token en caché asociado al `UserId`.
  5. Devuelve `LoginAuthResponse` con ambos tokens.

- **`RenewToken(RenewAuthRequest request)`**:
  1. Valida el refresh token desde caché.
  2. Si es válido y no expiró, genera un nuevo access token.
  3. Devuelve el nuevo access token.

#### `CacheService.cs`
Implementa `ICacheService` usando `IMemoryCache` de .NET:
- Wrappea las operaciones de `IMemoryCache` con una interfaz más simple y tipada.
- Las entradas se almacenan sin expiración explícita (configurables por cada llamada).

---

### `Helpers/`

#### `TokenHelper.cs`
Gestión completa de tokens JWT:
- `Configuration(IConfiguration config)` — lee `TokenConfiguration` de `appsettings.json` y construye un objeto `TokenConfiguration` con la `SecurityKey` derivada del secreto.
- `GenerateAccessToken(UserAccount user, TokenConfiguration config)` — crea un JWT firmado con claims: `UserId`, `UserName`, `Email`. Configura issuer, audience, expiración y firma con `HmacSha256`.
- `GenerateRefreshToken()` — delega en `Generate.GenerateRefreshToken()` para obtener un token seguro aleatorio.
- `ValidateRefreshToken(string token, ICacheService cache, Guid userId)` — verifica que el refresh token almacenado en caché coincida con el proporcionado y no haya expirado.

#### `CacheHelper.cs`
Helper con lógica de caché reutilizable:
- `GetOrSetAsync<T>(ICacheService cache, string key, Func<Task<T>> factory)` — implementa el patrón **cache-aside**: intenta leer del caché; si no existe, ejecuta la función factory, guarda el resultado y lo devuelve.

#### `ParentalControlHelper.cs`
Valida la edad del usuario en el registro:
- `IsMinor(DateTime birthday)` — devuelve `true` si el usuario tiene menos de 13 años según la fecha actual UTC.

#### `PasswordHelper.cs`
Wrapper sobre `Shared.Hasher`:
- `Hash(string password)` — hashea la contraseña.
- `Verify(string password, string hash)` — verifica la contraseña contra su hash.

#### `ResponsesHelper.cs`
Factory de respuestas estandarizadas:
- `Create<T>(T data, string message, List<string> errors)` — construye un objeto `GenericResponse<T>` con data, mensaje y lista de errores.
- `Create(string data, string message, List<string> errors)` — sobrecarga para respuestas de texto plano.

---

### `Models/`

#### `DTOs/UserDto.cs`
Objeto de transferencia de datos del usuario expuesto en las respuestas:
- `UserId`, `UserName`, `Email`, `DisplayName`, `Location`, `CreatedAt`

#### `DTOs/ChannelDto.cs`
DTO del canal expuesto en respuestas:
- `ChannelId`, `Handle`, `DisplayName`, `Description`, `AvatarUrl`, `BannerUrl`, `Verification`, `CreatedAt`

#### `Models/Responses/GenericResponse.cs`
Modelo de respuesta estándar para todos los endpoints:
```csharp
public class GenericResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
}
```

#### `Models/Responses/Auth/LoginAuthResponse.cs`
Respuesta del login:
- `AccessToken` — JWT de acceso.
- `RefreshToken` — token para renovar el acceso.

#### `Models/Requests/BaseRequest.cs`
Clase base de la que heredan algunos requests. Puede contener campos comunes de paginación o metadata.

#### `Models/Requests/Users/CreateUserRequest.cs`
Request para crear un usuario. Incluye validaciones con DataAnnotations:
- `UserName` — requerido, longitud mínima/máxima definida en `ValidationConstants`.
- `Email` — requerido, formato de email.
- `Password` — requerido, longitud mínima.
- `DisplayName`, `Birthday`, `Location` — requeridos.

#### `Models/Requests/Users/UpdateUserRequest.cs`
Request para actualizar un usuario. Todos los campos son opcionales (actualización parcial):
- `DisplayName`, `Location`, `Password` (opcionales).

#### `Models/Requests/Users/FilterUserRequest.cs`
Filtros para listar usuarios:
- `UserName` (opcional) — filtro por nombre de usuario.
- `CreatedFrom`, `CreatedTo` (opcionales) — rango de fechas de creación.

#### `Models/Requests/Auth/LoginAuthRequest.cs`
Request de login:
- `UserName` — requerido.
- `Password` — requerido.

#### `Models/Requests/Auth/RenewAuthRequest.cs`
Request para renovar token:
- `RefreshToken` — requerido.

#### `Models/Requests/Channels/CreateChannelRequest.cs`
Request para crear un canal:
- `Handle` — requerido, único, longitud máxima 20.
- `DisplayName` — requerido, longitud máxima 50.
- `Description` — opcional.
- `AvatarUrl`, `BannerUrl` — opcionales.

#### `Models/Requests/Channels/GetAllChannelRequest.cs`
Request para listar canales (puede incluir filtros futuros).

#### `Models/Helpers/TokenConfiguration.cs`
Modelo que almacena la configuración del token JWT:
- `Issuer`, `Audience`, `SecretKey`, `AccessTokenExpirationMinutes`, `RefreshTokenExpirationDays`, `SecurityKey` (derivada del secreto).

#### `Models/Helpers/RefreshToken.cs`
Modelo del refresh token almacenado en caché:
- `Token` — el valor del token.
- `ExpiresAt` — fecha de expiración.

#### `Models/Helpers/CacheKey.cs`
Modelo que encapsula la clave de caché de un recurso específico.

---

## 🗂️ Capa: `YoutubeClone.WebApp`

Capa de presentación. Expone los endpoints HTTP, configura el pipeline de ASP.NET Core y gestiona la inyección de dependencias.

### `Program.cs`
Punto de entrada de la aplicación:
```csharp
var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddCore(builder.Configuration); // Registra todos los servicios
var app = builder.Build();
app.UseMiddleware<ErrorHandleMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### `Extensions/ServiceCollectionExtension.cs`
Clase estática de extensión que centraliza el registro de todos los servicios. Contiene los siguientes métodos de extensión sobre `IServiceCollection`:

| Método | Descripción |
|---|---|
| `AddServices()` | Registra `ICacheService`, `IAuthServices`, `IUserService` como `Scoped` |
| `AddRepositories()` | Registra `IUnitOfWork`, `IUserRepository` como `Scoped` |
| `AddMiddlewares()` | Registra `ErrorHandleMiddleware` como `Scoped` |
| `AddLogging()` | Configura Serilog con 3 sinks: consola, archivo diario (`logs/log.txt`) y tabla `LogEvents` en SQL Server |
| `AddCache()` | Agrega `IMemoryCache` con `AddMemoryCache()` |
| `AddAuth(IConfiguration)` | Configura JWT Bearer con parámetros de validación (issuer, audience, firma, lifetime). Lanza `UnauthorizedException` en el evento `OnChallenge` |
| `Initialize()` | Construye un scope temporal, resuelve `IUserService` y llama `CreateFirstUser()` |
| `AddCore()` | Método principal que llama a todos los anteriores en orden: Controllers → OpenAPI → SqlServer → Repositories → Services → Middlewares → Logging → Cache → Auth → Initialize |

### `Controllers/UsersController.cs`
Controlador REST para la gestión de usuarios. Ruta base: `api/users`.

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| `POST` | `api/users` | ❌ Público | Crea un nuevo usuario |
| `GET` | `api/users` | ✅ JWT | Lista todos los usuarios (con filtros opcionales) |
| `PUT` | `api/users/{id}` | ✅ JWT | Actualiza un usuario por ID |

### `Controllers/AuthController.cs`
Controlador REST para autenticación. Ruta base: `api/auth`.

| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| `POST` | `api/auth/login` | ❌ Público | Autentica usuario y devuelve tokens |
| `POST` | `api/auth/renew` | ❌ Público | Renueva el access token con un refresh token |

### `Middlewares/ErrorHandleMiddleware.cs`
Middleware de manejo de errores centralizado. Intercepta todas las excepciones no controladas y devuelve respuestas JSON estandarizadas:

| Excepción | HTTP Status | Respuesta |
|---|---|---|
| `BadRequestException` | 400 | `GenericResponse` con el mensaje de la excepción |
| `NotFoundException` | 404 | `GenericResponse` con el mensaje de la excepción |
| `UnauthorizedException` | 401 | `GenericResponse` con el mensaje de la excepción |
| Cualquier otra | 500 | `GenericResponse` con mensaje genérico de error interno |

Adicionalmente, registra cada error con `Serilog` incluyendo el stack trace completo.

### `logs/`
Directorio donde Serilog escribe los archivos de log diarios con el formato `log{YYYYMMDD}.txt`. Los logs también se almacenan en la tabla `LogEvents` de SQL Server.

---

## 🔐 Flujo de autenticación

```
Cliente                    WebApp                   Application              Shared
  │                          │                           │                      │
  │── POST /auth/login ──►   │                           │                      │
  │                          │── Login(request) ──►      │                      │
  │                          │                           │── GetByUserName() ──►│
  │                          │                           │◄─ UserAccount ───────│
  │                          │                           │── Hasher.Verify() ───│
  │                          │                           │── GenerateJWT() ─────│
  │                          │                           │── GenerateRefresh() ─│
  │                          │                           │── CacheService.Set() │
  │◄── { accessToken,        │◄── LoginAuthResponse ─────│                      │
  │      refreshToken } ──── │                           │                      │
  │                          │                           │                      │
  │── GET /users (Bearer) ──►│                           │                      │
  │                          │── [JWT Validated] ────►   │                      │
  │◄── 200 OK ─────────────  │◄── List<UserDto> ─────────│                      │
```

---

## 🔄 Flujo de caché

El patrón **cache-aside** se aplica en la lectura de usuarios:

```
GetAllUsers()
  │
  ├── CacheService.Get("users-list")
  │     ├── HIT  → devuelve lista desde caché
  │     └── MISS → consulta repositorio
  │                 → CacheService.Set("users-list", lista)
  │                 → devuelve lista
  │
CreateUser() / UpdateUser()
  └── CacheService.Remove("users-list")  ← invalida caché
```

---

## 📋 Logging

Serilog está configurado con **3 destinos simultáneos**:

| Sink | Destino | Nivel |
|---|---|---|
| Consola | Salida estándar | Information |
| Archivo | `logs/log{fecha}.txt` (rotación diaria) | Information |
| SQL Server | Tabla `LogEvents` (autocreada) | Information |

Los errores capturados por `ErrorHandleMiddleware` se registran con nivel `Error` incluyendo el mensaje y el stack trace.

---

## 🚀 Ejecución

```bash
cd backend/with-database/YoutubeClone

# Restaurar dependencias
dotnet restore

# Ejecutar en modo desarrollo
dotnet run --project YoutubeClone.WebApp

# La API quedará disponible en:
# http://localhost:5000
# https://localhost:5001
```

> Al iniciar, la aplicación ejecuta `CreateFirstUser()` que crea automáticamente el usuario `neider01` si no existe en la base de datos.

---

## 📝 Notas adicionales

- El borrado de entidades es **lógico** (soft delete): se rellena `DeletedAt` en lugar de eliminar el registro.
- Los `GUID` se generan en base de datos mediante `NEWID()` (configurado en `OnModelCreating` con `HasDefaultValueSql`).
- Todas las fechas se almacenan en **UTC** (`SYSUTCDATETIME()`).
- El control parental bloquea el registro de usuarios menores de 13 años.
- La validación del modelo se realiza automáticamente por ASP.NET Core; los errores de validación son formateados por `InvalidModelStateResponseFactory` en `ServiceCollectionExtension.AddCore()`.
