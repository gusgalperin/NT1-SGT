using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Context
{
    public static class Seed
    {
        public async static Task SeedDataAsync(this DataContext context)
        {
            var hayCambios = false;

            await context.Database.EnsureCreatedAsync();

            hayCambios = hayCambios
                || await SeedEspecialidadesAsync(context)
                || await SeedAdminAsync(context)
                || await SeedProfesionalesAsync(context)
                || await SeedRecepcionistasAsync(context)
                || await SeedPacientesAsync(context);

            if (hayCambios)
                await context.SaveChangesAsync();
        }

        private async static Task<bool> SeedEspecialidadesAsync(DataContext context)
        {
            if (await context.Especialidades.AnyAsync())
                return false;

            await context.Especialidades.AddRangeAsync(
                new Especialidad("Dermatología"), 
                new Especialidad("Clínico"), 
                new Especialidad("Nutrición"),
                new Especialidad("Gastroenterología"),
                new Especialidad("Traumatología"),
                new Especialidad("Otorrinolaringología"),
                new Especialidad("Pediatría"),
                new Especialidad("Cardiología"));

            return true;
        }

        private async static Task<bool> SeedAdminAsync(DataContext context)
        {
            if (await context.Admins.AnyAsync())
                return false;

            await context.Admins.AddAsync(new Admin("Admin", "admin@sgt.com", "admin"));

            return true;
        }

        private async static Task<bool> SeedProfesionalesAsync(DataContext context)
        {
            if (await context.Profesionales.AnyAsync())
                return false;

            var especialidades = await context.Especialidades.ToListAsync();

            await context.Profesionales.AddRangeAsync(
                new Profesional("Charly Garcia", "charly@garcia.com", "sarasa", especialidades.Where(x => x.Descripcion == "Traumatología").ToList(), DiaHorario.DefaultTodaLaSemana()),
                new Profesional("Luis Alberto Spinetta", "luis@spinetta.com", "sarasa", especialidades.Where(x => x.Descripcion == "Cardiología").ToList(), DiaHorario.DefaultTodaLaSemana()),
                new Profesional("Mercedes Sosa", "mercedes@sosa.com", "sarasa", especialidades.Where(x => x.Descripcion == "Dermatología").ToList(), DiaHorario.DefaultTodaLaSemana()),
                new Profesional("Andres Calamaro", "andres@calamaro.com", "sarasa", especialidades.Where(x => x.Descripcion == "Pediatría").ToList(), DiaHorario.DefaultTodaLaSemana()));

            return true;
        }

        private async static Task<bool> SeedRecepcionistasAsync(DataContext context)
        {
            if (await context.Recepcionistas.AnyAsync())
                return false;

            await context.Recepcionistas.AddRangeAsync(
                new Recepcionista("Lito Nebbia", "lito@nebbia.com", "sarasa"),
                new Recepcionista("David Lebon", "david@lebon.com", "sarasa"));

            return true;
        }

        private async static Task<bool> SeedPacientesAsync(DataContext context)
        {
            if (await context.Pacientes.AnyAsync())
                return false;

            var hoy = DateTime.Today;

            await context.Pacientes.AddRangeAsync(
                new Paciente("Pedro Convaleciente", "1234567", hoy),
                new Paciente("Laura Perdida", "14857987", hoy),
                new Paciente("Juan Roto", "33123411", hoy),
                new Paciente("Lucas Dolido", "9999999", hoy));

            return true;
        }
    }
}