import { Component, OnInit } from '@angular/core';
import { ServicioService } from '../../Service/servicio.service';
import { Servicio } from '../../Entity/Servicio';
import { AuthService } from '../../Service/auth.service';
import { MessageService } from 'primeng/api';
import { CardModule } from 'primeng/card';
import { CurrencyPipe, NgForOf, NgIf } from '@angular/common';
import {ButtonDirective} from 'primeng/button';
import {Router} from '@angular/router';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-servicios',
  templateUrl: './servicios.component.html',
  styleUrls: ['./servicios.component.css'],
  standalone: true,
  imports: [
    CardModule,
    NgForOf,
    NgIf,
    CurrencyPipe,
    ButtonDirective,
    ToastModule
  ],
  providers: [MessageService]
})
export class ServiciosComponent implements OnInit {
  serviciosContratados: Servicio[] = [];
  clienteId: number | null = null;

  constructor(
    private router: Router,
    private servicioService: ServicioService,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;
      this.cargarServiciosContratados();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar los servicios. Intenta iniciar sesión nuevamente.'
      });
    }
  }

  cargarServiciosContratados(): void {
    if (this.clienteId) {
      this.servicioService.getServiciosByClienteId(this.clienteId).subscribe({
        next: (data) => {
          this.serviciosContratados = data.filter((servicio) => !servicio.estaBorrado);
        },
        error: (err) => {
          console.error('Error al cargar servicios contratados:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudieron cargar tus servicios contratados.'
          });
        }
      });
    }
  }

  getImagenUrl(servicio: Servicio): string {
    return servicio.imagen || 'https://via.placeholder.com/150';
  }

  // Nuevo método para quitar el servicio
  quitarServicio(servicioId: number): void {

    if (this.clienteId) {
      this.servicioService.quitarServicioDeCliente(servicioId, this.clienteId).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Servicio Eliminado',
            detail: 'El servicio ha sido eliminado de tus servicios contratados.'
          });
          // Actualizar la lista de servicios contratados
          this.serviciosContratados = this.serviciosContratados.filter(s => s.id !== servicioId);
        },
        error: (err) => {
          console.error('Error al eliminar el servicio:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo eliminar el servicio.'
          });
        }
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo identificar al cliente. Intenta iniciar sesión nuevamente.'
      });
    }

  }
  irACatalogo(): void {
    this.router.navigate(['/cliente/catalogo-servicios']);
  }
}
