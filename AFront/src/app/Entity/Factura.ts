import {Cliente} from './Cliente';
import {CartaPago} from './CartaPago';
import {ProductoFactura} from './ProductoFactura';
import {ServicioFactura} from './ServicioFactura';

export interface Factura {
  id?: number;
  codigoFactura?: string;
  clienteId: number;
  cliente?: Cliente;
  total: number|null;
  fechaCreacion: Date;
  fechaVencimiento: Date;
  estado: number|null;
  cartasPago?: CartaPago[];
  productosFactura?: ProductoFactura[];
  serviciosFactura?: ServicioFactura[];
  comentarios?: string;
}
export enum EstadoFactura {
  Pendiente = 1,
  Pagada = 0,
}

