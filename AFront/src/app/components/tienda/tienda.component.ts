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

  // Variables para el diálogo
  displayDialog: boolean = false;
  selectedProduct: Producto | null = null;
  newImageFile: File | null = null;
  previewImage: string | ArrayBuffer | null | undefined = null;
  uploadProgress: number = 0;
  uploadError: string = '';

  constructor(
    private carritoService: CarritoService,
    private productoService: ProductoService,
    private router: Router,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.productoService.getProductos().subscribe((data) => {
      this.productos = data;
    });
  }

  getCarritos(carritos: { $values: Carrito[] } | Carrito[]): Carrito[] {
    return Array.isArray(carritos) ? carritos : carritos.$values;
  }

  navigateToDetails(productId: number): void {
    this.router.navigate(['/producto', productId]);
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


  // Método para abrir el diálogo de cambio de imagen
  openChangeImageDialog(producto: Producto): void {
    this.selectedProduct = producto;
    this.displayDialog = true;
    this.newImageFile = null;
    this.previewImage = null;
    this.uploadProgress = 0;
    this.uploadError = '';
  }

  // Método para manejar la selección de archivo dentro del diálogo
  onFileSelected(event: any): void {
    if (event.target.files && event.target.files.length > 0) {
      const file = event.target.files[0];
      const tiposPermitidos = ['image/jpeg', 'image/png', 'image/gif'];
      const maxSize = 5 * 1024 * 1024; // 5 MB

      if (!tiposPermitidos.includes(file.type)) {
        this.messageService.add({severity:'error', summary: 'Error', detail: 'Tipo de archivo no permitido. Solo se permiten imágenes (jpg, jpeg, png, gif).' });
        return;
      }

      if (file.size > maxSize) {
        this.messageService.add({severity:'error', summary: 'Error', detail: 'El archivo es demasiado grande. El tamaño máximo permitido es 5 MB.' });
        return;
      }

      this.newImageFile = file;

      // Generar una vista previa de la imagen
      const reader = new FileReader();
      reader.onload = (e) => {
        this.previewImage = e.target?.result;
      };
      reader.readAsDataURL(file);
    }
  }

  // Método para subir la nueva imagen
  uploadImagen(): void {
    if (!this.selectedProduct || !this.newImageFile) {
      this.uploadError = 'Producto o archivo no seleccionado.';
      return;
    }

    this.uploadProgress = 0;
    this.uploadError = '';

    this.productoService.uploadImagen(this.selectedProduct.id, this.newImageFile).subscribe({
      next: (response) => {
        if (response.Url) {
          const productoIndex = this.productos.findIndex(p => p.id === this.selectedProduct?.id);
          if (productoIndex !== -1) {
            this.productos[productoIndex].imagen = response.Url;
          }
          this.messageService.add({severity:'success', summary: 'Éxito', detail: 'Imagen subida exitosamente.' });
          this.displayDialog = false;
        }
      },
      error: (error) => {
        console.error('Error al subir la imagen:', error);
        this.uploadError = 'Error al subir la imagen. Intenta nuevamente.';
        this.messageService.add({severity:'error', summary: 'Error', detail: 'Error al subir la imagen. Intenta nuevamente.' });
      }
    });
  }

  // Método para cerrar el diálogo
  closeDialog(): void {
    this.displayDialog = false;
  }

  // Método para obtener la URL de la imagen
  getImagenUrl(producto: Producto): string {
    return producto.imagen || 'https://via.placeholder.com/150';
  }

}
