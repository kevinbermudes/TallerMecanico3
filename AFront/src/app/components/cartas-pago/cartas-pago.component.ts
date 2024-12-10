import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { CartaPago } from '../../Entity/CartaPago';
import { CartaPagoService } from '../../Service/carta-pago.service';
import { AuthService } from '../../Service/auth.service';
import {TableModule} from 'primeng/table';
import {CurrencyPipe, DatePipe} from '@angular/common';
import {ButtonDirective} from 'primeng/button';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-cartas-pago',
  templateUrl: './cartas-pago.component.html',
  styleUrls: ['./cartas-pago.component.css'],
  standalone: true,
  imports: [
    TableModule,
    CurrencyPipe,
    DatePipe,
    ButtonDirective,
    ToastModule
  ],
  providers: [MessageService]
})
export class CartasPagoComponent implements OnInit {
  cartasPago: CartaPago[] = [];
  clienteId!: number;

  constructor(
    private cartaPagoService: CartaPagoService,
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;
      this.cargarCartasPago();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar el cliente. Intenta iniciar sesión nuevamente.',
      });
    }
  }

  cargarCartasPago(): void {
    this.cartaPagoService.getCartasPagoByClienteId(this.clienteId).subscribe({
      next: (cartasPago) => {
        if (Array.isArray(cartasPago) && cartasPago.length > 0) {
          this.cartasPago = cartasPago;
        } else {
          this.cartasPago = [];
          this.messageService.add({
            severity: 'info',
            summary: 'Sin cartas de pago',
            detail: 'No se encontraron cartas de pago.',
          });
        }
      },
      error: (err) => {
        console.error('Error al cargar las cartas de pago:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar las cartas de pago.',
        });
      },
    });
  }

  verFacturas(cartaPagoId: number): void {
    this.router.navigate([`/cliente/cartas-de-pago/${cartaPagoId}/facturas`]);
  }
}
