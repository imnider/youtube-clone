# Backend
Realicé dos versiones para el Backend, una con almacenamiento en Caché y otra con conexión a Base de Datos SqlServer.
## Conexión con Base de Datos
La base de datos utilizada fue creada para SqlServer.
### Scaffolding
```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;User=sa;Password=Admin1234@;Database=YoutubeClone;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project YoutubeClone.Domain --startup-project YoutubeClone.WebApp --context-dir Database/SqlServer/Context --output-dir Database/SqlServer/Entities --nobuild --force
```