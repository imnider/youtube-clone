# 🗄️ Database — YoutubeClone

Este directorio contiene todos los scripts SQL necesarios para crear y poblar la base de datos del proyecto YoutubeClone en **SQL Server**.

---

## 📁 Archivos

| Archivo | Descripción |
|---|---|
| `ddl.sql` | Data Definition Language — crea la base de datos y todas las tablas |
| `dml.sql` | Data Manipulation Language — inserta datos de prueba |
| `queries.sql` | Consultas de ejemplo para validar el esquema |

---

## ⚙️ Requisitos

- SQL Server 2019 o superior (o Docker con la imagen `mcr.microsoft.com/mssql/server:2022-latest`)
- SQL Server Management Studio, Azure Data Studio o cualquier cliente SQL compatible

---

## 🚀 Instalación

Ejecutar los scripts **en este orden**:

```sql
-- 1. Crear estructura
-- Ejecutar: ddl.sql

-- 2. Insertar datos de prueba
-- Ejecutar: dml.sql
```

> **Nota:** `ddl.sql` ya incluye `USE master` y `CREATE DATABASE YoutubeClone`, por lo que no es necesario crear la base de datos manualmente.

---

## 🧱 Esquema de la base de datos

### Tablas principales

#### `UserAccount`
Representa a los usuarios registrados en la plataforma.

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `UserID` | `UNIQUEIDENTIFIER` | PK, DEFAULT NEWID() | Identificador único del usuario |
| `UserName` | `NVARCHAR(20)` | NOT NULL, UNIQUE | Nombre de usuario único |
| `Email` | `NVARCHAR(255)` | NOT NULL, UNIQUE | Correo electrónico único |
| `DisplayName` | `NVARCHAR(50)` | NOT NULL | Nombre público visible |
| `Birthday` | `DATETIME2` | NOT NULL | Fecha de nacimiento |
| `Location` | `NVARCHAR(30)` | NOT NULL | País o ciudad del usuario |
| `Password` | `NVARCHAR(255)` | NOT NULL | Contraseña (hash) |
| `CreatedAt` | `DATETIME2` | DEFAULT SYSUTCDATETIME() | Fecha de creación |
| `UpdatedAt` | `DATETIME2` | NULL | Última actualización |
| `DeletedAt` | `DATETIME2` | NULL | Fecha de borrado lógico |

---

#### `Channel`
Canales creados por los usuarios. Un usuario puede tener múltiples canales.

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `ChannelID` | `UNIQUEIDENTIFIER` | PK, DEFAULT NEWID() | Identificador único del canal |
| `UserID` | `UNIQUEIDENTIFIER` | FK → UserAccount | Propietario del canal |
| `Handle` | `NVARCHAR(20)` | NOT NULL, UNIQUE | Handle público del canal (ej. `@neiderdev`) |
| `DisplayName` | `NVARCHAR(50)` | NOT NULL | Nombre visible del canal |
| `Verification` | `BIT` | DEFAULT 0 | Si el canal está verificado |
| `Description` | `NVARCHAR(255)` | NULL | Descripción del canal |
| `AvatarURL` | `NVARCHAR(255)` | NULL | URL del avatar |
| `BannerURL` | `NVARCHAR(255)` | NULL | URL del banner |
| `CreatedAt` | `DATETIME2` | DEFAULT SYSUTCDATETIME() | Fecha de creación |
| `UpdatedAt` | `DATETIME2` | NULL | Última actualización |
| `DeletedAt` | `DATETIME2` | NULL | Borrado lógico |

---

#### `VideoAccessibility`
Catálogo de tipos de visibilidad de un video.

| Columna | Tipo | Descripción |
|---|---|---|
| `VideoAccessibilityID` | `INT IDENTITY(1,1)` PK | ID autoincremental |
| `DisplayName` | `NVARCHAR(30)` | Nombre del tipo: `Public`, `Private`, `Unlisted` |
| `CreatedAt` | `DATETIME2` | Fecha de creación |

