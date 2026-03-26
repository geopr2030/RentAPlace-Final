import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { PropertyResponseDto } from '../../models/property.model';
import { PropertyCardComponent } from '../../shared/property-card/property-card.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, PropertyCardComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  topProperties: PropertyResponseDto[] = [];
  loading = true;

  searchParams = {
    location: '',
    propertyType: '',
    checkIn: '',
    checkOut: ''
  };

  propertyTypes = ['Flat', 'Villa', 'Apartment', 'Cottage', 'Studio', 'House'];

  constructor(
    private propertyService: PropertyService,
    private router: Router,
    public auth: AuthService
  ) { }

  ngOnInit(): void {
    this.fetchTopRated();
  }

  fetchTopRated(): void {
    this.loading = true;
    console.log('Fetching top rated properties...');
    this.propertyService.getTopRatedProperties(6).subscribe({
      next: (data) => {
        console.log('Top rated API returned:', data);

        let props: any[] = [];
        if (Array.isArray(data)) {
          props = data;
        } else if (data && typeof data === 'object' && 'value' in data) {
          props = (data as any).value || [];
        }

        // Ensure features and imageUrls are arrays
        props.forEach(prop => {
          prop.features = Array.isArray(prop.features) ? prop.features : [];
          prop.imageUrls = Array.isArray(prop.imageUrls) ? prop.imageUrls : [];
          prop.averageRating = prop.averageRating || 0;
          prop.location = prop.location || 'Unknown';
          prop.pricePerNight = prop.pricePerNight || 0;
        });

        this.topProperties = props;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching properties', err);
        this.topProperties = [];
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    const queryParams: any = {};
    
    if (this.searchParams.location && this.searchParams.location.trim() !== '') {
      queryParams.location = this.searchParams.location.trim();
    }
    
    if (this.searchParams.propertyType && this.searchParams.propertyType !== '' && this.searchParams.propertyType !== 'Any Type') {
      queryParams.propertyType = this.searchParams.propertyType;
    }
    
    if (this.searchParams.checkIn) {
      queryParams.checkIn = this.searchParams.checkIn;
    }
    
    if (this.searchParams.checkOut) {
      queryParams.checkOut = this.searchParams.checkOut;
    }

    console.log('Searching RentAPlace with:', queryParams);
    
    // Explicitly navigate to /search
    this.router.navigate(['/search'], { 
      queryParams,
      queryParamsHandling: 'merge' // ensure we don't lose other context if any
    }).then(success => {
      if (success) {
        console.log('Navigation to search results successful');
      } else {
        console.error('Navigation to search results failed');
      }
    });
  }
}
