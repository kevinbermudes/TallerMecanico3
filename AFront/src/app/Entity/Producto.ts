import {Carrito} from './Carrito';
export interface BackendProductoResponse {
  $values: Producto[];
}

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoria: number;
  fechaCreacion: string;
  fechaActualizacion: string | null;
  fechaBorrado: string | null;
  estaBorrado: boolean;
  carritos: { $values: Carrito[] } | Carrito[];
  imagen: string;
}
