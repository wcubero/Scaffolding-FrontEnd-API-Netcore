# Scaffolding-FrontEnd-API-Netcore

 Guía/Ejemplo para mostrar la interación entre los proyectos de la solución
 
 
-API Basic Auth

-JSON serialize and JSON deserialize 

-Conexión a base de datos de Azure 

-Arquitectura orientada a servicios 

-ORM Dapper, para mapear las estructuras de datos automaticamente


Esta solución es un Scaffolding que tiene varios proyectos:


Empresa.NombreProyecto.FrontEnd:

Este proyecto tendrá la interacción visual con el usuario, no debe tener acceso a datos directamente, sino que debe consumir el proyecto API para la interacción con datos.

Empresa.NombreProyecto.API:

Este proyecto se encarga de proveer el comportamiento de las entidades, funciones y toda lógica de negocio.  Este API está documentado con Swagger para la fácil visualización e interacción con los controladores. Este proyecto consume el proyecto Exceltec.Presupuesto.Models, donde viven todas las entidades comunes.

Empresa.NombreProyecto.Models:

Este proyecto contiene todas las entidades necesarias. Interactua directamente con las consultas de la base de datos.
