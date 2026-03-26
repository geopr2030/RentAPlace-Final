import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PropertyResponseDto } from '../../models/property.model';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-property-card',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './property-card.component.html',
  styleUrl: './property-card.component.css'
})
export class PropertyCardComponent {
  @Input() property!: PropertyResponseDto;
  baseUrl = environment.apiUrl.replace('/api', '');

  getDisplayImage(): string {
    if (this.property.imageUrls && this.property.imageUrls.length > 0) {
      const url = this.property.imageUrls[0];
      if (url.startsWith('http')) return url;
      return `${this.baseUrl}/${url}`;
    }
    return 'assets/placeholder-property.jpg';
  }
}
