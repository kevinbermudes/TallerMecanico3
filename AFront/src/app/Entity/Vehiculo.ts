import { Cliente } from './Cliente';
import { Servicio } from './Servicio';

export interface Vehiculo {
  id: number;
  clienteId: number;
  cliente: Cliente | null;
  marca: string;
  modelo: string;
  //posible problema ojo
  year: number;
  placa: string;
  serviciosRealizados: Servicio[];
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}
