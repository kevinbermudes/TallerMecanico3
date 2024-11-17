import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import {ClienteComponent} from './components/cliente/cliente.component';
import {AdminComponent} from './components/admin/admin.component';
import {RegistroComponent} from './components/registro/registro.component';
import {PerfilComponent} from './components/perfil/perfil.component';
import {ServiciosComponent} from './components/servicios/servicios.component';
import {VehiculosComponent} from './components/vehiculos/vehiculos.component';
import {FacturasComponent} from './components/facturas/facturas.component';
import {CartasPagoComponent} from './components/cartas-pago/cartas-pago.component';
import {CarritoComponent} from './components/carrito/carrito.component';
import {TiendaComponent} from './components/tienda/tienda.component';

export const routes: Routes = [
  {
    path: 'cliente',
    component: ClienteComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'tienda', component: TiendaComponent },
      { path: 'facturas', component: FacturasComponent },
      { path: 'vehiculos', component: VehiculosComponent },
      { path: 'servicios', component: ServiciosComponent },
      { path: 'perfil', component: PerfilComponent },
      { path: 'cartas-de-pago', component: CartasPagoComponent },
      { path: 'carrito', component: CarritoComponent },
      {path: '', redirectTo: 'tienda', pathMatch: 'full'}
    ]
  },
  { path: 'registro', component: RegistroComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard]
  },
  { path: '**', redirectTo: 'login' }
];
