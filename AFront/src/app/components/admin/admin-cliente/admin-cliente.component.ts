import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import {Table, TableModule} from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import {Cliente} from '../../../Entity/Cliente';
import {ClienteService} from '../../../Service/cliente.service';
import {DatePipe, NgIf} from '@angular/common';

@Component({
  selector: 'app-admin-cliente',
  standalone: true,
  templateUrl: './admin-cliente.component.html',
  styleUrls: ['./admin-cliente.component.css'],
  providers: [MessageService],
  imports: [DialogModule, TableModule, ButtonModule, FormsModule, InputTextModule, ToastModule, DatePipe, NgIf]
})
export class AdminClienteComponent implements OnInit {
  clientes: Cliente[] = [];
  clienteSeleccionado: Cliente | null = null;
  mostrarDialogo: boolean = false;
  mostrarDetalles: boolean = false;
  searchValue: string = '';
  filters: any = {};

  nuevoCliente: Cliente = {
    id: 0,
    usuarioId: 0,
    primerNombre: '',
    primerApellido: '',
    direccion: '',
    telefono: '',
    fechaNacimiento: null,
    genero: null,
    estadoCivil: null,
    vehiculos: [],
    servicios: [],
    facturas: [],
    pagos: [],
    cartasPago: [],
    notificaciones: [],
    estaBorrado: false,
    fechaCreacion: new Date(),
    fechaActualizacion: null,
    fechaBorrado: null
  };

  constructor(
    private clienteService: ClienteService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.clienteService.getAllClientes().subscribe({
      next: (clientes) => (this.clientes = clientes),
      error: (err) =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los clientes.'
        })
    });
  }

  mostrarDialogoNuevo(): void {
    this.nuevoCliente = {
      id: 0,
      usuarioId: 0,
      primerNombre: '',
      primerApellido: '',
      direccion: '',
      telefono: '',
      fechaNacimiento: null,
      genero: null,
      estadoCivil: null,
      vehiculos: [],
      servicios: [],
      facturas: [],
      pagos: [],
      cartasPago: [],
      notificaciones: [],
      estaBorrado: false,
      fechaCreacion: new Date(),
      fechaActualizacion: null,
      fechaBorrado: null
    };
    this.mostrarDialogo = true;
  }
  verDetallesCliente(cliente: Cliente): void {
    this.clienteSeleccionado = cliente;
    this.mostrarDetalles = true;
  }
  guardarCliente(): void {
    if (this.nuevoCliente.id) {
      // Actualizar cliente existente
      this.clienteService.updateCliente(this.nuevoCliente.id, this.nuevoCliente).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Cliente actualizado correctamente.'
          });
          this.cargarClientes();
          this.mostrarDialogo = false;
        },
        error: (err) =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo actualizar el cliente.'
          })
      });
    } else {
      // Crear cliente nuevo
      this.clienteService.createCliente(this.nuevoCliente).subscribe({
        next: (clienteCreado) => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Cliente creado correctamente.'
          });
          this.clientes.push(clienteCreado);
          this.mostrarDialogo = false;
        },
        error: (err) =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo crear el cliente.'
          })
      });
    }
  }

  eliminarCliente(clienteId: number): void {
    this.clienteService.deleteCliente(clienteId).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Cliente eliminado correctamente.'
        });
        this.clientes = this.clientes.filter((cliente) => cliente.id !== clienteId);
        this.cargarClientes()
      },
      error: (err) =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo eliminar el cliente.'
        })
    });
  }

  editarCliente(cliente: any) {

  }

  getGeneroLabel(genero: "Masculino" | "Femenino" | "Otro" | null | undefined): string {
    const generos: { [key: string]: string } = {
      '0': 'Masculino',
      '1': 'Femenino',
      '2': 'Otro'
    };
    // @ts-ignore
    return genero !== null ? generos[genero] || 'No especificado' : 'No especificado';
  }

  getEstadoCivilLabel(estadoCivil: "Soltero" | "Casado" | "Divorciado" | "Viudo" | "UnionLibre" | null | undefined): string {
    const estadosCiviles: { [key: string]: string } = {
      '0': 'Soltero',
      '1': 'Casado',
      '2': 'Divorciado',
      '3': 'Viudo',
      '4': 'Union Libre'
    };
    // @ts-ignore
    return estadoCivil !== null ? estadosCiviles[estadoCivil] || 'No especificado' : 'No especificado';
  }
  reactivarCliente(clienteId: number): void {
    this.clienteService.reactivarCliente(clienteId).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Cliente reactivado correctamente.'
        });
        this.cargarClientes(); // Recargar la lista de clientes
      },
      error: (err) => {
        console.error('Error al reactivar cliente:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo reactivar el cliente.'
        });
      }
    });
  }

  clear(table: Table): void {
    table.clear();
    this.searchValue = '';
  }

}
