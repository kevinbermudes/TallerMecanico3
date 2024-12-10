import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {environment} from '../../env/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      // .withUrl('http://localhost:5132/notificacion')
      .withUrl(`${environment.apiUrl}notificacion`)
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('Conexión SignalR iniciada.'))
      .catch((err) => console.error('Error al conectar SignalR:', err));
  }

  onReceiveNotification(callback: (message: string) => void): void {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveNotification', callback);
    }
  }

  stopConnection(): void {
    this.hubConnection?.stop().then(() => console.log('Conexión SignalR detenida.'));
  }
}
