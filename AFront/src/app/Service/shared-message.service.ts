import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SharedMessageService {
  private messageSource = new Subject<any>();
  message$ = this.messageSource.asObservable();

  private totalAmountSource = new BehaviorSubject<number>(0);
  totalAmount$ = this.totalAmountSource.asObservable();

  private facturaIdSource = new BehaviorSubject<number | null>(null);
  facturaId$ = this.facturaIdSource.asObservable();

  private facturaIdsSubject = new BehaviorSubject<number[] | null>(null);
  facturaIds$ = this.facturaIdsSubject.asObservable();

  setTotalAmount(amount: number): void {
    this.totalAmountSource.next(amount);
  }

  sendMessage(message: any) {
    this.messageSource.next(message);
  }

  setFacturaIds(ids: number[]): void {
    this.facturaIdsSubject.next(ids);
  }

  setFacturaId(facturaId: number | null): void {
    this.facturaIdSource.next(facturaId);
  }

  clearMessage() {
    this.messageSource.next(null);
  }

  // Agregar métodos para obtener el valor actual
  getFacturaIds(): number[] | null {
    return this.facturaIdsSubject.getValue();
  }

  getFacturaId(): number | null {
    return this.facturaIdSource.getValue();
  }
}
