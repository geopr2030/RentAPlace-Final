import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PropertyService } from '../../services/property.service';
import { PropertyResponseDto } from '../../models/property.model';

@Component({
  selector: 'app-owner-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './owner-dashboard.html',
  styleUrls: ['./owner-dashboard.css']
})
export class OwnerDashboardComponent implements OnInit {
  properties: PropertyResponseDto[] = [];
  loading = false;

  constructor(
    private propertyService: PropertyService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProperties();
  }

  loadProperties(): void {
    this.loading = true;
    this.properties = [];
    this.cdr.detectChanges();

    this.propertyService.getOwnerProperties().subscribe({
      next: (res) => {
        this.properties = res || [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading owner properties:', err);
        this.properties = [];
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  deleteProperty(id: number): void {
    if (!confirm('Delete this property?')) return;

    this.propertyService.deleteProperty(id).subscribe({
      next: () => {
        alert('Deleted successfully');
        this.loadProperties();
      },
      error: (err) => {
        console.error('Delete error:', err);
        alert('Delete failed');
      }
    });
  }
  getImageUrl(path: string): string {
  if (!path) return '';

  if (path.startsWith('http')) return path;

  return `https://localhost:7054/${path.replace(/^\/+/, '')}`;
}
}