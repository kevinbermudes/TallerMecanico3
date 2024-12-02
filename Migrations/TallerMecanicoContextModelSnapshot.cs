﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TallerMecanico;

#nullable disable

namespace TallerMecanico.Migrations
{
    [DbContext(typeof(TallerMecanicoContext))]
    partial class TallerMecanicoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TallerMecanico.models.Carrito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("integer");

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ParteId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PrecioTotal")
                        .HasColumnType("numeric");

                    b.Property<int?>("ProductoId")
                        .HasColumnType("integer");

                    b.Property<int?>("ServicioId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ParteId");

                    b.HasIndex("ProductoId");

                    b.HasIndex("ServicioId");

                    b.ToTable("Carritos");
                });

            modelBuilder.Entity("TallerMecanico.models.CartaPago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<int>("FacturaId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaPago")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MetodoPago")
                        .HasColumnType("integer");

                    b.Property<decimal>("Monto")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("FacturaId");

                    b.ToTable("CartasPago");
                });

            modelBuilder.Entity("TallerMecanico.models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Dni")
                        .HasColumnType("text");

                    b.Property<string>("EmailSecundario")
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<int?>("EstadoCivil")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaNacimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("Genero")
                        .HasColumnType("integer");

                    b.Property<string>("Notas")
                        .HasColumnType("text");

                    b.Property<string>("Ocupacion")
                        .HasColumnType("text");

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrimerNombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SegundoApellido")
                        .HasColumnType("text");

                    b.Property<string>("SegundoNombre")
                        .HasColumnType("text");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("TallerMecanico.models.ClienteServicio", b =>
                {
                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<int>("ServicioId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FechaAsignacion")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("ClienteId", "ServicioId");

                    b.HasIndex("ServicioId");

                    b.ToTable("ClienteServicios");
                });

            modelBuilder.Entity("TallerMecanico.models.Factura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<string>("CodigoFactura")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Comentarios")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<int>("Estado")
                        .HasColumnType("integer");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaVencimiento")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Total")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("CodigoFactura")
                        .IsUnique();

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("TallerMecanico.models.Notificacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaEnvio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Leido")
                        .HasColumnType("boolean");

                    b.Property<string>("Mensaje")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Tipo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Notificaciones");
                });

            modelBuilder.Entity("TallerMecanico.models.Pago", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaTransaccion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Monto")
                        .HasColumnType("numeric");

                    b.Property<int?>("ParteId")
                        .HasColumnType("integer");

                    b.Property<int?>("ProductoId")
                        .HasColumnType("integer");

                    b.Property<int?>("ServicioId")
                        .HasColumnType("integer");

                    b.Property<int>("TipoPago")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ParteId");

                    b.HasIndex("ProductoId");

                    b.HasIndex("ServicioId");

                    b.ToTable("Pagos");
                });

            modelBuilder.Entity("TallerMecanico.models.Parte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Costo")
                        .HasColumnType("numeric");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("imagen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Partes");
                });

            modelBuilder.Entity("TallerMecanico.models.Producto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Categoria")
                        .HasColumnType("integer");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Precio")
                        .HasColumnType("numeric");

                    b.Property<int>("Stock")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("TallerMecanico.models.ProductoFactura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("integer");

                    b.Property<int>("FacturaId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductoId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FacturaId");

                    b.HasIndex("ProductoId");

                    b.ToTable("ProductoFactura");
                });

            modelBuilder.Entity("TallerMecanico.models.Servicio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Imagen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Precio")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Servicios");
                });

            modelBuilder.Entity("TallerMecanico.models.ServicioFactura", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FacturaId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Precio")
                        .HasColumnType("numeric");

                    b.Property<int>("ServicioId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FacturaId");

                    b.HasIndex("ServicioId");

                    b.ToTable("ServicioFactura");
                });

            modelBuilder.Entity("TallerMecanico.models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<bool>("Estado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Rol")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("TallerMecanico.models.Vehiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("integer");

                    b.Property<bool>("EstaBorrado")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("FechaActualizacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("FechaBorrado")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Vehiculos");
                });

            modelBuilder.Entity("VehiculoServicio", b =>
                {
                    b.Property<int>("ServicioId")
                        .HasColumnType("integer");

                    b.Property<int>("VehiculoId")
                        .HasColumnType("integer");

                    b.HasKey("ServicioId", "VehiculoId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("VehiculoServicio");
                });

            modelBuilder.Entity("TallerMecanico.models.Carrito", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("Carritos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Parte", "Parte")
                        .WithMany()
                        .HasForeignKey("ParteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TallerMecanico.models.Producto", "Producto")
                        .WithMany("Carritos")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TallerMecanico.models.Servicio", "Servicio")
                        .WithMany()
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Cliente");

                    b.Navigation("Parte");

                    b.Navigation("Producto");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("TallerMecanico.models.CartaPago", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("CartasPago")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Factura", "Factura")
                        .WithMany("CartasPago")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Factura");
                });

            modelBuilder.Entity("TallerMecanico.models.Cliente", b =>
                {
                    b.HasOne("TallerMecanico.models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("TallerMecanico.models.ClienteServicio", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("ClienteServicios")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Servicio", "Servicio")
                        .WithMany("ClienteServicios")
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("TallerMecanico.models.Factura", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("Facturas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("TallerMecanico.models.Notificacion", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("Notificaciones")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("TallerMecanico.models.Pago", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("Pagos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Parte", "Parte")
                        .WithMany()
                        .HasForeignKey("ParteId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TallerMecanico.models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("TallerMecanico.models.Servicio", "Servicio")
                        .WithMany()
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Cliente");

                    b.Navigation("Parte");

                    b.Navigation("Producto");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("TallerMecanico.models.Parte", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("TallerMecanico.models.ProductoFactura", b =>
                {
                    b.HasOne("TallerMecanico.models.Factura", "Factura")
                        .WithMany("ProductosFactura")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Producto", "Producto")
                        .WithMany()
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Factura");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("TallerMecanico.models.Servicio", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", null)
                        .WithMany("Servicios")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("TallerMecanico.models.ServicioFactura", b =>
                {
                    b.HasOne("TallerMecanico.models.Factura", "Factura")
                        .WithMany("ServiciosFactura")
                        .HasForeignKey("FacturaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Servicio", "Servicio")
                        .WithMany()
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Factura");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("TallerMecanico.models.Vehiculo", b =>
                {
                    b.HasOne("TallerMecanico.models.Cliente", "Cliente")
                        .WithMany("Vehiculos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("VehiculoServicio", b =>
                {
                    b.HasOne("TallerMecanico.models.Servicio", null)
                        .WithMany()
                        .HasForeignKey("ServicioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TallerMecanico.models.Vehiculo", null)
                        .WithMany()
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("TallerMecanico.models.Cliente", b =>
                {
                    b.Navigation("Carritos");

                    b.Navigation("CartasPago");

                    b.Navigation("ClienteServicios");

                    b.Navigation("Facturas");

                    b.Navigation("Notificaciones");

                    b.Navigation("Pagos");

                    b.Navigation("Servicios");

                    b.Navigation("Vehiculos");
                });

            modelBuilder.Entity("TallerMecanico.models.Factura", b =>
                {
                    b.Navigation("CartasPago");

                    b.Navigation("ProductosFactura");

                    b.Navigation("ServiciosFactura");
                });

            modelBuilder.Entity("TallerMecanico.models.Producto", b =>
                {
                    b.Navigation("Carritos");
                });

            modelBuilder.Entity("TallerMecanico.models.Servicio", b =>
                {
                    b.Navigation("ClienteServicios");
                });
#pragma warning restore 612, 618
        }
    }
}
