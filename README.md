
Desarrollado con [Docker](https://www.docker.com/) y complementos de [Visual Studio Code](https://code.visualstudio.com/)

**Importante:** Requiere tener [Docker](https://www.docker.com/) instalado. Para levantar el proyecto ejecutar los siguientes pasos:

1. `docker-compose -f docker-compose.yml build --no-cache`
2. `docker-compose -f docker-compose.yml up -d`
3. Acceder a http://localhost:5002/ para la documentación generada.
4. Acceder a http://localhost:5002/api/Libro para un ejemplo de la respuesta Get
5. Acceder http://localhost:5002/api/Libro/1 para un ejemplo de la respuesta Get/1

Puede validar la creación de la imagen y contenedor:

1. `docker images`
2. `docker ps -a`
