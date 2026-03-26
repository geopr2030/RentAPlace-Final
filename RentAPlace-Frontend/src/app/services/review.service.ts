import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface CreateReviewDto {
  propertyId: number;
  rating: number;
  comment?: string;
}

export interface ReviewResponseDto {
  reviewId: number;
  propertyId: number;
  reviewerId: number;
  reviewerName: string;
  rating: number;
  comment?: string;
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = `${environment.apiUrl}/review`;

  constructor(private http: HttpClient) {}

  getPropertyReviews(propertyId: number): Observable<ReviewResponseDto[]> {
    return this.http.get<ReviewResponseDto[]>(`${this.apiUrl}/property/${propertyId}`);
  }

  createReview(dto: CreateReviewDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, dto);
  }
}
