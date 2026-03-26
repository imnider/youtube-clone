USE YoutubeClone;
GO

-- DEFAULT VALUES
INSERT INTO VideoAccessibility
	(DisplayName)
VALUES
	('Public'),
	('Private'),
	('Unlisted');
GO

INSERT INTO ReactionType
	(DisplayName)
VALUES
	('Like'),
	('Dislike');
GO

INSERT INTO CreatorType
	(DisplayName)
VALUES
	('UserAccount'),
	('Channel');
GO

-- VALUES
DECLARE @UserAccount01 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount02 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount03 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount04 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount05 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount06 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount07 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount08 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount09 UNIQUEIDENTIFIER = NEWID();
DECLARE @UserAccount10 UNIQUEIDENTIFIER = NEWID();

INSERT INTO UserAccount(
	UserID,
	UserName,
	Email,
	DisplayName,
	Birthday,
	Location,
	Password)
VALUES
	(@UserAccount01, 'neider01', 'neider01@gmail.com', 'Neider Vélez', '2000-05-10', 'Guayaquil', 'pass123'),
	(@UserAccount02, 'maria_dev', 'maria@gmail.com', 'María López', '1998-08-21', 'Quito', 'pass123'),
	(@UserAccount03, 'juan_code', 'juan@gmail.com', 'Juan Pérez', '1995-03-15', 'Lima', 'pass123'),
	(@UserAccount04, 'ana_tech', 'ana@gmail.com', 'Ana Torres', '2001-11-02', 'Bogotá', 'pass123'),
	(@UserAccount05, 'carlos99', 'carlos@gmail.com', 'Carlos Ruiz', '1997-07-30', 'México DF', 'pass123'),
	(@UserAccount06, 'dev_sara', 'sara@gmail.com', 'Sara Gómez', '1999-01-18', 'Medellín', 'pass123'),
	(@UserAccount07, 'luis_backend', 'luis@gmail.com', 'Luis Herrera', '1996-06-25', 'Santiago', 'pass123'),
	(@UserAccount08, 'frontend_ale', 'ale@gmail.com', 'Alejandro Díaz', '2002-09-12', 'Buenos Aires', 'pass123'),
	(@UserAccount09, 'paula_db', 'paula@gmail.com', 'Paula Castro', '1994-12-05', 'Panamá', 'pass123'),
	(@UserAccount10, 'andres_full', 'andres@gmail.com', 'Andrés Vega', '1993-04-28', 'San José', 'pass123');

DECLARE @Channel01 UNIQUEIDENTIFIER = NEWID();
DECLARE @Channel02 UNIQUEIDENTIFIER = NEWID();
DECLARE @Channel03 UNIQUEIDENTIFIER = NEWID();
DECLARE @Channel04 UNIQUEIDENTIFIER = NEWID();
DECLARE @Channel05 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Channel(
	ChannelID,
	UserID,
	Handle,
	DisplayName,
	Description)
VALUES
	(@Channel01, @UserAccount01, 'neiderdev', 'Neider Dev', 'Canal sobre desarrollo full stack y bases de datos'),
	(@Channel02, @UserAccount02, 'mariacode', 'Maria Code', 'Tutoriales de programación y tips de desarrollo'),
	(@Channel03, @UserAccount03, 'juantech', 'Juan Tech', 'Contenido sobre backend y arquitectura de software'),
	(@Channel04, @UserAccount04, 'anadev', 'Ana Dev', 'Frontend, UI/UX y diseño web moderno'),
	(@Channel05, @UserAccount05, 'carlosdev', 'Carlos Dev', 'Aprende bases de datos y SQL desde cero');

DECLARE @Video01 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video02 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video03 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video04 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video05 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video06 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video07 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video08 UNIQUEIDENTIFIER = NEWID();
DECLARE @Video09 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Video(
	VideoID,
	ChannelID,
	VideoAccessibilityID,
	Title,
	Description,
	DurationSeconds,
	ThumbnailURL,
	AgeRestriction,
	PublishedAt)
