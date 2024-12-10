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
import {CatalogoServiciosComponent} from './components/servicios/catalogo-servicios/catalogo-servicios.component';
import {PaymentComponent} from './components/carrito/payment/payment.component';
import {FacturasCartaPagoComponent} from './components/cartas-pago/facturas-carta-pago/facturas-carta-pago.component';
import {AdminClienteComponent} from './components/admin/admin-cliente/admin-cliente.component';
import {FacturasAdminComponent} from './components/admin/facturas-admin/facturas-admin.component';
import {FacturaClienteComponent} from './components/admin/facturas-admin/factura-cliente/factura-cliente.component';
import {ServiciosAdminComponent} from './components/admin/servicios-admin/servicios-admin.component';
import {ProductosAdminComponent} from './components/admin/productos-admin/productos-admin.component';
import {ReportesAdminComponent} from './components/admin/reportes-admin/reportes-admin.component';

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
      { path: 'catalogo-servicios', component: CatalogoServiciosComponent },
      { path: 'perfil', component: PerfilComponent },
      { path: 'payment', component: PaymentComponent },
      { path: 'cartas-de-pago', component: CartasPagoComponent },
      { path: 'carrito', component: CarritoComponent },
      { path: 'cartas-de-pago/:id/facturas', component: FacturasCartaPagoComponent },
      {path: '', redirectTo: 'tienda', pathMatch: 'full'}
    ]
  },
  { path: 'registro', component: RegistroComponent },

  { path: 'login', component: LoginComponent },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'clientes', component: AdminClienteComponent },
      { path: 'facturas', component: FacturasAdminComponent },
      {path: 'facturas/:id', component: FacturaClienteComponent},
      { path: 'servicios', component: ServiciosAdminComponent },
      { path: 'productos', component: ProductosAdminComponent },
      { path: 'reportes', component: ReportesAdminComponent },
      { path: '', redirectTo: 'clientes', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'login' }
];
