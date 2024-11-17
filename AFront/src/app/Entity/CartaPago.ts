import {Cliente} from './Cliente';
import {Factura} from './Factura';

export interface CartaPago {
  id: number;
  facturaId: number;
  factura?: Factura;
  clienteId: number;
  cliente?: Cliente;
  monto: number;
  fechaPago: Date;
  metodoPago: MetodoPago;
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}
export enum MetodoPago {
  Efectivo = 0,
  Tarjeta = 1,
  Transferencia = 2,
}
