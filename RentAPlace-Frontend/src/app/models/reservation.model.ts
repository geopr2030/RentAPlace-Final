export interface Reservation {
  reservationId: number;
  propertyId: number;
  propertyTitle: string;
  renterName: string;
  checkInDate: string;
  checkOutDate: string;
  totalPrice: number;
  status: string; // Pending, Confirmed, Cancelled
  createdAt: string;
}

export interface CreateReservationDto {
  propertyId: number;
  checkInDate: string; // ISO string
  checkOutDate: string; // ISO string
}
