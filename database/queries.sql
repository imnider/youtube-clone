USE YoutubeClone;
GO

-- Todos los canales y sus usuarios
SELECT
	c.DisplayName As ChannelName,
	c.ChannelID,
	ua.UserName
FROM Channel c
INNER JOIN UserAccount ua
	ON c.UserID = ua.UserID
GO

-- Todos los videos con sus visualizaciones
SELECT
	v.Title,
	COUNT(vh.ViewHistoryID) AS Views
FROM ViewHistory vh
INNER JOIN Video v
	ON vh.VideoID = v.VideoID
GROUP BY v.Title
GO

-- Videos de un canal ordenados por visualizaciones
DECLARE @Channel UNIQUEIDENTIFIER = 'B85F26E6-B7B1-43D3-895F-F8DE4DBCB90F';

SELECT
	c.DisplayName AS ChannelName,
	v.Title,
	Count(vh.ViewHistoryID) As Views
FROM ViewHistory vh
INNER JOIN Video v
	ON vh.VideoID = v.VideoID
INNER JOIN Channel c
	ON c.ChannelID = v.ChannelID
WHERE c.ChannelID = @Channel
GROUP BY v.Title, c.DisplayName
ORDER BY Views
GO

-- Canales con más suscriptores
SELECT 
	c.DisplayName As ChannelName,
	COUNT(*) AS SubscriptionCount
FROM Subscription s
INNER JOIN Channel c
	ON s.ChannelID = c.ChannelID
GROUP BY
	c.DisplayName
ORDER BY
	SubscriptionCount DESC
GO

-- Lista de todos los usuarios
SELECT
	DisplayName,
	UserID
FROM UserAccount
GO

-- Lista de todos las visualizaciones con porcentaje de visto
SELECT
	v.Title,
	ua.DisplayName,
	CompletionRate
FROM ViewHistory vh
INNER JOIN Video v
	ON vh.VideoID = v.VideoID
INNER JOIN UserAccount ua
	ON vh.UserID = ua.UserID
ORDER BY ua.DisplayName
GO

-- Videos completados por un usuario (porcentaje >= 90%)
DECLARE @User UNIQUEIDENTIFIER = 'C9237A7B-9A1D-4515-B4F7-A4B5B707DA91';

SELECT
	v.Title,
	ua.DisplayName,
	CompletionRate
FROM ViewHistory vh
INNER JOIN Video v
	ON vh.VideoID = v.VideoID
INNER JOIN UserAccount ua
	ON vh.UserID = ua.UserID
WHERE vh.CompletionRate>=90.0 AND vh.UserID = @USER
ORDER BY vh.CompletionRate
GO

-- Todos los comentarios con nombre del autor, fecha, y video donde se realizaron
SELECT
	v.Title,
	v.VideoID,
	c.Content,
	ua.DisplayName,
	c.CreatedAt
FROM Comment c
INNER JOIN Video v
	ON c.VideoID = v.VideoID
INNER JOIN UserAccount ua
	ON c.UserID = ua.UserID
GO

-- Comentarios de un video con el nombre del autor y su fecha
DECLARE @Video UNIQUEIDENTIFIER = '06C251F7-C35D-4351-ABF4-5BA5EA5A2553';
DECLARE @User NVARCHAR(30) = 'Neider Vélez';
DECLARE @Date DATETIME2 = '2026-03-18';

SELECT
	v.Title,
	c.Content,
	ua.DisplayName,
	c.CreatedAt
FROM Comment c
INNER JOIN Video v
	ON c.VideoID = v.VideoID
INNER JOIN UserAccount ua
	ON c.UserID = ua.UserID
WHERE
	v.VideoID = @Video AND ua.DisplayName = @User AND CAST(c.CreatedAt AS DATE) = @Date
GO

-- Videos con más likes que dislikes
SELECT
	v.Title,
	SUM(CASE WHEN ReactionTypeID = 1 THEN 1 ELSE 0 END) AS Likes,
    SUM(CASE WHEN ReactionTypeID = 2 THEN 1 ELSE 0 END) AS Dislikes
