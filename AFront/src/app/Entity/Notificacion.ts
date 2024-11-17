export interface Notificacion {
  id: number;
  clienteId: number;
  mensaje: string;
  fechaEnvio: Date;
  tipo: TipoNotificacion;
  leido: boolean;
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}


export enum TipoNotificacion {
  General = 0,
  Factura = 1,
  Carrito = 2,
  Servicio = 3,
  NuevaFactura = 4,
  RecordatorioPago = 5,
}
