TaskManager API - Clean Architecture con .NET 10
Este repositorio contiene una API REST diseñada para la gestión de tareas personales. La prioridad del proyecto es mantener un código desacoplado y fácil de testear, aplicando Clean Architecture y patrones de diseño modernos sobre .NET 10.

Stack Tecnológico
Runtime: .NET 10
Base de datos: SQLite para desarrollo local y SQL Server para entornos de producción.
ORM: Entity Framework Core 10 con un enfoque Code First.
Seguridad: Autenticación mediante JWT (HS256) y hashing de contraseñas con BCrypt (work factor 12).
Validaciones: FluentValidation 12 para mantener las entidades de dominio limpias de lógica de entrada.

Estructura del Proyecto
El código está organizado en cuatro capas principales, respetando que las dependencias siempre apunten hacia el Dominio:
TaskManager.Domain: Contiene el núcleo del negocio. Aquí residen las entidades (User, TaskItem), los enums y las interfaces de los repositorios. No tiene dependencias externas.
TaskManager.Application: Define los casos de uso. Incluye los DTOs, la lógica de validación, los servicios de aplicación y el Result Pattern para el manejo de errores.
TaskManager.Infrastructure: Implementaciones técnicas. Aquí se encuentra el DbContext de EF Core, la lógica de los repositorios, la generación de tokens JWT y el hasher de contraseñas.
TaskManager.API: El punto de entrada del sistema. Contiene los controladores, middlewares de excepción global y la configuración del Program.cs.

Cómo empezar
Requisitos previos
.NET 10 SDK instalado.

Pasos para ejecución local
Clonar el repositorio.
Desde la raíz, ejecutar dotnet restore para descargar los paquetes necesarios.
Entrar a la carpeta del proyecto de entrada: cd src/TaskManager.API.
Ejecutar la aplicación: dotnet run.
La API estará disponible en http://localhost:5230. Al iniciar en modo desarrollo, se creará automáticamente un archivo llamado taskmanager_dev.db en la carpeta de la API con las tablas necesarias.

Uso de la API y Autenticación
Todos los endpoints de tareas requieren que el usuario esté autenticado.
Registrarse: Enviar un POST a /api/auth/register. La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula y un número.
Login: Enviar un POST a /api/auth/login. El sistema devolverá un accessToken.
Autorización: En Swagger (o tu cliente de preferencia), utiliza el esquema Bearer. Debes incluir el header: Authorization: Bearer <tu_token>.

Decisiones de Diseño
Result Pattern: Se implementó un objeto genérico Result<T> para manejar fallos de lógica de negocio (como "tarea no encontrada"). Esto evita el uso excesivo de excepciones, mejorando el rendimiento y la legibilidad del flujo.
Nombramiento de Enums: El enum de estados se llama WorkTaskStatus para evitar conflictos de nombres con la clase System.Threading.Tasks.Task propia de .NET.
Persistencia de Enums: Se configuran para guardarse como texto (strings) en la base de datos. Esto facilita la depuración manual de los archivos .db y evita errores si se cambia el orden de los elementos en el código.

Mantenimiento de Base de Datos
Si realizas cambios en las entidades del dominio, recuerda generar una nueva migración:
dotnet ef migrations add NombreDeLaMigracion --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

Para aplicar los cambios manualmente:
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API