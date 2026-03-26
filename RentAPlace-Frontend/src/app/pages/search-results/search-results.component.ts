import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { PropertyResponseDto } from '../../models/property.model';
import { PropertyCardComponent } from '../../shared/property-card/property-card.component';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule, PropertyCardComponent, RouterLink],
  templateUrl: './search-results.component.html',
  styleUrl: './search-results.component.css'
})
export class SearchResultsComponent implements OnInit {
  results: PropertyResponseDto[] = [];
  loading = true;
  searchParams: any = {};

  constructor(
    private route: ActivatedRoute,
    private propertyService: PropertyService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchParams = { ...params };
      console.log('SearchResults: received queryParams:', this.searchParams);
      this.fetchResults();
    });
  }

  fetchResults(): void {
    this.loading = true;
    this.results = [];
    this.cdr.detectChanges();

    console.log('SearchResults: calling API with params:', this.searchParams);

    this.propertyService.searchProperties(this.searchParams).subscribe({
      next: (data: any) => {
        console.log('SearchResults: API returned raw data:', data);
        
        try {
          let props: any[] = [];
          if (Array.isArray(data)) {
            props = data;
          } else if (data && typeof data === 'object' && 'value' in data) {
            props = (data as any).value || [];
          } else if (data && typeof data === 'object' && '$values' in data) {
            props = (data as any).$values || [];
          }

          // Safely normalize each property
          this.results = props.map((prop: any) => ({
            ...prop,
            features: Array.isArray(prop.features) ? prop.features : [],
            imageUrls: Array.isArray(prop.imageUrls) ? prop.imageUrls : [],
            averageRating: prop.averageRating || 0,
            reviewCount: prop.reviewCount || 0,
            location: prop.location || 'Unknown',
            pricePerNight: prop.pricePerNight || 0
          }));

          console.log('SearchResults: processed results count:', this.results.length);
        } catch (e) {
          console.error('SearchResults: error processing data:', e);
          this.results = [];
        }

        this.loading = false;
        this.cdr.detectChanges();
        console.log('SearchResults: loading set to false, results:', this.results.length);
      },
      error: (err) => {
        console.error('SearchResults: API error:', err);
        this.results = [];
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
