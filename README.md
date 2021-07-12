# NT1- Sistema de gestión de turnos (SGT)

## Roles

- Recepcionista
- Profesional
- Admin

## Flujo de estados del turno

### Acciones

> `Crear` --> Profesional | Recepcionista
>
> `Check-In` --> Recepcionista
> 
> `Llamar` --> Profesional
> 
> `FinDeAtencion`  --> Profesional
> 
> `Cancelar` --> Profesional | Recepcionista


```mermaid
stateDiagram-v2
    [*] --> Pendiente : Creación
    Pendiente --> Encolado : Check-In
    Encolado --> EnAtencion : Llamada
    EnAtencion --> Finalizado : Fin de atención
    Pendiente --> Cancelado : Cancelar
    Encolado --> Cancelado : Cancelar
    Finalizado --> [*]
```

### UML (Resumido)


```mermaid
 classDiagram
      Usuario <|-- Recepcionista
      Usuario <|-- Admin
      Usuario <|-- Profesional
      Profesional --> Especialidad : Especialidades  (*)
      Profesional --> DiaHora : DiasQueAtiende  (*)
      Profesional --> Cola : ColaAtencion  (*)
      Cola --> Turno : DiasQueAtiende  (*)
      Turno --> DiaHora : Fecha (1)
      Turno --> Profesional : Profesional (1)
      Turno --> Paciente : Paciente (1)
      Usuario --> Rol : Rol (1)
      Rol --> Permiso : Permisos (*)

      class Usuario{ 
          -string Nombre
          -String Email
          -String Password
      }
      class Recepcionista{
      }
      class Admin{
      }
      class Profesional{
      }
      class Especialidad{
          -string Especialidad
      }
      class DiaHora{
          -DateTime Fecha
          -Timespan HoraDesde
          -Timespan HoraHasta
      }
      class Paciente{
          -string Nombre
          -string DNI
          -DateTime FechaNacimiento
          -DateTime FechaAlta
      }
      class Turno{

      }
      class Cola{
          -int Orden
      }
      class Rol{
          -string Descripcion
      }
      class Permiso{
          -string Descripcion
      }
```
