import { Component, OnInit } from '@angular/core';
import {FacturaService} from '../../../Service/factura.service';
import {Factura} from '../../../Entity/Factura';
import {CardModule} from 'primeng/card';
import {ChartModule} from 'primeng/chart';

@Component({
  selector: 'app-reportes',
  templateUrl: './reportes-admin.component.html',
  styleUrls: ['./reportes-admin.component.css'],
  imports: [
    CardModule,
    ChartModule
  ],
  standalone: true
})
export class ReportesAdminComponent  implements OnInit {
  ventasPorMes: any;
  estadoFacturas: any;
  clientesFacturacion: any;
  chartOptions: any;

  constructor(private facturaService: FacturaService) {}

  ngOnInit(): void {
    this.chartOptions = {
      responsive: true,
      plugins: {
        legend: { position: 'top' },
        tooltip: { enabled: true },
      },
    };

    this.cargarDatos();
  }

  cargarDatos(): void {
    this.facturaService.getAllFacturas().subscribe((facturas) => {
      this.generarGraficoVentasPorMes(facturas);
      this.generarGraficoEstadoFacturas(facturas);
      this.generarGraficoClientesFacturacion(facturas);
    });
  }

  generarGraficoVentasPorMes(facturas: Factura[]): void {
    const meses = [
      'Enero',
      'Febrero',
      'Marzo',
      'Abril',
      'Mayo',
      'Junio',
      'Julio',
      'Agosto',
      'Septiembre',
      'Octubre',
      'Noviembre',
      'Diciembre',
    ];

    const totalesPorMes = new Array(12).fill(0);

    facturas.forEach((factura) => {
      const mes = new Date(factura.fechaCreacion).getMonth();
      totalesPorMes[mes] += factura.total;
    });

    this.ventasPorMes = {
      labels: meses,
      datasets: [
        {
          label: 'Ventas',
          data: totalesPorMes,
          backgroundColor: '#42A5F5',
        },
      ],
    };
  }

  generarGraficoEstadoFacturas(facturas: Factura[]): void {
    const pagadas = facturas.filter((f) => f.estado === 0).length;
    const pendientes = facturas.filter((f) => f.estado === 1).length;

    this.estadoFacturas = {
      labels: ['Pagadas', 'Pendientes'],
      datasets: [
        {
          data: [pagadas, pendientes],
          backgroundColor: ['#66BB6A', '#FFA726'],
        },
      ],
    };
  }

  generarGraficoClientesFacturacion(facturas: Factura[]): void {
    const facturacionPorCliente: { [clienteId: number]: number } = {};

    facturas.forEach((factura) => {
      if (!facturacionPorCliente[factura.clienteId]) {
        facturacionPorCliente[factura.clienteId] = 0;
      }
      // @ts-ignore
      facturacionPorCliente[factura.clienteId] += factura.total;
    });

    const labels = Object.keys(facturacionPorCliente).map(
      (clienteId) => `Cliente ${clienteId}`
    );
    const data = Object.values(facturacionPorCliente);

    this.clientesFacturacion = {
      labels,
      datasets: [
        {
          label: 'Facturación por Cliente',
          data,
          backgroundColor: '#FF5722',
        },
      ],
    };
  }
}
