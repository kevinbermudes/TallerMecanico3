import { Component } from '@angular/core';
import {AuthService} from '../../Service/auth.service';
import {Router, RouterLink} from '@angular/router';
import {CardModule} from 'primeng/card';
import {FormsModule} from '@angular/forms';
import {PasswordModule} from 'primeng/password';
import {InputTextModule} from 'primeng/inputtext';
import {DividerModule} from 'primeng/divider';
import {ButtonDirective} from 'primeng/button';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [
    CardModule,
    FormsModule,
    PasswordModule,
    InputTextModule,
    DividerModule,
    ButtonDirective,
    NgIf,
    RouterLink
  ],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.css'
})
export class RegistroComponent {
  nombre: string = '';
  apellido: string = '';
  email: string = '';
  password: string = '';
  direccion: string = '';
  telefono: string = '';
  fechaNacimiento: Date | null = null;
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  registrar() {
    const registroData = {
      nombre: this.nombre,
      apellido: this.apellido,
      email: this.email,
      password: this.password,
      direccion: this.direccion,
      telefono: this.telefono,
      fechaNacimiento: this.fechaNacimiento
    };

    this.authService.registrar(registroData).subscribe(
      (response) => {
        console.log("Registro exitoso:", response);
        this.router.navigate(['/login']);
      },
      (error) => {
        if (error.status === 409 && error.error && error.error.message) {
          this.errorMessage = error.error.message; // Correo ya está en uso
        } else {
          this.errorMessage = 'Error al registrarse, intente nuevamente';
        }
      }
    );
  }


}