VALUES
	(@Video01, @Channel02, 1, 'Intro a SQL', 'Aprende lo básico de SQL', 300, 'thumb1.jpg', 0, '2024-01-10'),
	(@Video02, @Channel03, 1, 'Joins en SQL', 'Explicación de INNER JOIN', 600, 'thumb2.jpg', 0, '2024-02-15'),
	(@Video03, @Channel03, 1, 'LEFT JOIN', 'Cómo usar LEFT JOIN', 550, 'thumb3.jpg', 0, '2024-03-01'),
	(@Video04, @Channel04, 1, 'HTML básico', 'Estructura de una página web', 400, 'thumb4.jpg', 0, SYSUTCDATETIME()),
	(@Video05, @Channel04, 1, 'CSS Flexbox', 'Diseños modernos con Flexbox', 700, 'thumb5.jpg', 0, '2024-02-20'),
	(@Video06, @Channel04, 2, 'CSS Grid', 'Aprende CSS Grid', 800, 'thumb6.jpg', 0, '2024-03-10'),
	(@Video07, @Channel05, 1, 'Backend con .NET', 'API básica en .NET', 900, 'thumb7.jpg', 0, SYSUTCDATETIME()),
	(@Video08, @Channel05, 2, 'Entity Framework', 'ORM en .NET', 750, 'thumb8.jpg', 0, '2024-02-28'),
	(@Video09, @Channel05, 1, 'SQL Avanzado', 'Consultas complejas', 1000, 'thumb9.jpg', 1, '2023-12-15');

INSERT INTO Subscription(
	UserID,
	ChannelID,
	VideoID)
VALUES
	(@UserAccount06, @Channel01, NULL),
	(@UserAccount07, @Channel01, NULL),
	(@UserAccount08, @Channel01, NULL),
	(@UserAccount01, @Channel02, @Video01),
	(@UserAccount03, @Channel02, NULL),
	(@UserAccount01, @Channel03, @Video02),
	(@UserAccount02, @Channel03, @Video03),
	(@UserAccount04, @Channel03, NULL),
	(@UserAccount05, @Channel03, @Video02),
	(@UserAccount06, @Channel03, NULL),
	(@UserAccount02, @Channel05, @Video07),
	(@UserAccount03, @Channel05, NULL),
	(@UserAccount04, @Channel05, @Video08),
	(@UserAccount09, @Channel05, @Video09);

INSERT INTO ViewHistory(
	UserID,
	VideoID,
	CompletionRate)
VALUES
	(@UserAccount01, @Video01, 100),
	(@UserAccount02, @Video01, 75),
	(@UserAccount03, @Video01, 50),
	(@UserAccount04, @Video01, 90),
	(@UserAccount05, @Video01, 80),
	(@UserAccount06, @Video02, 60),
	(@UserAccount07, @Video02, 100),
	(@UserAccount08, @Video03, 70),
	(@UserAccount09, @Video03, 90),
	(@UserAccount10, @Video04, 100),
	(@UserAccount01, @Video04, 80),
	(@UserAccount02, @Video04, 60);

-- Modifico los datos actuales para la consulta de users que no han visto ningún video en 30 días
UPDATE ViewHistory
SET CreatedAt = '2026-01-01'
WHERE UserID = 'C9237A7B-9A1D-4515-B4F7-A4B5B707DA91'; --poner ID de algún usuario

SELECT * FROM UserAccount --seleccionarlo aquí (ID)

DECLARE @Tag01 UNIQUEIDENTIFIER = NEWID();
DECLARE @Tag02 UNIQUEIDENTIFIER = NEWID();
DECLARE @Tag03 UNIQUEIDENTIFIER = NEWID();
DECLARE @Tag04 UNIQUEIDENTIFIER = NEWID();
DECLARE @Tag05 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Tag(
	TagID,
	DisplayName)
VALUES
	(@Tag01, 'SQL'),
	(@Tag02, 'Backend'),
	(@Tag03, 'Frontend'),
	(@Tag04, 'CSS'),
	(@Tag05, 'FullStack');

INSERT INTO VideoTags(
	VideoID,
	TagID)
VALUES
	-- Video01: Intro a SQL
	(@Video01, @Tag01),       -- SQL
	(@Video01, @Tag05),       -- FullStack

	-- Video02: Joins en SQL
	(@Video02, @Tag01),       -- SQL
	(@Video02, @Tag02),       -- Backend

	-- Video03: LEFT JOIN
	(@Video03, @Tag01),       -- SQL
	(@Video03, @Tag02),       -- Backend

	-- Video04: HTML básico
	(@Video04, @Tag03),       -- Frontend
	(@Video04, @Tag05),       -- FullStack

	-- Video05: CSS Flexbox
	(@Video05, @Tag03),       -- Frontend
	(@Video05, @Tag04),       -- CSS

	-- Video06: CSS Grid
	(@Video06, @Tag03),       -- Frontend
	(@Video06, @Tag04),       -- CSS

	-- Video07: Backend con .NET
	(@Video07, @Tag02),       -- Backend
	(@Video07, @Tag05),       -- FullStack

	-- Video08: Entity Framework
	(@Video08, @Tag02),       -- Backend
	(@Video08, @Tag05),       -- FullStack

	-- Video09: SQL Avanzado
	(@Video09, @Tag01),       -- SQL
	(@Video09, @Tag02),       -- Backend
	(@Video09, @Tag05);       -- FullStack

