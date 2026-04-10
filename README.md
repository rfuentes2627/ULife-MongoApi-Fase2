# U-Life MongoDB API - Fase 2

## Descripción
Este proyecto corresponde a la fase 2 del proyecto de cátedra de Base de Datos II.  
Se implementó una API REST con ASP.NET Core y MongoDB para la plataforma **U-Life**, una red social académica para estudiantes universitarios.

En esta fase se desarrolló la parte funcional de MongoDB, implementando operaciones CRUD, filtros, paginación y la estructura base para alta disponibilidad y respaldos.

## Tecnologías utilizadas
- ASP.NET Core Web API
- .NET 10
- MongoDB
- MongoDB.Driver
- Swagger / OpenAPI
- Docker
- Percona Backup for MongoDB (PBM) como herramienta propuesta para backups automáticos

## Colecciones implementadas
- `users`
- `posts`
- `comments`

## Estructura del proyecto
- `Controllers/` Controladores REST
- `DTOs/` Objetos de transferencia de datos
- `Models/` Modelos de las colecciones MongoDB
- `Services/` Lógica de negocio
- `Data/` Contexto de MongoDB
- `Settings/` Configuración de conexión
- `Helpers/` Inicialización de índices

## Configuración
En `appsettings.json` se configura la conexión a MongoDB:

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