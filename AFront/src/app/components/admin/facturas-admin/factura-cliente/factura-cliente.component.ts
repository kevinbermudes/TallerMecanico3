import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import {EstadoFactura, Factura} from '../../../../Entity/Factura';
import {FacturaService} from '../../../../Service/factura.service';
import {TableModule} from 'primeng/table';
import {CurrencyPipe, DatePipe, NgClass, NgIf} from '@angular/common';
import {DialogModule} from 'primeng/dialog';
import {ButtonDirective} from 'primeng/button';
import {InputTextModule} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {DropdownModule} from 'primeng/dropdown';
import {InputTextareaModule} from 'primeng/inputtextarea';
import {CalendarModule} from 'primeng/calendar';
import {ToastModule} from 'primeng/toast';

@Component({
  selector: 'app-factura-cliente',
  templateUrl: './factura-cliente.component.html',
  styleUrls: ['./factura-cliente.component.css'],
  standalone: true,
  imports: [
    TableModule,
    DatePipe,
    CurrencyPipe,
    DialogModule,
    ButtonDirective,
    InputTextModule,
    FormsModule,
    DropdownModule,
    InputTextareaModule,
    NgIf,
    NgClass,
    CalendarModule,
    ToastModule
  ],
  providers: [MessageService]
})
export class FacturaClienteComponent implements OnInit {
  clienteId: number = 0;
  facturas: Factura[] = [];
  mostrarDialogo: boolean = false;
  searchValue: string = '';

  nuevaFactura: Partial<Factura> = {
    total: null,
    estado: null,
    comentarios: ''
  };
  mostrarDetalles: boolean = false;
  facturaSeleccionada: Factura | null = null;
  estadosFactura = [
    { label: 'Pendiente', value: 'Pendiente' },
    { label: 'Pagada', value: 'Pagada' },
  ];
  filtroEstado: number | null = null;
  filtroFecha: Date | null = null;
  facturasOriginales: Factura[] = [];

  constructor(
    private route: ActivatedRoute,
    private facturaService: FacturaService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.clienteId = +params['id'];
      this.cargarFacturas();
    });
  }

  cargarFacturas(): void {
    this.facturaService.getFacturasByClienteId(this.clienteId).subscribe({
      next: (facturas) => {
        this.facturasOriginales = facturas;
        this.facturas = [...this.facturasOriginales];
        this.filtroEstado = null;
        this.filtroFecha = null;
      },
      error: () =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar las facturas del cliente.'
        })
    });
  }




  mostrarDialogoCrearFactura(): void {
    this.nuevaFactura = { total: null, estado: null };
    this.mostrarDialogo = true;
  }

  crearFactura(): void {
    if (!this.nuevaFactura.total || !this.nuevaFactura.estado) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Datos incompletos',
        detail: 'Por favor, complete todos los campos para crear la factura.'
      });
      return;
    }

    // Crear factura con los datos necesarios
    const factura: Factura = {
      total: this.nuevaFactura.total!,
      estado: EstadoFactura[this.nuevaFactura.estado! as unknown as keyof typeof EstadoFactura],
      clienteId: this.clienteId,
      fechaCreacion: new Date(),
      fechaVencimiento: new Date(new Date().setDate(new Date().getDate() + 30)),
      codigoFactura: this.nuevaFactura.codigoFactura || "",
      comentarios: this.nuevaFactura.comentarios || "",
      cartasPago: [],
      productosFactura: [],
      serviciosFactura: []
    };



    this.facturaService.createFactura(factura).subscribe({
      next: (nuevaFactura) => {
        this.facturas.push(nuevaFactura);
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Factura creada correctamente.'
        });
        this.mostrarDialogo = false;
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo crear la factura.'
        });
      }
    });
  }
  getEstadoNombre(estado: number | null): string {
    if (estado === null) return 'Desconocido';
    return estado === EstadoFactura.Pagada ? 'Pagada' : 'Pendiente';
  }
  cerrarDialogoCrearFactura(): void {
    this.mostrarDialogo = false;
  }
  verDetallesFactura(factura: Factura): void {
    this.facturaSeleccionada = factura;
    this.mostrarDetalles = true;
  }
  get facturasPendientes(): number {
    return this.facturas.filter((factura) => factura.estado === EstadoFactura.Pendiente).length;
  }
  aplicarFiltroEstado(): void {
    this.aplicarFiltros();
  }
  aplicarFiltroFecha(): void {
    this.aplicarFiltros();
  }
  aplicarFiltros(): void {
    let facturasFiltradas = [...this.facturasOriginales]; // Copia de la lista original

    // Filtro por fecha
    if (this.filtroFecha) {
      const fechaSeleccionada = this.obtenerFechaLocal(this.filtroFecha); // Fecha seleccionada en formato local
      facturasFiltradas = facturasFiltradas.filter((factura) => {
        const fechaFactura = this.obtenerFechaLocal(factura.fechaCreacion); // Fecha de la factura en formato local
        return fechaFactura === fechaSeleccionada;
      });
    }

    // Filtro por estado
    if (this.filtroEstado !== null) {
      facturasFiltradas = facturasFiltradas.filter((factura) => factura.estado === this.filtroEstado);
    }

    // Actualiza la lista visible
    this.facturas = facturasFiltradas;
  }




  private obtenerFechaLocal(fecha: string | Date): string {
    const date = typeof fecha === 'string' ? new Date(fecha) : fecha;
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); // Meses son base 0
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`; // Formato yyyy-MM-dd
  }
  clear(table: any): void {
    table.clear();
    this.searchValue = '';
  }
  clearFiltroFecha(): void {
    this.filtroFecha = null; // Restablece el filtro de fecha
    this.aplicarFiltros(); // Actualiza la lista
  }
  clearFiltroEstado(): void {
    this.filtroEstado = null; // Restablece el filtro de estado
    this.aplicarFiltros(); // Actualiza la lista
  }
  protected readonly EstadoFactura = EstadoFactura;
}
