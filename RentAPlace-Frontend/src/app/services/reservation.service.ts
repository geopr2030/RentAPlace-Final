import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Reservation, CreateReservationDto } from '../models/reservation.model';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private apiUrl = `${environment.apiUrl}/reservation`;

  constructor(private http: HttpClient) {}

  createReservation(dto: CreateReservationDto): Observable<Reservation> {
    return this.http.post<Reservation>(this.apiUrl, dto);
  }

  getMyReservations(): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(`${this.apiUrl}/my-reservations`);
  }

  getPropertyReservations(propertyId: number): Observable<Reservation[]> {
    return this.http.get<Reservation[]>(`${this.apiUrl}/property/${propertyId}`);
  }

 updateStatus(id: number, status: string): Observable<any> {
  if (status.toLowerCase() === 'confirmed') {
    return this.http.put(`${this.apiUrl}/${id}/confirm`, {});
  }

  if (status.toLowerCase() === 'cancelled') {
    return this.http.put(`${this.apiUrl}/${id}/cancel`, {});
  }

  throw new Error('Unsupported reservation status');
}

cancelReservation(id: number): Observable<any> {
  return this.http.put(`${this.apiUrl}/${id}/cancel`, {});
}
}