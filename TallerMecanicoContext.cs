using Microsoft.EntityFrameworkCore;
using TallerMecanico.models;

namespace TallerMecanico;

public class TallerMecanicoContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<CartaPago> CartasPago { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Servicio> Servicios { get; set; }
    public DbSet<Vehiculo> Vehiculos { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Notificacion> Notificaciones { get; set; }
    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<Parte> Partes { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Username=kevin;Password=1234;Database=mydatabase");
        base.OnConfiguring(optionsBuilder);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación uno a muchos entre Cliente y Factura
        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Cliente)
            .WithMany(c => c.Facturas)
            .HasForeignKey(f => f.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Cliente y CartaPago
        modelBuilder.Entity<CartaPago>()
            .HasOne(cp => cp.Cliente)
            .WithMany(c => c.CartasPago)
            .HasForeignKey(cp => cp.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Factura y CartaPago
        modelBuilder.Entity<CartaPago>()
            .HasOne(cp => cp.Factura)
            .WithMany(f => f.CartasPago)
            .HasForeignKey(cp => cp.FacturaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Cliente y Pago
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pagos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Pago y Producto, Servicio, Parte
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Producto)
            .WithMany()
            .HasForeignKey(p => p.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Servicio)
            .WithMany()
            .HasForeignKey(p => p.ServicioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Parte)
            .WithMany()
            .HasForeignKey(p => p.ParteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación muchos a muchos entre Cliente y Servicio
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Servicios)
            .WithMany(s => s.Clientes)
            .UsingEntity<Dictionary<string, object>>(
                "ClienteServicio",
                cs => cs.HasOne<Servicio>()
                    .WithMany()
                    .HasForeignKey("ServicioId")
                    .OnDelete(DeleteBehavior.Restrict),
                cs => cs.HasOne<Cliente>()
                    .WithMany()
                    .HasForeignKey("ClienteId")
                    .OnDelete(DeleteBehavior.Restrict)
                
            );
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Relación uno a muchos entre Cliente y Vehiculo
        modelBuilder.Entity<Vehiculo>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Vehiculos)
            .HasForeignKey(v => v.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Cliente y Notificacion
        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Cliente)
            .WithMany(c => c.Notificaciones)
            .HasForeignKey(n => n.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Cliente y Carrito
        modelBuilder.Entity<Carrito>()
            .HasOne(c => c.Cliente)
            .WithMany(c => c.Carritos)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación uno a muchos entre Carrito y Producto
        modelBuilder.Entity<Carrito>()
            .HasOne(c => c.Producto)
            .WithMany(p => p.Carritos)
            .HasForeignKey(c => c.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice único para el código de factura
        modelBuilder.Entity<Factura>()
            .HasIndex(f => f.CodigoFactura)
            .IsUnique();
        // Relación muchos a muchos entre Vehiculo y Servicio
        modelBuilder.Entity<Vehiculo>()
            .HasMany(v => v.ServiciosRealizados)
            .WithMany(s => s.Vehiculos)
            .UsingEntity<Dictionary<string, object>>(
                "VehiculoServicio",
                vs => vs.HasOne<Servicio>()
                    .WithMany()
                    .HasForeignKey("ServicioId")
                    .OnDelete(DeleteBehavior.Restrict),
                vs => vs.HasOne<Vehiculo>()
                    .WithMany()
                    .HasForeignKey("VehiculoId")
                    .OnDelete(DeleteBehavior.Restrict)
            );
        // Configurar ProductoId, ServicioId y ParteId como opcionales en Pago
        modelBuilder.Entity<Pago>()
            .Property(p => p.ProductoId)
            .IsRequired(false);

        modelBuilder.Entity<Pago>()
            .Property(p => p.ServicioId)
            .IsRequired(false);

        modelBuilder.Entity<Pago>()
            .Property(p => p.ParteId)
            .IsRequired(false);

        // Configura la relación uno a muchos con Cliente y Pago
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pagos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);


        // Deshabilitar el borrado en cascada
        foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
