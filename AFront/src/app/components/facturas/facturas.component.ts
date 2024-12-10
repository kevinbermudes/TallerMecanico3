import { Component, OnInit } from '@angular/core';
import { FacturaService } from '../../Service/factura.service';
import { AuthService } from '../../Service/auth.service';
import {EstadoFactura, Factura} from '../../Entity/Factura';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import {FormsModule} from '@angular/forms';
import {InputTextModule} from 'primeng/inputtext';

@Component({
  selector: 'app-facturas',
  templateUrl: './facturas.component.html',
  styleUrls: ['./facturas.component.css'],
  providers: [MessageService],
  imports: [CommonModule, TableModule, ToastModule, DialogModule, ButtonModule, FormsModule, InputTextModule],
  standalone: true
})
export class FacturasComponent implements OnInit {
  facturas: Factura[] = [];
  clienteId!: number;
  searchValue: string = '';
  mostrarDialogo: boolean = false;
  facturaSeleccionada: Factura | null = null;

  constructor(
    private facturaService: FacturaService,
    private authService: AuthService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;
      this.cargarFacturas();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar el cliente. Intenta iniciar sesión nuevamente.',
      });
    }
  }

  cargarFacturas(): void {
    this.facturaService.getFacturasByClienteId(this.clienteId).subscribe({
      next: (facturas) => {
        this.facturas = facturas;
      },
      error: (err) => {
        console.error('Error al cargar las facturas:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar las facturas.',
        });
      },
    });
  }

  verDetalles(factura: Factura): void {
    this.facturaSeleccionada = factura;
    this.mostrarDialogo = true;
  }

  protected readonly EstadoFactura = EstadoFactura;
  protected readonly HTMLInputElement = HTMLInputElement;
}
