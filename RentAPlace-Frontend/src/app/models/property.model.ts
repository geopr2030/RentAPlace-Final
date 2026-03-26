export interface Property {
  propertyId: number;
  ownerId: number;
  title: string;
  description: string;
  location: string;
  propertyType: string;
  pricePerNight: number;
  features: string; // Comma-separated
  imageUrls: string[];
  isActive: boolean;
  createdDate: string;
  averageRating?: number;
  reviews?: Review[];
}

export interface Review {
  reviewId: number;
  propertyId: number;
  reviewerId: number;
  reviewerName: string;
  rating: number;
  comment: string;
  createdAt: string;
}

export interface PropertyResponseDto {
  propertyId: number;
  ownerId: number;
  ownerName: string;
  title: string;
  description: string;
  location: string;
  propertyType: string;
  pricePerNight: number;
  features: string[];
  imageUrls: string[];
  averageRating: number;
  reviewCount: number;
}
