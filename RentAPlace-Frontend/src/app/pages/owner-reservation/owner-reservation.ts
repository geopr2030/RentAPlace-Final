import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { ReservationService } from '../../services/reservation.service';
import { Reservation } from '../../models/reservation.model';
import { PropertyResponseDto } from '../../models/property.model';

@Component({
  selector: 'app-owner-reservations',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './owner-reservation.html',
  styleUrls: ['./owner-reservation.css']
})
export class OwnerReservationsComponent implements OnInit {
  properties: PropertyResponseDto[] = [];
  reservationsByProperty: { [propertyId: number]: Reservation[] } = {};
  loading = true;
  actionLoading: { [id: number]: boolean } = {};

  constructor(
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadOwnerProperties();
  }

  loadOwnerProperties(): void {
    this.loading = true;
    this.propertyService.getOwnerProperties().subscribe({
      next: (props: any) => {
        this.properties = Array.isArray(props) ? props : [];
        // Load reservations for each property
        let pending = this.properties.length;
        if (pending === 0) {
          this.loading = false;
          this.cdr.detectChanges();
          return;
        }

        this.properties.forEach(prop => {
          console.log(`Fetching reservations for property ID: ${prop.propertyId}`);
          this.reservationService.getPropertyReservations(prop.propertyId).subscribe({
            next: (res: any) => {
              console.log(`Reservations for property ${prop.propertyId}:`, res);
              this.reservationsByProperty[prop.propertyId] = Array.isArray(res) ? res : [];
              pending--;
              if (pending === 0) {
                this.loading = false;
                this.cdr.detectChanges();
              }
            },
            error: (err) => {
              console.error(`Error loading reservations for property ${prop.propertyId}:`, err);
              this.reservationsByProperty[prop.propertyId] = [];
              pending--;
              if (pending === 0) {
                this.loading = false;
                this.cdr.detectChanges();
              }
            }
          });
        });
      },
      error: (err) => {
        console.error('Error loading properties:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getReservations(propertyId: number): Reservation[] {
    return this.reservationsByProperty[propertyId] || [];
  }

  getTotalReservations(): number {
    let total = 0;
    for (const key in this.reservationsByProperty) {
      total += this.reservationsByProperty[key].length;
    }
    return total;
  }

  onConfirm(reservationId: number): void {
    this.actionLoading[reservationId] = true;
    this.cdr.detectChanges();

    this.reservationService.updateStatus(reservationId, 'confirmed').subscribe({
      next: () => {
        // Update locally
        for (const key in this.reservationsByProperty) {
          const res = this.reservationsByProperty[key].find(r => r.reservationId === reservationId);
          if (res) {
            res.status = 'Confirmed';
            break;
          }
        }
        this.actionLoading[reservationId] = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        alert('Failed to confirm reservation.');
        this.actionLoading[reservationId] = false;
        this.cdr.detectChanges();
      }
    });
  }

  onReject(reservationId: number): void {
    if (!confirm('Are you sure you want to reject this reservation?')) return;

    this.actionLoading[reservationId] = true;
    this.cdr.detectChanges();

    this.reservationService.updateStatus(reservationId, 'cancelled').subscribe({
      next: () => {
        for (const key in this.reservationsByProperty) {
          const res = this.reservationsByProperty[key].find(r => r.reservationId === reservationId);
          if (res) {
            res.status = 'Cancelled';
            break;
          }
        }
        this.actionLoading[reservationId] = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        alert('Failed to reject reservation.');
        this.actionLoading[reservationId] = false;
        this.cdr.detectChanges();
      }
    });
  }

  getStatusClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'confirmed': return 'status-confirmed';
      case 'pending': return 'status-pending';
      case 'cancelled': return 'status-cancelled';
      default: return '';
    }
  }
}