# NT1- Sistema de gestión de turnos (SGT)

#### Roles

- Recepcionista
- Profesional
- Admin

#### Flujo de estados del turno

- Recepcionista/Profesional --> Crea turno [Pendiente]

- Recepcionista --> Check in de turno [Encolado]

- Profesional --> LLama paciente [EnAtencion]

- Profesional --> Finaliza turno [Finalizado]

- Recepcionista/Profesional  --> Cancela turno [Cancelado]