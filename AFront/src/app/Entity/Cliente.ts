import { CartaPago } from './CartaPago';
import { Factura } from './Factura';
import { Usuario } from './Usuario';
import { Vehiculo } from './Vehiculo';
import { Servicio } from './Servicio';
import { Pago } from './Pago';
import { Notificacion } from './Notificacion';

export interface Cliente {
  id: number;
  usuarioId: number;
  usuario?: Usuario | null;

  // Nuevos campos para datos personales
  primerNombre: string;
  segundoNombre?: string | null;
  primerApellido: string;
  segundoApellido?: string | null;
  direccion: string;
  telefono: string;
  emailSecundario?: string | null;
  dni?: string | null;
  fechaNacimiento: Date | null;
  genero?: 'Masculino' | 'Femenino' | 'Otro' | null;
  estadoCivil?: 'Soltero' | 'Casado' | 'Divorciado' | 'Viudo' | 'UnionLibre' | null;
  ocupacion?: string | null;
  notas?: string | null;

  // Relaciones con otras entidades
  vehiculos?: Vehiculo[];
  servicios?: Servicio[];
  facturas?: Factura[];
  pagos?: Pago[];
  cartasPago?: CartaPago[];
  notificaciones?: Notificacion[];

  // Auditoría
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}
