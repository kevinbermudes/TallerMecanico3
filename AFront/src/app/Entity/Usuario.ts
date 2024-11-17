export interface Usuario {
  id: number;
  nombre: string;
  apellido: string;
  email: string;
  rol: RolUsuario;
  fechaRegistro: Date;
  estado: boolean;
  fechaCreacion: Date;
  fechaActualizacion: Date | null;
  fechaBorrado: Date | null;
  estaBorrado: boolean;
}

export enum RolUsuario {
  Admin = 0,
  Cliente = 1
}
