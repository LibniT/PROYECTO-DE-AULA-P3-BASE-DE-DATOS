using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIDADES;

namespace LOGICA
{
    public class OracleService : DbContext
    {
        public OracleService() : base("name=OracleDbContext")
        {
            // Disable proxy creation to avoid circular reference issues
            this.Configuration.ProxyCreationEnabled = false;
            // Disable lazy loading to have more control over data loading
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<CLIENTE> Clientes { get; set; }
        public DbSet<PRODUCTO> Productos { get; set; }
        public DbSet<VENTA> Ventas { get; set; }
        public DbSet<ADMINISTRADOR> Administradores { get; set; }
        public DbSet<CARRITO> Carritos { get; set; }
        public DbSet<EFECTIVO> Efectivos { get; set; }
        public DbSet<DEUDA> Deudas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("ADMINISTRADOR");
            // Configure table names
            modelBuilder.Entity<ADMINISTRADOR>().ToTable("ADMINISTRADOR");
            modelBuilder.Entity<CLIENTE>().ToTable("CLIENTE");
            modelBuilder.Entity<PRODUCTO>().ToTable("PRODUCTO");
            modelBuilder.Entity<VENTA>().ToTable("VENTA");
            modelBuilder.Entity<CARRITO>().ToTable("CARRITO");

            // Configure inheritance for payment types
            // Use TPT (Table-Per-Type) inheritance without a base table
            modelBuilder.Entity<EFECTIVO>().ToTable("EFECTIVO");
            modelBuilder.Entity<DEUDA>().ToTable("DEUDA");

            // Ignore the abstract Pago class as it doesn't have a corresponding table

            // Configure relationships
            modelBuilder.Entity<VENTA>()
                .HasMany(v => v.Productos)
                .WithRequired()
                .HasForeignKey(c => c.venta_id);

            modelBuilder.Entity<VENTA>()
                .HasRequired(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.cliente_id);

            modelBuilder.Entity<CARRITO>()
                .HasRequired(c => c.Producto)
                .WithMany()
                .HasForeignKey(c => c.producto_id);

            modelBuilder.Entity<CARRITO>()
                .HasRequired(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.cliente_id);

            // Configure property mappings
            modelBuilder.Entity<VENTA>()
                .Property(v => v.Total)
                .HasColumnName("total");

            base.OnModelCreating(modelBuilder);
        }
    }
}
