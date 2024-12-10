import { Component, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Servicio } from '../../../Entity/Servicio';
import { ServicioService } from '../../../Service/servicio.service';
import {CurrencyPipe, NgForOf, NgIf} from '@angular/common';
import { AuthService } from '../../../Service/auth.service';
import { CarritoService } from '../../../Service/carrito.service';

@Component({
  selector: 'app-catalogo-servicios',
  standalone: true,
  imports: [CardModule, ButtonModule, ToastModule, NgForOf, CurrencyPipe, NgIf],
  templateUrl: './catalogo-servicios.component.html',
  styleUrls: ['./catalogo-servicios.component.css'],
  providers: [MessageService]
})
export class CatalogoServiciosComponent implements OnInit {
  servicios: Servicio[] = [];
  serviciosContratados: number[] = [];
  constructor(
    private authService: AuthService,
    private servicioService: ServicioService,
    private carritoService: CarritoService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.cargarCatalogo();
  }

  cargarCatalogo(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      const clienteId = clienteData.id;

      // Obtener servicios contratados por el cliente
      this.servicioService.getServiciosByClienteId(clienteId).subscribe({
        next: (contratados) => {
          this.serviciosContratados = contratados.map((servicio) => servicio.id); // IDs de servicios contratados

          // Cargar el catálogo de servicios
          this.servicioService.getAllServicios().subscribe({
            next: (data) => {
              // Filtrar servicios para excluir los eliminados
              this.servicios = data.filter((servicio) => !servicio.estaBorrado);
            },
            error: (err) => {
              console.error('Error al cargar el catálogo:', err);
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: 'No se pudo cargar el catálogo.'
              });
            }
          });
        },
        error: (err) => {
          console.error('Error al obtener servicios contratados:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo verificar los servicios contratados.'
          });
        }
      });
    }
  }



  contratarServicio(servicioId: number): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      const clienteId = clienteData.id;

      this.carritoService.agregarAlCarritoservicio({ clienteId, servicioId }).subscribe({
        next: () => {
          console.log('Servicio agregado al carrito');

          this.messageService.add({
            severity: 'success',
            summary: 'Servicio Agregado',
            detail: 'El servicio ha sido agregado al carrito exitosamente.'
          });
        },
        error: (err) => {
          console.error('Error al agregar el servicio al carrito:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo agregar el servicio al carrito.'
          });
        }
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Debes iniciar sesión para agregar un servicio al carrito.'
      });
    }
  }

  getImagenUrl(servicio: Servicio): string {
    return servicio.imagen || 'https://via.placeholder.com/150';
  }
}
