export interface Pago {
  id: number;
  clienteId: number;
  monto: number;
  fechaTransaccion: Date;
  tipoPago: TipoPago; // enum que puedes definir en TypeScript
  productoId: number | null;
  servicioId: number | null;
  parteId: number | null;
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}

// Enum para tipo de pago
export enum TipoPago {
  Producto = 0,
  Servicio = 1,
  Parte = 2
}
