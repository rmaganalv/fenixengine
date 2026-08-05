// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using FenixEngine.DataBase.Src.Models;
using FenixEngine.DataBase.Src.Utilis;
using Microsoft.EntityFrameworkCore;


namespace FenixEngine.DataBase.Src.Services
{
    public class ServiceDbContext: DbContext 
    {
        // 1. Quitar 'static' y registrar todas las tablas requeridas
        public DbSet<UserArchitect> UserArchitects { get; set; } = null!;
        public DbSet<GeneralProyects> GeneralProyects { get; set; } = null!;
        public DbSet<ProyectType> ProyectTypes { get; set; } = null!;
        public DbSet<UserActivity> UserActivities { get; set; } = null!;

        // Constructor para soportar Inyección de Dependencias
        public ServiceDbContext()
        {
        }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Usar formato estándar Sqlite
                string connexionDb = $"Data Source={DbConexion.DbPath("demo.db")}";
                optionsBuilder.UseSqlite(connexionDb);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clave primaria compuesta de UserActivity que ya teníamos
            modelBuilder.Entity<UserActivity>()
                .HasKey(ua => new { ua.UserId, ua.ProyectId });

            // 🚀 SEED DATA: Se inserta automáticamente al crear la base de datos
            modelBuilder.Entity<UserArchitect>().HasData(
                new UserArchitect
                {
                    //UserId = Guid.NewGuid().ToString(), // Usar una clave fija para el seed
                    UserName = "Big Boss",
                    UserBirth = new DateTime(1988, 8, 7)
                }
            );

            
        }

    }


}