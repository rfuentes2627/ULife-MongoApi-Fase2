# U-Life MongoDB API - Fase 2

## Descripción
Este proyecto corresponde a la fase 2 del proyecto de cátedra de Base de Datos II.
Se implementó una API REST con ASP.NET Core y MongoDB para la plataforma **U-Life**, una red social académica para estudiantes universitarios.

## Tecnologías utilizadas
- ASP.NET Core Web API
- .NET 10
- MongoDB
- MongoDB.Driver
- Swagger / OpenAPI
- Docker
- Percona Backup for MongoDB (PBM) como herramienta propuesta para backups automáticos

## Colecciones implementadas
- users
- posts
- comments

## Configuración
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "ulife_db",
    "UsersCollectionName": "users",
    "PostsCollectionName": "posts",
    "CommentsCollectionName": "comments"
  }
}
```

## Cómo ejecutar el proyecto
### 1. Levantar MongoDB
```bash
docker run -d --name mongo-ulife -p 27017:27017 mongo
```

### 2. Restaurar paquetes y compilar
```bash
dotnet restore
dotnet build
```

### 3. Ejecutar la API
```bash
dotnet run
```

### 4. Abrir Swagger
```text
http://localhost:5104/swagger
```

## Endpoints disponibles

### Users
- GET /api/Users
- POST /api/Users
- GET /api/Users/{id}
- PUT /api/Users/{id}
- DELETE /api/Users/{id}

**Filtros:** `university`, `career`, `username`  
**Paginación:** `page`, `limit`  
**Orden:** `sortBy`, `sortOrder`

Ejemplo:
```text
GET /api/Users?page=1&limit=10&university=Universidad Evangelica de El Salvador
```

### Posts
- GET /api/Posts
- POST /api/Posts
- GET /api/Posts/{id}
- PUT /api/Posts/{id}
- DELETE /api/Posts/{id}

**Filtros:** `type`, `category`, `tag`, `authorId`, `visibility`  
**Paginación:** `page`, `limit`  
**Orden:** `sortBy`, `sortOrder`

Ejemplo:
```text
GET /api/Posts?page=1&limit=5&type=article&category=academic&tag=mongodb
```

### Comments
- GET /api/Comments
- POST /api/Comments
- GET /api/Comments/{id}
- PUT /api/Comments/{id}
- DELETE /api/Comments/{id}

**Filtros:** `postId`, `parentCommentId`  
**Paginación:** `page`, `limit`  
**Orden:** `sortBy`, `sortOrder`

Ejemplo:
```text
GET /api/Comments?page=1&limit=10&postId=ID_DEL_POST
```

## Alta disponibilidad
Se propone un Replica Set de MongoDB con:
- 2 nodos de datos
- 1 árbitro

## Sharding
Se propone configurar sharding sobre la colección `posts` por su crecimiento proyectado.

## Backups manuales
```bash
mongodump --uri="mongodb://localhost:27017" --db ulife_db --out ./backups/manual
```

```bash
mongorestore --uri="mongodb://localhost:27017" --db ulife_db ./backups/manual/ulife_db
```

## Backups automáticos con software de terceros
La herramienta propuesta es **Percona Backup for MongoDB (PBM)**, por su compatibilidad con Replica Set y sharding.

## Estado del proyecto
- API compilando correctamente
- Swagger funcional
- Endpoints CRUD implementados
- Filtros y paginación implementados en el código
- Pendiente documentar y demostrar réplica, sharding y respaldos
