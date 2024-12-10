import { Component, OnInit } from '@angular/core';
import { ClienteService } from '../../../Service/cliente.service';
import { Cliente } from '../../../Entity/Cliente';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import {TableModule} from 'primeng/table';
import {ButtonDirective} from 'primeng/button';
import {ToastModule} from "primeng/toast";
import {InputTextModule} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-facturas-admin',
  templateUrl: './facturas-admin.component.html',
  styleUrls: ['./facturas-admin.component.css'],
  standalone: true,
  imports: [
    TableModule,
    ButtonDirective,
    ToastModule,
    InputTextModule,
    FormsModule
  ],
  providers: [MessageService]
})
export class FacturasAdminComponent implements OnInit {
  clientes: Cliente[] = [];
  searchValue: string = '';

  constructor(
    private clienteService: ClienteService,
    private router: Router,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.clienteService.getAllClientes().subscribe({
      next: (clientes) => (this.clientes = clientes),
      error: (err) =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudieron cargar los clientes.'
        })
    });
  }

  clear(table: any): void {
    table.clear();
    this.searchValue = '';
  }

  verFacturas(clienteId: number): void {
    this.router.navigate([`/admin/facturas/${clienteId}`]);
  }
}
