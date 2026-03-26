import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface MessageResponseDto {
  messageId: number;
  propertyId: number;
  propertyTitle: string;
  senderId: number;
  senderName: string;
  receiverId: number;
  receiverName: string;
  content: string;
  sentAt: Date;
  isRead: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private apiUrl = `${environment.apiUrl}/message`;

  constructor(private http: HttpClient) {}

  getInbox(): Observable<MessageResponseDto[]> {
    return this.http.get<MessageResponseDto[]>(`${this.apiUrl}/inbox`);
  }

  sendMessage(dto: any): Observable<any> {
    return this.http.post(this.apiUrl, dto);
  }

  markAsRead(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/read`, {});
  }
}
