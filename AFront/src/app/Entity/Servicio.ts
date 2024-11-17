import { Cliente } from './Cliente';
import { Vehiculo } from './Vehiculo';

export interface Servicio {
  id: number;
  nombre: string;
  descripcion: string;
  precio: number;
  clientes: Cliente[];
  vehiculos: Vehiculo[];
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}
