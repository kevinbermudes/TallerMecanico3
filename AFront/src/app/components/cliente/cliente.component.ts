import { Component } from '@angular/core';
import {ClienteDashboardComponent} from '../cliente-dashboard/cliente-dashboard.component';

@Component({
  selector: 'app-cliente',
  standalone: true,
  imports: [
    ClienteDashboardComponent
  ],
  templateUrl: './cliente.component.html',
  styleUrl: './cliente.component.css'
})
export class ClienteComponent {

}
