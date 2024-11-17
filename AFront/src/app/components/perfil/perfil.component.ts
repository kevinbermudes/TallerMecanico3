import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ClienteService } from '../../Service/cliente.service';
import { Cliente } from '../../Entity/Cliente';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { AuthService } from '../../Service/auth.service';
import {ChipsModule} from 'primeng/chips';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DropdownModule, ButtonModule, ToastModule, ChipsModule],
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css'],
  providers: [MessageService]
})
export class PerfilComponent implements OnInit {
  clienteForm!: FormGroup;
  clienteOriginal!: Cliente | null;
  isFormChanged: boolean = false;

  // Opciones para los enums
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
    { label: 'UnionLibre', value: 4 }
  ];



  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService,
    private messageService: MessageService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.clienteForm = this.fb.group({
      primerNombre: ['',], // Asegúrate de definir 'primerNombre'
      segundoNombre: [''],
      primerApellido: ['', ],
      segundoApellido: [''],
      direccion: ['', ],
      telefono: ['', [ Validators.pattern(/^[0-9]{7,10}$/)]],
      emailSecundario: ['', Validators.email],
      dni: [''],
      fechaNacimiento: [''],
      genero: [''],
      estadoCivil: [''],
      ocupacion: [''],
      notas: ['']
    });
    this.loadCliente();
  }

  loadCliente(): void {
    const clienteData = this.authService.getClienteData();
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
        this.clienteOriginal = cliente;

        // Actualiza los valores del formulario con patchValue
        this.clienteForm.patchValue({
          primerNombre: cliente.primerNombre,
          segundoNombre: cliente.segundoNombre,
          primerApellido: cliente.primerApellido,
          segundoApellido: cliente.segundoApellido,
          direccion: cliente.direccion,
          telefono: cliente.telefono,
          emailSecundario: cliente.emailSecundario,
          dni: cliente.dni,
          fechaNacimiento: this.formatDateToInput(cliente.fechaNacimiento),
          genero: cliente.genero,
          estadoCivil: cliente.estadoCivil,
          ocupacion: cliente.ocupacion,
          notas: cliente.notas
        });

        // Detectar cambios en el formulario
        this.clienteForm.valueChanges.subscribe(() => {
          this.isFormChanged = this.detectFormChanges();
        });
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


  private formatDateToInput(date: string | Date | null): string | null {
    if (!date) return null;
    const parsedDate = typeof date === 'string' ? new Date(date) : date;
    return parsedDate.toISOString().split('T')[0];
  }

  detectFormChanges(): boolean {
    if (!this.clienteOriginal || !this.clienteForm) return false;
    return JSON.stringify(this.clienteOriginal) !== JSON.stringify({ ...this.clienteOriginal, ...this.clienteForm.value });
  }

  actualizarCliente(): void {
    if (!this.clienteForm.valid) {
      console.log('Errores del formulario:', this.clienteForm.errors);
      return;
    }

    console.log('Form válido:', this.clienteForm.valid);
    console.log('Form valores:', this.clienteForm.value);

    if (this.clienteOriginal) {
      const clienteActualizado = {
        ...this.clienteOriginal,
        ...this.clienteForm.value
      };

      this.clienteService.updateCliente(this.clienteOriginal.id, clienteActualizado).subscribe(
        () => {
          this.messageService.add({
            severity: 'success',
            summary: 'Éxito',
            detail: 'Datos del cliente actualizados correctamente.'
          });
          this.isFormChanged = false;
          this.loadCliente(); // Recargar datos
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
  }


}
