import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../../Service/carrito.service';
import { AuthService } from '../../Service/auth.service';
import { Carrito } from '../../Entity/Carrito';
import { CommonModule, NgIf, NgForOf } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-carrito',
  standalone: true,
  imports: [
    CommonModule,
    NgIf,
    NgForOf,
    TableModule,
    ButtonModule,
    ToastModule
  ],
  templateUrl: './carrito.component.html',
  styleUrls: ['./carrito.component.css'],
  providers: [MessageService]
})
export class CarritoComponent implements OnInit {
  carritoProductos: Carrito[] = [];
  clienteId: number | null = null;
  totalItems: number = 0;
  totalPrecio: number = 0;

  constructor(
    private carritoService: CarritoService,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;
      this.cargarCarrito();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar el carrito. Intenta iniciar sesión nuevamente.'
      });
    }
  }

  cargarCarrito(): void {
    if (this.clienteId) {
      this.carritoService.getCarritosByClienteId(this.clienteId).subscribe(
        (response) => {
          // Manejar el caso donde la respuesta tenga la propiedad $values
          const carritoArray = Array.isArray(response) ? response : response.$values || [];

          // Filtrar productos donde estaBorrado es false
          this.carritoProductos = carritoArray.filter((item) => item.estaBorrado === false);
          this.calcularResumen(); // Calcular el resumen después de cargar el carrito
        },
        (error) => {
          console.error('Error al cargar el carrito:', error);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo cargar el carrito.'
          });
        }
      );
    }
  }


  eliminarProducto(carritoId: number): void {
    this.carritoService.deleteCarrito(carritoId).subscribe(
      () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Producto eliminado del carrito.'
        });
        this.cargarCarrito(); // Recargar el carrito después de eliminar
      },
      (error) => {
        console.error('Error al eliminar producto del carrito:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo eliminar el producto del carrito.'
        });
      }
    );
  }


  calcularResumen(): void {
    this.totalItems = this.carritoProductos.reduce((sum, item) => sum + item.cantidad, 0);
    this.totalPrecio = this.carritoProductos.reduce((sum, item) => sum + item.precioTotal, 0);
  }


  pagarCarrito(): void {
    this.messageService.add({
      severity: 'success',
      summary: 'Pago realizado',
      detail: 'El pago del carrito se ha completado.'
    });
    // Aquí puedes añadir la lógica de pago real, como redirigir a una pasarela de pago.
  }
}