Valores por defecto insertados: `Public (1)`, `Private (2)`, `Unlisted (3)`.

---

#### `Video`
Videos subidos a los canales.

| Columna | Tipo | Restricciones | Descripción |
|---|---|---|---|
| `VideoID` | `UNIQUEIDENTIFIER` | PK, DEFAULT NEWID() | Identificador único del video |
| `ChannelID` | `UNIQUEIDENTIFIER` | FK → Channel | Canal propietario |
| `VideoAccessibilityID` | `INT` | FK → VideoAccessibility | Visibilidad del video |
| `Title` | `NVARCHAR(255)` | NOT NULL | Título del video |
| `Description` | `NVARCHAR(255)` | NULL | Descripción |
| `DurationSeconds` | `INT` | NOT NULL | Duración en segundos |
| `ThumbnailURL` | `NVARCHAR(255)` | NOT NULL | URL de la miniatura |
| `AgeRestriction` | `BIT` | DEFAULT 0 | Restricción de edad |
| `PublishedAt` | `DATETIME2` | DEFAULT SYSUTCDATETIME() | Fecha de publicación |
| `CreatedAt` | `DATETIME2` | DEFAULT SYSUTCDATETIME() | Fecha de creación |
| `UpdatedAt` | `DATETIME2` | NULL | Última actualización |
| `DeletedAt` | `DATETIME2` | NULL | Borrado lógico |

---

#### `Tag`
Etiquetas que pueden asociarse a videos.

| Columna | Tipo | Descripción |
|---|---|---|
| `TagID` | `UNIQUEIDENTIFIER` PK | Identificador único |
| `DisplayName` | `NVARCHAR(20)` NOT NULL | Nombre de la etiqueta |

---

#### `VideoTags` *(tabla de unión)*
Relación muchos a muchos entre `Video` y `Tag`.

| Columna | Tipo | Descripción |
|---|---|---|
| `VideoID` | `UNIQUEIDENTIFIER` FK → Video | |
| `TagID` | `UNIQUEIDENTIFIER` FK → Tag | |
| **PK compuesta** | (`VideoID`, `TagID`) | |

---

#### `ReactionType`
Catálogo de tipos de reacción (Like / Dislike).

| Columna | Tipo | Descripción |
|---|---|---|
| `ReactionTypeID` | `INT IDENTITY(1,1)` PK | |
| `DisplayName` | `NVARCHAR(20)` | `Like (1)` o `Dislike (2)` |
| `CreatedAt` | `DATETIME2` | |

---

#### `VideoReaction`
Registro de reacciones de usuarios a videos.

| Columna | Tipo | Descripción |
|---|---|---|
| `VideoReactionID` | `UNIQUEIDENTIFIER` PK | |
| `VideoID` | FK → Video | |
| `UserID` | FK → UserAccount | |
| `ReactionTypeID` | FK → ReactionType | |
| `CreatedAt` | `DATETIME2` | |

---

#### `Comment`
Comentarios en videos, con soporte para respuestas anidadas.

| Columna | Tipo | Descripción |
|---|---|---|
| `CommentID` | `UNIQUEIDENTIFIER` PK | |
| `VideoID` | FK → Video | Video al que pertenece |
| `UserID` | FK → UserAccount | Autor del comentario |
| `Content` | `NVARCHAR(255)` NOT NULL | Texto del comentario |
| `IsPinned` | `BIT` DEFAULT 0 | Si está fijado por el canal |
| `ParentCommentID` | FK → Comment (nullable) | Para respuestas anidadas |
| `CreatedAt` | `DATETIME2` | |
| `UpdatedAt` | `DATETIME2` NULL | |
| `DeletedAt` | `DATETIME2` NULL | Borrado lógico |

---

#### `Subscription`
Suscripciones de usuarios a canales, opcionalmente asociadas a un video específico.

