export interface Carrito {
  id: number;
  clienteId: number;
  producto?: {
    id: number;
    nombre: string;
    imagen: string;
    precio: number;
  };
  servicio?: {
    id: number;
    nombre: string;
    precio: number;
  };
  cantidad: number;
  precioTotal: number;
  estaBorrado: boolean;
}
