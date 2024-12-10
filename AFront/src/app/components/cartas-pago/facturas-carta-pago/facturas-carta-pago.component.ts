import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Factura, EstadoFactura } from '../../../Entity/Factura';
import { CartaPagoService } from '../../../Service/carta-pago.service';
import { MessageService } from 'primeng/api';
import { SharedMessageService } from '../../../Service/shared-message.service';
import {ButtonDirective} from 'primeng/button';
import {TableModule} from 'primeng/table';
import {CurrencyPipe, DatePipe, NgForOf} from '@angular/common';
import {DialogModule} from 'primeng/dialog';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-facturas-carta-pago',
  templateUrl: './facturas-carta-pago.component.html',
  styleUrls: ['./facturas-carta-pago.component.css'],
  providers: [MessageService],
  imports: [
    ButtonDirective,
    TableModule,
    CurrencyPipe,
    DatePipe,
    DialogModule,
    NgForOf,
    ToastModule
  ],
  standalone: true
})
export class FacturasCartaPagoComponent implements OnInit {
  cartaPagoId!: number;
  facturas: Factura[] = [];
  facturaSeleccionada: Factura | null = null;
  mostrarDialogo: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartaPagoService: CartaPagoService,
    private messageService: MessageService,
    private sharedMessageService: SharedMessageService
  ) {}

  ngOnInit(): void {
    this.cartaPagoId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.cartaPagoId) {
      this.cargarFacturas();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se proporcionó un ID de carta de pago.',
      });
    }
  }

  cargarFacturas(): void {
    this.cartaPagoService.getFacturasByCartaPagoId(this.cartaPagoId).subscribe({
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

  getNombreEstado(estado: number | null): string {
    switch (estado) {
      case EstadoFactura.Pendiente:
        return 'Pendiente';
      case EstadoFactura.Pagada:
        return 'Pagada';
      default:
        return 'Desconocido';
    }
  }

  verDetalles(factura: Factura): void {
    this.facturaSeleccionada = factura;
    this.mostrarDialogo = true;
  }

  pagarFactura(factura: Factura): void {
    const total = factura.total ?? 0;
    this.sharedMessageService.setTotalAmount(total);
    this.sharedMessageService.setFacturaId(factura.id ?? null);
    this.sharedMessageService.setFacturaIds([]); // Limpiar facturaIds
    this.router.navigate(['/cliente/payment']);
  }

  pagarTodas(): void {
    const totalAmount = this.facturas.reduce((sum, factura) => sum + (factura.total ?? 0), 0);

    const facturasPendientes = this.facturas
      .filter(factura => factura.estado === EstadoFactura.Pendiente)
      .map(factura => factura.id!)
      .filter(id => id !== undefined);

    this.sharedMessageService.setTotalAmount(totalAmount);
    this.sharedMessageService.setFacturaIds(facturasPendientes);
    this.sharedMessageService.setFacturaId(null); // Limpiar facturaId
    this.router.navigate(['/cliente/payment']);
  }




  volver(): void {
    this.router.navigate(['/cliente/cartas-de-pago']);
  }
}