| Columna | Tipo | Descripción |
|---|---|---|
| `SubscriptionID` | `UNIQUEIDENTIFIER` PK | |
| `UserID` | FK → UserAccount | Usuario suscrito |
| `ChannelID` | FK → Channel | Canal suscrito |
| `VideoID` | FK → Video (nullable) | Video desde el que se suscribió |
| `CreatedAt` | `DATETIME2` | |
| `DeletedAt` | `DATETIME2` NULL | |

---

#### `ViewHistory`
Historial de reproducción de un usuario en un video.

| Columna | Tipo | Descripción |
|---|---|---|
| `ViewHistoryID` | `UNIQUEIDENTIFIER` PK | |
| `UserID` | FK → UserAccount | |
| `VideoID` | FK → Video | |
| `CompletionRate` | `DECIMAL` NOT NULL DEFAULT 0.0 | Porcentaje visto (0–100) |
| `CreatedAt` | `DATETIME2` | Fecha de la vista |

---

#### `CreatorType`
Catálogo que indica si una playlist fue creada por un usuario o por un canal.

| Columna | Tipo | Descripción |
|---|---|---|
| `CreatorTypeID` | `INT IDENTITY(1,1)` PK | |
| `DisplayName` | `NVARCHAR(30)` | `UserAccount (1)` o `Channel (2)` |

---

#### `Playlist`
Listas de reproducción, que pueden ser creadas por usuarios o canales.

| Columna | Tipo | Descripción |
|---|---|---|
| `PlaylistID` | `UNIQUEIDENTIFIER` PK | |
| `CreatorTypeID` | FK → CreatorType | Tipo de creador |
| `UserID` | FK → UserAccount | Propietario (si es de usuario) |
| `ChannelID` | FK → Channel (nullable) | Canal propietario (si aplica) |
| `CreatedAt` | `DATETIME2` | |
| `UpdatedAt` | `DATETIME2` NULL | |
| `DeletedAt` | `DATETIME2` NULL | |

---

#### `PlaylistVideos` *(tabla de unión)*
Relación muchos a muchos entre `Playlist` y `Video`.

| Columna | Tipo | Descripción |
|---|---|---|
| `PlaylistID` | FK → Playlist | |
| `VideoID` | FK → Video | |
| **PK compuesta** | (`PlaylistID`, `VideoID`) | |

---

## 🔗 Diagrama de relaciones (resumen)

```
UserAccount ──< Channel ──< Video >── VideoTags >── Tag
     │               │         │
     │               │         └──< VideoReaction >── ReactionType
     │               │         └──< Comment (anidados)
     │               │         └──< ViewHistory
     │               │         └──< PlaylistVideos >── Playlist
     │               └──< Subscription
     └──< Subscription
     └──< Playlist (tipo usuario)
```

---

## 📊 Datos de prueba (`dml.sql`)

El script de datos de prueba inserta:

- **10 usuarios** (`neider01`, `maria_dev`, `juan_code`, `ana_tech`, `carlos99`, `dev_sara`, `luis_backend`, `frontend_ale`, `paula_db`, `andres_full`)
- **5 canales** (`neiderdev`, `mariacode`, `juantech`, `anadev`, `carlosdev`)
- **9 videos** sobre temáticas de SQL, HTML, CSS y .NET
- **5 tags** (`SQL`, `Backend`, `Frontend`, `CSS`, `FullStack`)
- **14 suscripciones**, **12 vistas**, **14 reacciones**, **10 comentarios** (con respuestas anidadas)
- **5 playlists** (de usuario y de canal)

> **Nota:** Algunos IDs clave se actualizan con valores fijos para facilitar las pruebas en `queries.sql`.

---

## 🔍 Consultas de ejemplo (`queries.sql`)

El archivo incluye consultas para:

- Listar videos con sus canales y visibilidad
- Obtener el historial de un usuario específico
- Contar reacciones por video
- Buscar usuarios que no han visto ningún video en los últimos 30 días
- Listar comentarios con sus respuestas anidadas
- Obtener playlists con sus videos
