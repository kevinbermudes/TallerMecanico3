export interface Carrito {
  id: number;
  clienteId: number;
  productoId: number;
  cantidad: number;
  precioTotal: number;
  fechaCreacion: string;
  fechaActualizacion: string | null;
  fechaBorrado: string | null;
  estaBorrado: boolean;
}
