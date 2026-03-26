import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { PropertyResponseDto } from '../models/property.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private apiUrl = `${environment.apiUrl}/property`;

  constructor(private http: HttpClient) {}

  getAllProperties(): Observable<PropertyResponseDto[]> {
    return this.http.get<PropertyResponseDto[]>(this.apiUrl);
  }

  getTopRatedProperties(count: number = 5): Observable<PropertyResponseDto[]> {
    return this.http.get<PropertyResponseDto[]>(`${this.apiUrl}/top-rated`, {
      params: new HttpParams().set('count', count.toString())
    });
  }

  getPropertyById(id: number): Observable<PropertyResponseDto> {
    return this.http.get<PropertyResponseDto>(`${this.apiUrl}/${id}`);
  }

  getOwnerProperties(): Observable<PropertyResponseDto[]> {
    return this.http.get<PropertyResponseDto[]>(`${this.apiUrl}/my-listings`);
  }

  createProperty(formData: FormData): Observable<any> {
    return this.http.post(this.apiUrl, formData);
  }

  updateProperty(id: number, dto: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dto);
  }

  deleteProperty(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  searchProperties(params: any): Observable<PropertyResponseDto[]> {
    let httpParams = new HttpParams();

    if (params.location) httpParams = httpParams.set('location', params.location);
    if (params.propertyType) httpParams = httpParams.set('propertyType', params.propertyType);
    if (params.features) httpParams = httpParams.set('features', params.features);
    if (params.checkIn) httpParams = httpParams.set('checkIn', params.checkIn);
    if (params.checkOut) httpParams = httpParams.set('checkOut', params.checkOut);

    console.log('Final search URL:', `${this.apiUrl}/search`, 'with params:', httpParams.toString());

    return this.http.get<PropertyResponseDto[]>(`${this.apiUrl}/search`, {
      params: httpParams
    });
  }

  getAllPropertiesAdmin(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/admin/properties`);
  }

  deletePropertyAdmin(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/admin/properties/${id}`);
  }
  
}