# 🎬 YouTube Clone

Clon funcional de YouTube desarrollado como proyecto final del **Bootcamp Full Stack Developer | Impulsa tu futuro**, en colaboración entre [Neider Vélez](https://github.com/imnider) y [Juliet Morales](https://github.com/julsmagical).

El proyecto implementa una API REST con autenticación JWT, base de datos SQL Server y una arquitectura limpia en capas (Clean Architecture), con dos variantes del backend: una con caché en memoria y otra integrada completamente con base de datos.

---

## 👥 Colaboradores

| Nombre | GitHub |
|---|---|
| Neider Vélez | [@imnider](https://github.com/imnider) |
| Juliet Morales | [@julsmagical](https://github.com/julsmagical) |

---

## 📁 Estructura del repositorio

```
youtube-clone/
├── backend/
│   ├── with-cache/          # Backend con caché en memoria (sin base de datos real)
│   │   └── YoutubeClone/
│   │       ├── YoutubeClone.Application/
│   │       ├── YoutubeClone.Shared/
│   │       └── YoutubeClone.WebApp/
│   ├── with-database/       # Backend completo con SQL Server, JWT y Serilog
│   │   └── YoutubeClone/
│   │       ├── YoutubeClone.Application/
│   │       ├── YoutubeClone.Domain/
│   │       ├── YoutubeClone.Infrastructure/
│   │       ├── YoutubeClone.Shared/
│   │       └── YoutubeClone.WebApp/
│   └── README.md
├── database/
│   ├── ddl.sql              # Definición de tablas (estructura)
│   ├── dml.sql              # Datos de prueba
│   ├── queries.sql          # Consultas de ejemplo
│   └── README.md
├── docker/
│   └── docker-compose.yml   # Configuración Docker para SQL Server
└── README.md
```

---

## 🛠️ Stack tecnológico

| Capa | Tecnología |
|---|---|
| Lenguaje backend | C# (.NET 9) |
| Base de datos | SQL Server (T-SQL) |
| ORM | Entity Framework Core |
| Autenticación | JWT Bearer Tokens |
| Logging | Serilog (consola, archivo y base de datos) |
| Caché | IMemoryCache (.NET) |
| Contenedores | Docker / Docker Compose |

---

## ⚙️ Requisitos previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local o en Docker)
- Un IDE compatible con .NET (Visual Studio, Rider, VS Code)
- Docker (opcional, para levantar SQL Server automáticamente)

---

## 🚀 Inicio rápido

### Opción A — Con Docker (recomendado)

```bash
# 1. Levantar SQL Server con Docker
cd docker
docker compose up -d

# 2. Ejecutar los scripts de base de datos
# Abrir SQL Server Management Studio o Azure Data Studio
# Ejecutar en orden: ddl.sql → dml.sql

# 3. Levantar el backend
cd backend/with-database/YoutubeClone
dotnet run --project YoutubeClone.WebApp
```

### Opción B — SQL Server local

```bash
# 1. Ejecutar en orden: database/ddl.sql → database/dml.sql

# 2. Ajustar la cadena de conexión en:
# backend/with-database/YoutubeClone/YoutubeClone.WebApp/appsettings.json

# 3. Ejecutar la aplicación
cd backend/with-database/YoutubeClone
dotnet run --project YoutubeClone.WebApp
```

---

## 🔧 Variantes del backend

### `backend/with-cache`
Versión inicial del proyecto. Usa un caché en memoria (`Dictionary<string, object>`) en lugar de una base de datos real. Implementa solo el módulo de usuarios y canales, sin autenticación JWT completa.

### `backend/with-database`
Versión completa y actual. Integra SQL Server mediante Entity Framework Core, autenticación JWT con refresh tokens, manejo de errores centralizado con middleware, logging con Serilog y arquitectura limpia en 4 capas.

> 📖 Ver la documentación detallada en [`backend/README.md`](./backend/README.md)

---

## 🗄️ Base de datos

El esquema modela las entidades principales de YouTube: usuarios, canales, videos, suscripciones, comentarios, reacciones, playlists, historial de vistas y etiquetas.

> 📖 Ver la documentación detallada en [`database/README.md`](./database/README.md)

---

## 🐳 Docker

El archivo `docker/docker-compose.yml` levanta una instancia de SQL Server 2022 lista para usar:

```bash
cd docker
docker compose up -d
```

Credenciales por defecto:
- **Host:** `localhost,1433`
- **Usuario:** `sa`
- **Contraseña:** `Admin1234@`
- **Base de datos:** `YoutubeClone`

---

## 📐 Diseño del sistema

Diagrama de arquitectura y modelo de datos disponible en Excalidraw:  
🔗 [Ver diagrama](https://excalidraw.com/#json=Ac39waOTlC1vGvEelTVgl,3o7MYImjDWIB5MEH8V0ABQ)

---

## ⚠️ Disclaimer

Todos los derechos del nombre, marca e identidad visual de YouTube pertenecen a **Google LLC**. Este proyecto es únicamente con fines educativos.
