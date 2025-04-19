# 🛠️ Taller Mecánico - Sistema de Gestión Integral

Este proyecto es una solución integral diseñada para la administración eficiente de un **taller mecánico**, abarcando desde la gestión de clientes y vehículos hasta la facturación, pagos, notificaciones en tiempo real y venta de productos y servicios. Desarrollado con tecnologías modernas como **.NET 7** para el backend y **Angular** para el frontend, ofrece una arquitectura robusta, escalable y segura.

---

## 🚗 Funcionalidades Principales

### 🔐 Autenticación y Seguridad
- Registro e inicio de sesión de usuarios con JWT
- Roles definidos: **Administrador** y **Cliente**
- Validación de cuenta activa y lógica de borrado suave
- Seguridad en endpoints usando políticas basadas en roles

### 👥 Gestión de Clientes
- Registro de datos personales
- Asociación con vehículos
- Visualización personalizada de datos en el panel del cliente

### 🚘 Gestión de Vehículos
- Añadir, editar y eliminar vehículos asociados a clientes
- Visualización de historial de servicios por vehículo

### 📄 Facturas, Cartas de Pago y Partes
- Generación de facturas automáticas con código único
- Creación de cartas de pago y control de vencimiento
- Asociar partes y servicios a un cliente con trazabilidad

### 💳 Pasarela de Pago
- Registro de pagos con control de estado
- Asociación de pagos a productos, servicios, partes o facturas
- Comprobantes de pago y validación desde el cliente

### 🔔 Notificaciones en Tiempo Real
- Integración con **SignalR** para emitir notificaciones instantáneas
- Avisos de nuevas facturas, recordatorios de pago, entre otros
- Sistema extensible para futuros tipos de notificación

### 🛍️ Tienda Integrada
- Carrito de compras para productos y servicios
- Visualización y gestión de categorías
- Finalización de compra con creación de facturas y pagos

### 📊 Paneles Administrativos y del Cliente
- Panel administrador con visión global del sistema
- Panel del cliente con acceso a sus servicios, pagos y vehículos
- Navegación fluida con barra de navegación persistente

---

## 🧰 Tecnologías Usadas

### Backend (.NET 7 + EF Core)
- ASP.NET Core Web API
- Entity Framework Core con Code First
- SQL Server
- SignalR para WebSockets
- JSON Web Tokens (JWT) para autenticación
- Arquitectura en capas con principios de Clean Code

### Frontend (Angular + PrimeNG)
- Angular CLI
- PrimeNG y PrimeFlex para interfaz y estilos
- Angular JWT para autenticación de cliente
- Angular SignalR para comunicación en tiempo real

---

## ⚙️ Instalación y Ejecución

### Requisitos Previos
- .NET 7 SDK
- Node.js y Angular CLI
- SQL Server o Docker
- Visual Studio / VS Code

### Backend
```bash
git clone https://github.com/tuusuario/taller-mecanico.git
cd taller-mecanico/backend
dotnet ef database update
dotnet run
```

### Frontend
```bash
cd frontend
npm install
ng serve
```

---

## 🧪 Estado del Proyecto

- ✅ Login y roles completos
- ✅ Facturación y pagos implementados
- ✅ Notificaciones en tiempo real funcionando
- ✅ Gestión de clientes, vehículos, servicios y productos
- 🚧 En desarrollo: historial detallado de servicios y estadísticas del taller

---

## 📌 Notas Adicionales

- El proyecto utiliza borrado lógico para mantener trazabilidad
- Las notificaciones pueden personalizarse por tipo de evento
- Preparado para despliegue en producción con perfiles `dev` y `prod`

---

## 🤝 Autor

**Kevin Bermúdez**  
_Técnico Superior en Desarrollo de Aplicaciones Web_  
💡 Apasionado por el desarrollo Full Stack y siempre abierto a nuevos retos.

📫 Contáctame por GitHub o LinkedIn para más información o colaboración.

---

## 📄 Licencia

Este proyecto está bajo licencia MIT. Puedes usarlo, modificarlo y adaptarlo con fines educativos o comerciales bajo los términos de dicha licencia.
