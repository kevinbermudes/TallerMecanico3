// tienda.component.ts
import { Component, OnInit } from '@angular/core';
import { ProductoService } from '../../Service/producto.service';
import { Producto } from '../../Entity/Producto';
import { CardModule } from 'primeng/card';
import { CommonModule, NgForOf, NgIf } from '@angular/common';
import { Button, ButtonDirective } from 'primeng/button';
import { Carrito } from '../../Entity/Carrito';
import { Router } from '@angular/router';
import { FileUploadModule } from 'primeng/fileupload';
import { DialogModule } from 'primeng/dialog';
import { ProgressBarModule } from 'primeng/progressbar';
import { MessageService } from 'primeng/api';
import {ToastModule} from 'primeng/toast';
import {CarritoService} from '../../Service/carrito.service';
import {AuthService} from '../../Service/auth.service';

@Component({
  selector: 'app-tienda',
  standalone: true,
  imports: [
    CardModule,
    NgForOf,
    ButtonDirective,
    NgIf,
    CommonModule,
    Button,
    FileUploadModule,
    DialogModule,
    ProgressBarModule,
    ToastModule
  ],
  templateUrl: './tienda.component.html',
  styleUrls: ['./tienda.component.css'],
  providers: [MessageService]
})
export class TiendaComponent implements OnInit {
  productos: Producto[] = [];
  productosFiltrados: Producto[] = [];

  // Variables para el diálogo de detalles
  mostrarDialogoDetalles: boolean = false;
  productoSeleccionado: Producto | null = null;

  constructor(
    private carritoService: CarritoService,
    private productoService: ProductoService,
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.productoService.getProductos().subscribe({
      next: (productos) => {
        this.productos = productos;
        this.productosFiltrados = productos.filter((producto) => !producto.estaBorrado);
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los productos.',
        });
      },
    });
  }

  agregarAlCarrito(producto: Producto): void {
    const clienteData = this.authService.getClienteData();
    if (!clienteData) {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo obtener la información del cliente.' });
      return;
    }

    const data = {
      clienteId: clienteData.id, // Asegúrate de que `getClienteData` devuelva el ID del cliente.
      productoId: producto.id,
      cantidad: 1, // Puedes implementar una lógica para seleccionar la cantidad.
    };

    this.carritoService.agregarAlCarrito(data).subscribe(
      (response) => {
        this.messageService.add({ severity: 'success', summary: 'Éxito', detail: 'Producto agregado al carrito.' });
      },
      (error) => {
        console.error('Error al agregar producto al carrito:', error);
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'No se pudo agregar el producto al carrito.' });
      }
    );
  }

  abrirDialogoDetalles(producto: Producto): void {
    this.productoSeleccionado = producto;
    this.mostrarDialogoDetalles = true;
  }

  cerrarDialogoDetalles(): void {
    this.mostrarDialogoDetalles = false;
    this.productoSeleccionado = null;
  }

  getImagenUrl(producto: Producto | null): string {
    return producto?.imagen || 'https://via.placeholder.com/150';
  }
}
