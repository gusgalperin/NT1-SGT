using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data.Context
{
    public class DataContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DataContext(DbContextOptions<DataContext> options, ILoggerFactory loggerFactory)
            : base(options)
        {
            _loggerFactory = loggerFactory ?? throw new System.ArgumentNullException(nameof(loggerFactory));
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<ProfesionalCola> Colas { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<DiaHorario> Horarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<TurnoHistorial> TurnoHistorial { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            #if DEBUG
            builder.UseLoggerFactory(_loggerFactory);
            builder.EnableSensitiveDataLogging();
            #endif
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            BuildTables(builder);
            BuildPaciente(builder);
            BuildUsuario(builder);
            BuildEspecialidad(builder);
            BuildProfesional(builder);
            BuildTurno(builder);
            BuildRol(builder);
        }

        private void BuildTables(ModelBuilder builder)
        {
            builder.Entity<Usuario>().ToTable("Usuario").HasKey(x => x.Id);
            builder.Entity<Paciente>().ToTable("Paciente").HasKey(x => x.Id);
            builder.Entity<Especialidad>().ToTable("Especialidad").HasKey(x => x.Id);
            builder.Entity<ProfesionalCola>().ToTable("Cola").HasKey(x => x.Id);
            builder.Entity<Turno>().ToTable("Turno").HasKey(x => x.Id);
            builder.Entity<DiaHorario>().ToTable("Horario").HasKey(x => x.Id);
            builder.Entity<ProfesionalEspecialidad>().ToTable("ProfesionalEspecialidad");
            builder.Entity<Rol>().ToTable("Rol");
            builder.Entity<Permiso>().ToTable("Permiso");
            builder.Entity<TurnoHistorial>().ToTable("TurnoHistorial");
        }

        private void BuildEspecialidad(ModelBuilder builder)
        {
            builder.Entity<Especialidad>()
               .Property(p => p.Descripcion)
               .HasMaxLength(50)
               .IsRequired();
        }

        private void BuildUsuario(ModelBuilder builder)
        {
            builder.Entity<Usuario>()
               .Property(p => p.Email)
               .HasMaxLength(80)
               .IsRequired();
            
            builder.Entity<Usuario>()
               .Property(p => p.Nombre)
               .HasMaxLength(80)
               .IsRequired();
            
            builder.Entity<Usuario>()
               .Property(p => p.Password)
               .HasMaxLength(80)
               .IsRequired();

            builder.Entity<Usuario>()
                .HasOne(x => x.Rol);
        }

        private void BuildPaciente(ModelBuilder builder)
        {
            builder.Entity<Paciente>()
               .Property(p => p.Dni)
               .HasMaxLength(8)
               .IsRequired();

            builder.Entity<Paciente>()
                .HasIndex(i => i.Dni)
                .IsUnique();

            builder.Entity<Paciente>()
                .Property(p => p.FechaAlta)
                .IsRequired();

            builder.Entity<Paciente>()
                .Property(p => p.FechaNacimiento)
                .IsRequired();

            builder.Entity<Paciente>()
                .Property(p => p.Nombre)
                .HasMaxLength(50)
                .IsRequired();
        }

        private void BuildProfesional(ModelBuilder builder)
        {
            builder.Entity<Profesional>()
                .HasMany(x => x.DiasQueAtiende)
                .WithOne(x => x.Profesional)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Profesional>()
                .HasMany(p => p.Cola)
                .WithOne(x => x.Profesional)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProfesionalEspecialidad>()
                .HasKey(x => new { x.ProfesionalId, x.EspecialidadId });

            builder.Entity<ProfesionalEspecialidad>(b =>
            {
                b.HasOne(x => x.Profesional).WithMany(x => x.Especialidades).OnDelete(DeleteBehavior.Restrict); 
                b.HasOne(x => x.Especialidad).WithMany(x => x.Profesionales).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Especialidad>()
                .Property(p => p.Descripcion)
                .IsRequired();

            builder.Entity<ProfesionalCola>()
                .Ignore(p => p.OperationType);
        }

        private void BuildTurno(ModelBuilder builder)
        {

            builder.Entity<Turno>()
                .Property(p => p.Fecha)
                .HasColumnType("date");

            builder.Entity<Turno>()
                .HasMany(x => x.Historial)
                .WithOne(x => x.Turno);

            builder.Entity<TurnoHistorial>()
                .HasOne(x => x.Turno)
                .WithMany(x => x.Historial)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TurnoHistorial>()
                .HasOne(x => x.Usuario)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void BuildRol(ModelBuilder builder)
        {
            builder.Entity<Rol>()
                .HasMany(x => x.Permisos)
                .WithOne(x => x.Rol)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RolPermiso>()
                .HasKey(x => new { x.RolId, x.PermisoId });

            builder.Entity<RolPermiso>(b =>
            {
                b.HasOne(x => x.Rol).WithMany(x => x.Permisos).OnDelete(DeleteBehavior.Restrict);
                //b.HasOne(x => x.Especialidad).WithMany(x => x.Profesionales).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}