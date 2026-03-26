import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../services/property.service';
import { ReservationService } from '../../services/reservation.service';
import { ReviewService, ReviewResponseDto } from '../../services/review.service';
import { MessageService } from '../../services/message.service';
import { PropertyResponseDto } from '../../models/property.model';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-property-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './property-detail.component.html',
  styleUrl: './property-detail.component.css'
})
export class PropertyDetailComponent implements OnInit {
  property?: PropertyResponseDto;
  loading = true;
  baseUrl = environment.apiUrl.replace('/api', '');

  // Reservation Form
  checkIn = '';
  checkOut = '';
  totalPrice = 0;
  reserving = false;
  reservationError = '';
  reservationSuccess = '';

  // Reviews
  reviews: ReviewResponseDto[] = [];
  newReview = { rating: 5, comment: '' };
  stars = [1, 2, 3, 4, 5];
  submittingReview = false;
  reviewError = '';
  reviewSuccess = '';
  // Contact Owner
  contactMessage = '';
  sendingMessage = false;
  messageSuccess = '';

  constructor(
    private route: ActivatedRoute,
    private propertyService: PropertyService,
    private reservationService: ReservationService,
    private reviewService: ReviewService,
    private messageService: MessageService,
    public auth: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.fetchProperty(id);
      this.fetchReviews(id);
    }
  }

  fetchProperty(id: number): void {
    this.propertyService.getPropertyById(id).subscribe({
      next: (data: any) => {
        // Normalize the data
        this.property = {
          ...data,
          features: Array.isArray(data.features) ? data.features : [],
          imageUrls: Array.isArray(data.imageUrls) ? data.imageUrls : [],
          averageRating: data.averageRating || 0,
          reviewCount: data.reviewCount || 0
        };
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Error fetching property', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  fetchReviews(propertyId: number): void {
    this.reviewService.getPropertyReviews(propertyId).subscribe({
      next: (res) => {
        this.reviews = res || [];
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching reviews', err);
      }
    });
  }

  calculateTotal(): void {
    if (this.checkIn && this.checkOut && this.property) {
      const d1 = new Date(this.checkIn);
      const d2 = new Date(this.checkOut);
      const diff = d2.getTime() - d1.getTime();
      const nights = Math.ceil(diff / (1000 * 60 * 60 * 24));
      if (nights > 0) {
        this.totalPrice = nights * this.property.pricePerNight;
      } else {
        this.totalPrice = 0;
      }
    }
  }

  onReserve(): void {
    if (!this.auth.isLoggedIn()) {
      this.reservationError = 'Please login to reserve this property.';
      return;
    }

    if (this.totalPrice <= 0 || !this.property) {
      this.reservationError = 'Invalid dates selected.';
      return;
    }

    this.reserving = true;
    this.reservationError = '';
    this.reservationSuccess = '';

    const dto = {
      propertyId: this.property.propertyId,
      checkInDate: new Date(this.checkIn).toISOString(),
      checkOutDate: new Date(this.checkOut).toISOString()
    };

    this.reservationService.createReservation(dto).subscribe({
      next: () => {
        this.reserving = false;
        this.reservationSuccess = 'Reservation request sent successfully! 🎉';
        this.checkIn = '';
        this.checkOut = '';
        this.totalPrice = 0;
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.reserving = false;
        this.reservationError = err.error?.message || err.message || 'Failed to create reservation.';
        this.cdr.detectChanges();
      }
    });
  }

  submitReview(): void {
    if (!this.property) return;

    this.submittingReview = true;
    this.reviewError = '';
    this.reviewSuccess = '';

    const dto = {
      propertyId: this.property.propertyId,
      rating: this.newReview.rating,
      comment: this.newReview.comment
    };

    this.reviewService.createReview(dto).subscribe({
      next: (res) => {
        this.submittingReview = false;
        this.reviewSuccess = 'Review submitted successfully!';
        this.newReview = { rating: 5, comment: '' };
        if (res.review) {
          this.reviews.unshift(res.review);
        }
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.submittingReview = false;
        this.reviewError = err.error?.message || 'Failed to submit review.';
        this.cdr.detectChanges();
      }
    });
  }

  sendMessageToOwner(): void {
    if (!this.property || !this.contactMessage.trim()) return;
    this.sendingMessage = true;
    this.messageSuccess = '';

    const dto = {
      propertyId: this.property.propertyId,
      receiverId: this.property.ownerId,
      content: this.contactMessage
    };

    this.messageService.sendMessage(dto).subscribe({
      next: () => {
        this.sendingMessage = false;
        this.messageSuccess = 'Message sent to owner!';
        this.contactMessage = '';
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.sendingMessage = false;
        console.error('Error sending message', err);
        this.cdr.detectChanges();
      }
    });
  }
}
