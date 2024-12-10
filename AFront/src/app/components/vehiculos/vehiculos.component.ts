import {AfterViewInit, ChangeDetectorRef, Component, OnInit} from '@angular/core';
import { Vehiculo } from '../../Entity/Vehiculo';
import { VehiculoService } from '../../Service/vehiculo.service';
import { AuthService } from '../../Service/auth.service';
import { MessageService } from 'primeng/api';
import {ButtonDirective} from 'primeng/button';
import {ToastModule} from 'primeng/toast';
import {FormsModule} from '@angular/forms';
import {InputNumberModule} from 'primeng/inputnumber';
import {DialogModule} from 'primeng/dialog';
import {TableModule} from 'primeng/table';
import {InputTextModule} from 'primeng/inputtext';

@Component({
  selector: 'app-vehiculos',
  templateUrl: './vehiculos.component.html',
  styleUrls: ['./vehiculos.component.css'],
  providers: [MessageService],
  standalone: true,
  imports: [
    ButtonDirective,
    ToastModule,
    FormsModule,
    InputNumberModule,
    DialogModule,
    TableModule,
    InputTextModule
  ]
})
export class VehiculosComponent implements OnInit , AfterViewInit {
  vehiculos: Vehiculo[] = [];
  nuevoVehiculo: Partial<Vehiculo> = {
    marca: '',
    modelo: '',
    year: undefined,
    placa: '',
  };
  clienteId!: number;
  currentYear: number = new Date().getFullYear();
  mostrarDialogo: boolean = false;

  constructor(
    private vehiculoService: VehiculoService,
    private authService: AuthService,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef

  ) {}

  ngOnInit(): void {
    const clienteData = this.authService.getClienteData();
    if (clienteData) {
      this.clienteId = clienteData.id;
      this.cargarVehiculos();
    } else {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo cargar los datos del cliente. Intenta iniciar sesión nuevamente.',
      });
    }
  }

  cargarVehiculos(): void {
    this.vehiculoService.getVehiculosByClienteId(this.clienteId).subscribe({
      next: (vehiculos) => {
        this.vehiculos = vehiculos;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al cargar los vehículos:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo cargar la lista de vehículos.',
        });
      },
    });
  }

  mostrarDialogoNuevoVehiculo(): void {
    this.mostrarDialogo = true;
  }

  agregarVehiculo(): void {
    if (
      !this.nuevoVehiculo.marca ||
      !this.nuevoVehiculo.modelo ||
      !this.nuevoVehiculo.year ||
      !this.nuevoVehiculo.placa
    ) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Datos incompletos',
        detail: 'Por favor, completa todos los campos.',
      });
      return;
    }

    const vehiculo = {
      ...this.nuevoVehiculo,
      clienteId: this.clienteId,
    } as Vehiculo;

    this.vehiculoService.createVehiculo(vehiculo).subscribe({
      next: (vehiculoCreado) => {
        this.vehiculos.push(vehiculoCreado);
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Vehículo añadido correctamente.',
        });
        this.nuevoVehiculo = { marca: '', modelo: '', year: undefined, placa: '' };
        this.mostrarDialogo = false; // Cerrar el diálogo
      },
      error: (err) => {
        console.error('Error al agregar vehículo:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo añadir el vehículo.',
        });
      },
    });
  }

  eliminarVehiculo(id: number): void {
    this.vehiculoService.deleteVehiculo(id).subscribe({
      next: () => {
        this.vehiculos = this.vehiculos.filter((vehiculo) => vehiculo.id !== id);
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Vehículo eliminado correctamente.',
        });
      },
      error: (err) => {
        console.error('Error al eliminar vehículo:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo eliminar el vehículo.',
        });
      },
    });
  }

  ngAfterViewInit(): void {
    this.cdr.detectChanges();
  }
}
