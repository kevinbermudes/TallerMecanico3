import { Component } from '@angular/core';
import { AuthService } from '../../Service/auth.service';
import {Router, RouterLink} from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {CardModule} from 'primeng/card';
import {PasswordModule} from 'primeng/password';
import {Button, ButtonModule} from 'primeng/button';
import {InputTextModule} from 'primeng/inputtext';
import {DividerModule} from 'primeng/divider';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, CardModule, PasswordModule, Button, ButtonModule, InputTextModule, DividerModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}


  login() {
    this.authService.login(this.email, this.password).subscribe(
      (response) => {
        console.log("Token recibido:", response.token);
        this.authService.setToken(response.token);

        const role = this.authService.getUserRole();
        if (role === 'Admin') {
          this.router.navigate(['/admin']);
        } else if (role === 'Cliente') {
          this.router.navigate(['/cliente']);
        } else {
          this.errorMessage = 'Rol desconocido';
        }
      },
      (error) => {
        this.errorMessage = 'Credenciales incorrectas';
      }
    );
  }
}
