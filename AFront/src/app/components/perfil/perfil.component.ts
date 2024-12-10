import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ClienteService } from '../../Service/cliente.service';
import { Cliente } from '../../Entity/Cliente';
import { MessageService } from 'primeng/api';
import { AuthService } from '../../Service/auth.service';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    InputTextareaModule,
    ButtonModule,
    ToastModule
  ],
  providers: [MessageService]
})
export class PerfilComponent implements OnInit {
  clienteForm!: FormGroup;
  clienteOriginal: Cliente | null = null;

  generos = [
    { label: 'Masculino', value: 0 },
    { label: 'Femenino', value: 1 },
    { label: 'Otro', value: 2 }
  ];

  estadosCiviles = [
    { label: 'Soltero', value: 0 },
    { label: 'Casado', value: 1 },
    { label: 'Divorciado', value: 2 },
    { label: 'Viudo', value: 3 },
    { label: 'Unión Libre', value: 4 }
  ];

  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.inicializarFormulario();
    this.loadCliente();
  }

  inicializarFormulario(): void {
    this.clienteForm = this.fb.group({
      primerNombre: ['', [Validators.minLength(3)]],
      segundoNombre: ['', [Validators.minLength(3)]],
      primerApellido: ['', [Validators.minLength(3)]],
      segundoApellido: ['', [Validators.minLength(3)]],
      direccion: ['', [Validators.minLength(3)]],
      telefono: ['', [Validators.required, Validators.pattern(/^[0-9]{8,10}$/)]], // Mínimo 8 dígitos
      emailSecundario: ['', [Validators.email]],
      dni: [''],
      fechaNacimiento: [''],
      genero: [''],
      estadoCivil: [''],
      ocupacion: ['', [Validators.minLength(3)]],
      notas: ['', [Validators.minLength(3)]]
    });

    console.log('Formulario inicializado:', this.clienteForm);
  }

  loadCliente(): void {
    const clienteData = this.authService.getClienteData();
    console.log('Datos de cliente obtenidos de AuthService:', clienteData); // Log añadido

    if (!clienteData || !clienteData.id) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'No se pudo obtener los datos del cliente.'
      });
      return;
    }

    this.clienteService.getClienteById(clienteData.id).subscribe(
      (cliente) => {
        console.log('Datos recibidos del backend:', cliente); // Log añadido
        if (!cliente) {
          console.error('El backend no devolvió un cliente válido.');
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'No se pudieron cargar los datos del cliente.'
          });
          return;
        }

        this.clienteOriginal = cliente;
        console.log('Cliente original asignado:', this.clienteOriginal); // Log añadido

        this.clienteForm.patchValue({
          primerNombre: cliente.primerNombre || '',
          segundoNombre: cliente.segundoNombre || '',
          primerApellido: cliente.primerApellido || '',
          segundoApellido: cliente.segundoApellido || '',
          direccion: cliente.direccion || '',
          telefono: cliente.telefono || '',
          emailSecundario: cliente.emailSecundario || '',
          dni: cliente.dni || '',
          fechaNacimiento: this.formatDateToInput(cliente.fechaNacimiento),
          genero: cliente.genero !== undefined ? cliente.genero : '',
          estadoCivil: cliente.estadoCivil !== undefined ? cliente.estadoCivil : '',
          ocupacion: cliente.ocupacion || '',
          notas: cliente.notas || ''
        });

        console.log('Valores del formulario después de patchValue:', this.clienteForm.value); // Log añadido

        // Verificar la validez del formulario después de patchValue
        this.verificarValidezFormulario();
      },
      (error) => {
        console.error('Error al cargar cliente:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo cargar los datos del cliente.'
        });
      }
    );
  }

  verificarValidezFormulario(): void {
    if (this.clienteForm.invalid) {
      console.warn('Formulario inválido después de cargar los datos:', this.clienteForm.errors);
      Object.keys(this.clienteForm.controls).forEach((key) => {
        const control = this.clienteForm.get(key);
        if (control && control.invalid) {
          console.warn(`Campo inválido: ${key}`, control.errors);
        }
      });
    } else {
      console.log('Formulario válido después de cargar los datos.');
    }
  }

  actualizarCliente(): void {
    console.log('Intentando actualizar cliente...');
    console.log('Estado del formulario:', this.clienteForm.status); // Log añadido
    console.log('Cliente original:', this.clienteOriginal); // Log añadido

    if (!this.clienteOriginal) {
      console.error('Cliente original no cargado.');
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Los datos del cliente no están cargados.'
      });
      return;
    }

    if (!this.clienteForm.valid) {
      console.error('Formulario no válido:', this.clienteForm.errors);
      Object.keys(this.clienteForm.controls).forEach((key) => {
        const control = this.clienteForm.get(key);
        if (control && control.invalid) {
          console.error(`Campo inválido: ${key}`, control.errors);
        }
      });
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'El formulario tiene errores. Por favor, verifica los campos marcados.'
      });
      return;
    }

    const clienteActualizado = {
      ...this.clienteOriginal,
      ...this.clienteForm.value,
      fechaNacimiento: this.formatDateToBackend(this.clienteForm.value.fechaNacimiento)
    };

    console.log('Cliente a actualizar:', clienteActualizado); // Log añadido

    this.clienteService.updateCliente(this.clienteOriginal.id, clienteActualizado).subscribe(
      () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Datos del cliente actualizados correctamente.'
        });
        this.clienteForm.markAsPristine(); // Marcar el formulario como limpio después de actualizar
        this.loadCliente(); // Recargar los datos actualizados
      },
      (error) => {
        console.error('Error al actualizar cliente:', error);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo actualizar los datos del cliente.'
        });
      }
    );
  }

  private formatDateToInput(date: string | Date | null): string | null {
    if (!date) return null;
    const parsedDate = typeof date === 'string' ? new Date(date) : date;
    if (isNaN(parsedDate.getTime())) {
      console.warn('Fecha inválida recibida:', date);
      return null;
    }
    return parsedDate.toISOString().split('T')[0];
  }

  private formatDateToBackend(date: string | null): string | null {
    if (!date) return null;
    const parsedDate = new Date(date);
    if (isNaN(parsedDate.getTime())) {
      console.warn('Fecha inválida para backend:', date);
      return null;
    }
    return parsedDate.toISOString();
  }
}