INSERT INTO VideoReaction(
	VideoID,
	UserID,
	ReactionTypeID)
VALUES
	(@Video01, @UserAccount01, 1),
	(@Video01, @UserAccount02, 2),
	(@Video01, @UserAccount03, 1),
	(@Video02, @UserAccount04, 1),
	(@Video02, @UserAccount05, 1),
	(@Video03, @UserAccount06, 1),
	(@Video03, @UserAccount07, 2),
	(@Video03, @UserAccount08, 1),
	(@Video03, @UserAccount09, 1),
	(@Video04, @UserAccount01, 1),
	(@Video04, @UserAccount10, 2),
	(@Video05, @UserAccount02, 1),
	(@Video05, @UserAccount03, 1),
	(@Video05, @UserAccount04, 2);

DECLARE @Comment01 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment02 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment03 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment04 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment05 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment06 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment07 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment08 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment09 UNIQUEIDENTIFIER = NEWID();
DECLARE @Comment10 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Comment(
	CommentID,
	VideoID,
	UserID,
	Content,
	IsPinned,
	ParentCommentID)
VALUES
	-- Comentarios principales
	(@Comment01, @Video01, @UserAccount01, 'Excelente introducción a SQL!', 1, NULL),
	(@Comment02, @Video01, @UserAccount02, 'Muy útil, gracias!', 0, NULL),
	(@Comment03, @Video01, @UserAccount03, 'Pregunta: ¿puedo usar esto en MySQL?', 0, NULL),

	-- Respuesta al comentario de User03
	(@Comment04, @Video01, @UserAccount01, 'Sí, la sintaxis es muy similar.', 0, @Comment03),

	-- Comentarios en Video02
	(@Comment05, @Video02, @UserAccount04, 'Me costó entender los joins, pero este video ayuda.', 0, NULL),
	(@Comment06, @Video02, @UserAccount05, 'Gracias por la explicación!', 0, NULL),

	-- Comentarios en Video04
	(@Comment07, @Video04, @UserAccount06, 'Buen repaso de HTML básico', 0, NULL),
	(@Comment08, @Video04, @UserAccount07, 'Me gustó la parte del DOCTYPE', 0, NULL),

	-- Comentarios en Video05
	(@Comment09, @Video05, @UserAccount02, 'CSS Flexbox explicado de manera clara', 1, NULL),
	(@Comment10, @Video05, @UserAccount03, 'Gracias, justo lo que necesitaba!', 0, @Comment09);

DECLARE @Playlist01 UNIQUEIDENTIFIER = NEWID();
DECLARE @Playlist02 UNIQUEIDENTIFIER = NEWID();
DECLARE @Playlist03 UNIQUEIDENTIFIER = NEWID();
DECLARE @Playlist04 UNIQUEIDENTIFIER = NEWID();
DECLARE @Playlist05 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Playlist(
	PlaylistID,
	CreatorTypeID,
	UserID,
	ChannelID)
VALUES
	-- Playlists de usuario
	(@Playlist01, 1, @UserAccount01, NULL),
	(@Playlist02, 1, @UserAccount02, NULL),

	-- Playlists de canal
	(@Playlist03, 2, @UserAccount01, @Channel01),
	(@Playlist04, 2, @UserAccount02, @Channel02),
	(@Playlist05, 2, @UserAccount03, @Channel03);

INSERT INTO PlaylistVideos(
	PlaylistID,
	VideoID)
VALUES
	-- Playlist01 (usuario)
	(@Playlist01, @Video01),
	(@Playlist01, @Video02),
	(@Playlist01, @Video03),

	-- Playlist02 (usuario)
	(@Playlist02, @Video04),
	(@Playlist02, @Video05),

	-- Playlist03 (canal)
	(@Playlist03, @Video01),
	(@Playlist03, @Video04),

	-- Playlist04 (canal)
	(@Playlist04, @Video02),
	(@Playlist04, @Video05),

	-- Playlist05 (canal)
	(@Playlist05, @Video03),
	(@Playlist05, @Video06);
GO

UPDATE Channel
SET ChannelID = 'B85F26E6-B7B1-43D3-895F-F8DE4DBCB90F'
WHERE Handle = 'juantech'
GO

UPDATE UserAccount
SET UserID = 'C9237A7B-9A1D-4515-B4F7-A4B5B707DA91'
WHERE UserName = 'neider01'
GO

UPDATE Video
SET VideoID = '06C251F7-C35D-4351-ABF4-5BA5EA5A2553'
WHERE Title = 'Intro a SQL'
GO