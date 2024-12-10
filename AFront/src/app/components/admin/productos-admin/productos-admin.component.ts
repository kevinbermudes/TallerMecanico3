import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ProductoService } from '../../../Service/producto.service';
import { Producto } from '../../../Entity/Producto';
import {ButtonDirective} from 'primeng/button';
import {CurrencyPipe, NgIf} from '@angular/common';
import {TableModule} from 'primeng/table';
import {ToastModule} from 'primeng/toast';
import {DialogModule} from 'primeng/dialog';
import {FormsModule} from '@angular/forms';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {InputTextModule} from 'primeng/inputtext';

@Component({
  selector: 'app-productos-admin',
  templateUrl: './productos-admin.component.html',
  styleUrls: ['./productos-admin.component.css'],
  standalone: true,
  imports: [
    ButtonDirective,
    CurrencyPipe,
    TableModule,
    NgIf,
    ToastModule,
    DialogModule,
    FormsModule,
    InputTextareaModule,
    InputTextModule
  ],
  providers: [MessageService]
})
export class ProductosAdminComponent implements OnInit {
  productos: Producto[] = [];
  productoSeleccionado: Producto | null = null;
  mostrarDialogo: boolean = false;
  creando: boolean = false;
  imagenSeleccionada: File | null = null;
  imagenPrevisualizacion: string | ArrayBuffer | null = null;

  nuevoProducto: Partial<Producto> = {
    nombre: '',
    descripcion: '',
    precio: null,
    stock: null,
    imagen: '',
  };

  constructor(
    private productoService: ProductoService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.cargarProductos();
  }

  cargarProductos(): void {
    this.productoService.getProductos().subscribe({
      next: (productos) => (this.productos = productos),
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los productos.',
        }),
    });
  }

  abrirDialogoCrear(): void {
    this.creando = true;
    this.nuevoProducto = {
      nombre: '',
      descripcion: '',
      precio: null,
      stock: null,
      imagen: '',
    };
    this.imagenSeleccionada = null;
    this.imagenPrevisualizacion = null;
    this.mostrarDialogo = true;
  }


  abrirDialogoEditar(producto: Producto): void {
    this.creando = false;
    this.productoSeleccionado = { ...producto };
    this.nuevoProducto = { ...producto };
    this.imagenSeleccionada = null;
    this.imagenPrevisualizacion = null;
    this.mostrarDialogo = true;
  }


  guardarProducto(): void {
    const formData = new FormData();

    if (this.imagenSeleccionada) {
      formData.append('imagen', this.imagenSeleccionada, this.imagenSeleccionada.name);
    }

    formData.append('nombre', this.nuevoProducto.nombre!);
    formData.append('descripcion', this.nuevoProducto.descripcion!);
    formData.append('precio', this.nuevoProducto.precio!.toString());
    formData.append('stock', this.nuevoProducto.stock!.toString());

    if (this.creando) {
      this.productoService.createProducto(formData).subscribe({
        next: (producto) => {
          this.productos.push(producto);
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Producto creado correctamente.',
          });
          this.mostrarDialogo = false;
        },
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo crear el producto.',
          }),
      });
    } else {
      this.productoService.updateProducto(this.productoSeleccionado!.id, formData).subscribe({
        next: () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Producto actualizado correctamente.',
          });
          this.cargarProductos();
          this.mostrarDialogo = false;
        },
        error: () =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo actualizar el producto.',
          }),
      });
    }
  }

  eliminarProducto(id: number): void {
    this.productoService.deleteProducto(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Producto eliminado correctamente.',
        });
        this.cargarProductos();
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo eliminar el producto.',
        }),
    });
  }

  reactivarProducto(id: number): void {
    this.productoService.reactivarProducto(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Producto reactivado correctamente.',
        });
        this.cargarProductos();
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo reactivar el producto.',
        }),
    });
  }

  onImagenSeleccionada(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.imagenSeleccionada = file;

      // Crear la previsualización
      const reader = new FileReader();
      reader.onload = () => {
        this.imagenPrevisualizacion = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }

  getImagenUrl(producto: Partial<Producto>): string {
    return producto.imagen || 'https://via.placeholder.com/150';
  }
  cerrarDialogoCrearEditar(): void {
    this.mostrarDialogo = false;
    this.imagenSeleccionada = null;
    this.imagenPrevisualizacion = null;
    this.nuevoProducto = {
      nombre: '',
      descripcion: '',
      precio: null,
      stock: null,
      imagen: '',
    };
  }



}
