using FenixEngine.Services.Src.Models;
using FenixEngine.Services.Src.Utilis;
using Microsoft.EntityFrameworkCore;


namespace FenixEngine.Services.Src.DataAccess
{
    public class EngineDbContext: DbContext 
    {
                // 1. Quitar 'static' y registrar todas las tablas requeridas
        public DbSet<UserArchitect> UserArchitects { get; set; } = null!;
        public DbSet<GeneralProyects> GeneralProyects { get; set; } = null!;
        public DbSet<ProyectType> ProyectTypes { get; set; } = null!;
        public DbSet<UserActivity> UserActivities { get; set; } = null!;

         // Constructor para soportar Inyección de Dependencias
        public EngineDbContext()
        {
        }

        public EngineDbContext(DbContextOptions<EngineDbContext> options) : base(options)
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