import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection | undefined;
  public productUpdate = new BehaviorSubject<any>(null);  // Observable for product updates

  constructor() {}

  // Start the SignalR connection
  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/productHub')  // Backend SignalR Hub URL
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connection established'))
      .catch((err) => console.error('SignalR connection error: ', err));
  }

  // Listen for product updates
  public listenForUpdates() {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveProductUpdate', (barcode: string, newQuantity: number) => {
        console.log(`Product ${barcode} updated to quantity: ${newQuantity}`);
        // Emit the update to subscribers
        this.productUpdate.next({ barcode, newQuantity });
      });
    }
  }
}
