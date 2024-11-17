using AutoMapper;
using TallerMecanico.models;
using TallerMecanico.Dtos;

namespace TallerMecanico.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            // Mapeo de Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();

            // Mapeo de Carrito
            CreateMap<Carrito, CarritoDto>().ReverseMap();

            // Mapeo de CartaPago
            CreateMap<CartaPago, CartaPagoDto>().ReverseMap();

            // Mapeo de Factura
            CreateMap<Factura, FacturaDto>().ReverseMap();

            // Mapeo de Notificacion
            CreateMap<Notificacion, NotificacionDto>().ReverseMap();

            // Mapeo de Pago
            CreateMap<Pago, PagoDto>().ReverseMap();

            // Mapeo de Parte
            CreateMap<Parte, ParteDto>().ReverseMap();

            // Mapeo de Producto
            CreateMap<Producto, ProductoDto>().ReverseMap();

            // Mapeo de Servicio
            CreateMap<Servicio, ServicioDto>().ReverseMap();

            // Mapeo de Usuario
            CreateMap<Usuario, UsuarioDto>().ReverseMap();

            // Mapeo de Vehiculo
            CreateMap<Vehiculo, VehiculoDto>().ReverseMap();
        }
    }
}