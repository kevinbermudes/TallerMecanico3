import {Cliente} from './Cliente';
import {CartaPago} from './CartaPago';

export interface Factura {
  id: number;
  codigoFactura: string;
  clienteId: number;
  cliente?: Cliente;
  total: number;
  fechaCreacion: Date;
  fechaVencimiento: Date;
  estado: number;
  cartasPago?: CartaPago[];
}
