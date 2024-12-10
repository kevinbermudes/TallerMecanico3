import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ServicioService } from '../../../Service/servicio.service';
import { Servicio } from '../../../Entity/Servicio';
import { ButtonDirective } from 'primeng/button';
import { TableModule } from 'primeng/table';
import {CurrencyPipe, NgIf} from '@angular/common';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule } from '@angular/forms';
import { InputTextareaModule } from 'primeng/inputtextarea';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-servicios-admin',
  templateUrl: './servicios-admin.component.html',
  styleUrls: ['./servicios-admin.component.css'],
  providers: [MessageService],
  imports: [
    ButtonDirective,
    TableModule,
    CurrencyPipe,
    DialogModule,
    InputTextModule,
    FormsModule,
    InputTextareaModule,
    ToastModule,
    NgIf,
  ],
  standalone: true,
})
export class ServiciosAdminComponent implements OnInit {
  servicios: Servicio[] = [];
  servicioSeleccionado: Servicio | null = null;
  mostrarDialogo: boolean = false;
  creando: boolean = false;
  imagenSeleccionada: File | null = null;
  nuevoServicio: Partial<Servicio> = {
    nombre: '',
    descripcion: '',
    precio: null,
    imagen: '',
  };

  constructor(
    private servicioService: ServicioService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.cargarServicios();
  }

  cargarServicios(): void {
    this.servicioService.getAllServicios().subscribe({
      next: (servicios) => (this.servicios = servicios),
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los servicios.',
        }),
    });
  }

  abrirDialogoCrear(): void {
    this.creando = true;
    this.nuevoServicio = {
      nombre: '',
      descripcion: '',
      precio: null,
      imagen: '',
    };
    this.imagenSeleccionada = null; // Limpia la imagen seleccionada
    this.servicioSeleccionado = null; // Limpia cualquier selección previa
    this.mostrarDialogo = true;
  }


  abrirDialogoEditar(servicio: Servicio): void {
    this.creando = false;
    this.servicioSeleccionado = { ...servicio };
    this.nuevoServicio = { ...servicio };
    this.mostrarDialogo = true;
  }

  guardarServicio(): void {
    if (this.creando) {
      // Crear el FormData manualmente
      const formData = new FormData();
      formData.append('Nombre', this.nuevoServicio.nombre || '');
      formData.append('Descripcion', this.nuevoServicio.descripcion || '');
      formData.append('Precio', this.nuevoServicio.precio?.toString() || '0');
      if (this.imagenSeleccionada) {
        formData.append('Imagen', this.imagenSeleccionada);
      }

      // Llamar al servicio con el FormData creado
      this.servicioService.createServicio(formData).subscribe({
        next: (servicio) => {
          this.servicios.push(servicio);
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Servicio creado correctamente.',
          });
          this.mostrarDialogo = false;
        },
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo crear el servicio.',
          }),
      });
    } else {
      // Actualizar servicio
      this.servicioService.updateServicio(this.servicioSeleccionado!.id!, this.nuevoServicio as Servicio).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Servicio actualizado correctamente.',
          });
          this.cargarServicios();
          this.mostrarDialogo = false;
        },
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo actualizar el servicio.',
          }),
      });

    }
  }


  eliminarServicio(id: number): void {
    this.servicioService.deleteServicio(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Servicio eliminado correctamente.',
        });
        this.cargarServicios();
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo eliminar el servicio.',
        }),
    });
  }

  reactivarServicio(id: number): void {
    this.servicioService.reactivarServicio(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Servicio reactivado correctamente.',
        });
        this.cargarServicios();
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo reactivar el servicio.',
        }),
    });
  }

  cerrarDialogo(): void {
    this.mostrarDialogo = false;
    this.imagenSeleccionada = null;
  }

  onImagenSeleccionada(event: any): void {
    this.imagenSeleccionada = event.target.files[0];
  }

  getImagenUrl(servicio: Partial<Servicio>): string {
    return servicio.imagen || 'https://via.placeholder.com/150';
  }
}
