using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Profesional> Profesionales { get; set; }
        public DbSet<Recepcionista> Recepcionistas { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<ProfesionalCola> Colas { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<DiaHorario> Horarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            BuildTables(builder);
            BuildPaciente(builder);
            BuildUsuario(builder);
            BuildEspecialidad(builder);
            BuildProfesional(builder);
            BuildTurno(builder);
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
        }

        private void BuildPaciente(ModelBuilder builder)
        {
            builder.Entity<Paciente>()
               .Property(p => p.Dni)
               .HasMaxLength(8)
               .IsRequired();

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
        }

        private void BuildTurno(ModelBuilder builder)
        {

            builder.Entity<Turno>()
                .Property(p => p.Fecha)
                .HasColumnType("date");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Server=localhost;Database=nt1-sgt;User=sa;Password=GGalp3r1n", x => x.MigrationsAssembly("Infrastructure.Data"));
        }
    }
}