### Proyecto: FocusFlow  
**Autores**: Iván Morales Salgado, Laura Amaya, Daniela Martínez Zuluaga  



#### Descripción General
FocusFlow es una aplicación web desarrollada con Blazor y .NET Core API que tiene como objetivo principal fomentar la productividad y la organización a través de la técnica Pomodoro. Este proyecto está diseñado tanto para estudiantes como para profesores, permitiendo a los usuarios organizar sus tareas, realizar un seguimiento del tiempo invertido en actividades y fomentar hábitos productivos mediante recompensas motivacionales.  

El sistema está estructurado de manera que el profesor (administrador) pueda asignar proyectos a los estudiantes y gestionar sus actividades, mientras que los estudiantes pueden registrar sus avances, realizar tareas y ganar bonificaciones como reconocimiento por su esfuerzo.  



#### Funcionalidades Clave
1. **Gestión de Proyectos y Tareas**  
   - Los profesores pueden crear proyectos y asignarlos a los estudiantes.  
   - Los estudiantes pueden registrar tareas dentro de los proyectos asignados, estableciendo fechas límite y descripciones detalladas.  

2. **Sesiones Pomodoro**  
   - Los estudiantes pueden registrar y gestionar sesiones Pomodoro, configurando duración, estado y asociándolas a tareas o proyectos.  
   - El sistema permite hacer un seguimiento del tiempo invertido en cada tarea y proyecto.  

3. **Historial y Reportes**  
   - Los usuarios pueden consultar un historial de sus actividades y sesiones Pomodoro, obteniendo datos sobre horas dedicadas y tareas completadas.  
   - Los profesores pueden evaluar el progreso de los estudiantes mediante reportes generales.  

4. **Recompensas y Motivación**  
   - Los estudiantes pueden recibir recompensas como bonificaciones por completar tareas dentro de los tiempos establecidos.  
   - Estas recompensas están diseñadas para motivar la participación y la organización.  

5. **Seguridad y Roles**  
   - Implementación de roles y autenticación:  
     - **Administrador (Profesor)**: Gestión completa del sistema, incluyendo operaciones CRUD (crear, leer, actualizar y eliminar) sobre entidades como proyectos, tareas y sesiones Pomodoro.  
     - **Usuario (Estudiante)**: Acceso limitado a las funcionalidades correspondientes, como la gestión de sus propias tareas y sesiones Pomodoro.  



#### Estructura del Proyecto
1. **Backend (API REST)**  
   - Desarrollado en .NET Core, incluye controladores que manejan las operaciones CRUD para las entidades principales del sistema:  
     - Proyectos  
     - Tareas  
     - Sesiones Pomodoro  
     - Técnicas de Estudio  
     - Recompensas  

   - La base de datos utiliza **Entity Framework Core** para manejar migraciones y consultas, mientras que los datos están estructurados con entidades y DTOs (Data Transfer Objects) para garantizar un intercambio eficiente entre el backend y el frontend.  

2. **Frontend (Blazor)**  
   - Construido con Blazor, el frontend proporciona una interfaz intuitiva y dinámica para que los usuarios gestionen sus actividades.  
   - Incluye páginas como:  
     - Dashboard con navegación lateral.  
     - Formularios para crear y editar entidades.  
     - Tablas para listar proyectos, tareas, y sesiones Pomodoro.  
     - Sección de historial con reportes gráficos.  

3. **Estilos y Animaciones**  
   - El diseño visual se centra en la simplicidad y la funcionalidad, utilizando colores vivos (como rojo y sus combinaciones) para reflejar un ambiente motivador.  
   - Animaciones sutiles para botones y elementos clave que mejoran la experiencia de usuario.  

4. **Integraciones**  
   - **SweetAlert2** para notificaciones de éxito, error y confirmación.  
   - **Autenticación**: Sistema basado en roles con políticas específicas para limitar el acceso a ciertas funcionalidades.  



#### Público Objetivo
FocusFlow está diseñado específicamente para:
- **Profesores**: Que deseen mantener el control y la organización en sus aulas, asignar tareas de manera efectiva y motivar a los estudiantes a completar sus objetivos.  
- **Estudiantes**: Que buscan una herramienta efectiva para organizar su tiempo, realizar un seguimiento de sus avances y ser recompensados por su esfuerzo.  



#### Tecnologías Utilizadas
- **Backend**: .NET Core API, Entity Framework Core  
- **Frontend**: Blazor  
- **Base de Datos**: SQL Server  
- **Diseño Visual**: Bootstrap, CSS personalizado  
- **Herramientas de Notificación**: SweetAlert2  
- **Seguridad**: ASP.NET Identity para autenticación y autorización  



#### Impacto Esperado
FocusFlow no solo busca mejorar la productividad, sino también enseñar a los usuarios técnicas efectivas de organización, incentivando la disciplina y el manejo del tiempo mediante herramientas modernas e intuitivas. Al fomentar un ambiente educativo más estructurado y motivador, la aplicación espera ser un recurso valioso tanto en el ámbito académico como en la gestión personal.  