FROM VideoReaction vr
INNER JOIN Video v
	ON vr.VideoID = v.VideoID
GROUP BY v.Title
HAVING
	-- No pude usar los alias porque el SELECT se ejecuta después :(
	SUM(CASE WHEN ReactionTypeID = 1 THEN 1 ELSE 0 END) > SUM(CASE WHEN ReactionTypeID = 2 THEN 1 ELSE 0 END)
GO

-- Suscripciones de un usuario con el último video publicado de cada canal
DECLARE @User UNIQUEIDENTIFIER = 'C9237A7B-9A1D-4515-B4F7-A4B5B707DA91';

SELECT
    c.DisplayName AS ChannelName,
    v.Title AS LatestTitle,
    v.PublishedAt
FROM Subscription sub
INNER JOIN Channel c
    ON sub.ChannelID = c.ChannelID
INNER JOIN Video v
    ON v.ChannelID = c.ChannelID
INNER JOIN (
    -- fecha mas reciente de cada canal
    SELECT ChannelID, MAX(PublishedAt) AS LatestPublishedAt
    FROM Video
    GROUP BY ChannelID
) vmax -- subconsulta para devolver lo mas reciente de la publicacion
    ON v.ChannelID = vmax.ChannelID
   AND v.PublishedAt = vmax.LatestPublishedAt
WHERE sub.UserID = @User
ORDER BY v.PublishedAt DESC;
GO

-- Top 3 etiquetas con más videos
SELECT TOP 3 
	tg.DisplayName AS TagName, 
	COUNT(vts.VideoID) AS VideoCount --columnas que se mostraran
FROM VideoTags vts
INNER JOIN Tag tg
	ON vts.TagID = tg.TagID --columna en común
GROUP BY tg.DisplayName
ORDER BY VideoCount DESC;
GO

-- Usuarios que no han visto ningún video en los últimos 30 días
SELECT 
	uacc.UserID,
	uacc.DisplayName, 
	uacc.UserName
FROM UserAccount uacc
LEFT JOIN ViewHistory vh
    ON vh.UserID = uacc.UserID
	--agregue dateadd por ser compatible con datetime2 para restar esos 30 dias
    AND vh.CreatedAt >= DATEADD(DAY, -30, SYSUTCDATETIME()) 
WHERE vh.UserID IS NULL
ORDER BY uacc.DisplayName;
GO

-- Duración total en horas del contenido de un canal
SELECT 
	c.DisplayName AS ChannelName, 
	COUNT(vid.VideoID) AS VideoCount,
	SUM(vid.DurationSeconds) AS TotalSeconds,
	--como el atributo esta en seg, lo dividimos para los seg que tiene una hora
	SUM(vid.DurationSeconds)/3600.0 AS TotalHours 
	--3600.0 para que no salga todo 0
FROM Channel c
INNER JOIN Video vid
	ON c.ChannelID = vid.ChannelID
WHERE vid.DeletedAt IS NULL
GROUP BY c.DisplayName
ORDER BY TotalSeconds DESC;
GO

-- Videos de canales suscritos ordenados por fecha para el feed de un usuario
DECLARE @User UNIQUEIDENTIFIER = 'C9237A7B-9A1D-4515-B4F7-A4B5B707DA91';

SELECT
    c.DisplayName AS ChannelName,
    v.Title,
    v.PublishedAt
FROM Subscription sub
INNER JOIN Channel c
    ON sub.ChannelID = c.ChannelID
INNER JOIN Video v
    ON v.ChannelID = c.ChannelID
INNER JOIN VideoAccessibility va
    ON v.VideoAccessibilityID = va.VideoAccessibilityID
WHERE sub.UserID = @User
  AND v.DeletedAt IS NULL
  AND va.DisplayName = 'Public'
ORDER BY v.PublishedAt DESC;
GO