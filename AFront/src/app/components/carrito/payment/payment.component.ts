import { Component, OnInit } from '@angular/core';
import { Stripe, loadStripe, StripeCardElement } from '@stripe/stripe-js';
import { MessageService } from 'primeng/api';
import { CarritoService } from '../../../Service/carrito.service';
import { Router } from '@angular/router';
import { SharedMessageService } from '../../../Service/shared-message.service';
import { AuthService } from '../../../Service/auth.service';
import {CurrencyPipe, NgIf} from '@angular/common';
import {ButtonDirective} from 'primeng/button';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
  providers: [MessageService],
  imports: [
    CurrencyPipe,
    ButtonDirective,
    ToastModule,
    NgIf
  ],
  standalone: true
})
export class PaymentComponent implements OnInit {
  stripe!: Stripe | null;
  cardElement!: StripeCardElement;
  totalAmount: number = 0;
  clienteId!: number;
  facturaId: number | null = null;
  clientSecret!: string;
  isProcessing: boolean = false;

  constructor(
    private sharedMessageService: SharedMessageService,
    private router: Router,
    private carritoService: CarritoService,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  async ngOnInit(): Promise<void> {
    this.stripe = await loadStripe('pk_test_51QONQHG2c2GcAZ6KPkPqVp2FzRj5VKNbMhc4ZHnSZAu0yKXG0W903e7ccy3e4FYHZFTLcq0D5Z0sDhCMo3oDU0Dp00sBgChW6c');

    if (this.stripe) {
      const elements = this.stripe.elements();
      this.cardElement = elements.create('card');
      this.cardElement.mount('#card-element');
    }

    this.sharedMessageService.totalAmount$.subscribe((amount) => {
      this.totalAmount = amount;
    });

    this.sharedMessageService.facturaId$.subscribe((facturaId) => {
      this.facturaId = facturaId;
    });

    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;

      const amountInCents = Math.round(this.totalAmount * 100);
      this.carritoService.createPaymentIntent(amountInCents, 'usd').subscribe({
        next: (response) => {
          this.clientSecret = response.clientSecret;
        },
        error: (err) => {
          console.error('Error al crear el intento de pago:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudo crear el intento de pago.',
          });
        },
      });
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar el cliente. Intenta iniciar sesión nuevamente.',
      });
    }
  }

  async pay(): Promise<void> {
    if (!this.stripe || !this.cardElement) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Stripe no se inicializó correctamente.',
      });
      return;
    }

    if (this.isProcessing) {
      return; // Evitar múltiples llamadas
    }

    this.isProcessing = true;

    try {
      const { paymentIntent, error } = await this.stripe.confirmCardPayment(
        this.clientSecret,
        {
          payment_method: {
            card: this.cardElement,
          },
        }
      );

      if (error) {
        throw new Error(error.message);
      }

      if (paymentIntent && paymentIntent.status === 'succeeded') {
        // Determinar si es un solo pago o varios
        const facturaIds = this.sharedMessageService.getFacturaIds();
        const facturaId = this.facturaId;

        this.carritoService
          .confirmarPago({
            clienteId: this.clienteId,
            paymentIntentId: paymentIntent.id,
            facturaId: facturaIds && facturaIds.length === 1 ? facturaIds[0] : facturaId,
            facturaIds: facturaIds && facturaIds.length > 1 ? facturaIds : null,
          })
          .subscribe({
            next: () => {
              this.messageService.add({
                severity: 'success',
                summary: 'Pago exitoso',
                detail: 'El pago se procesó correctamente.',
              });
              this.router.navigate(['/cliente/facturas']);
            },
            error: (err) => {
              console.error('Error al confirmar el pago en el servidor:', err);
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: 'No se pudo confirmar el pago en el servidor.',
              });
            },
          });
      }
    } catch (err: any) {
      console.error('Error al realizar el pago:', err.message);
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: err.message,
      });
    } finally {
      this.isProcessing = false;
    }
  }

  async cancel(): Promise<void> {
    this.sharedMessageService.sendMessage({
      severity: 'info',
      summary: 'Info',
      detail: 'El pago fue cancelado.',
    });

    this.router.navigate(['/cliente/carrito']);
  }
}
