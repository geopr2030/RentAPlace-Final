import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PropertyService } from '../../services/property.service';

@Component({
  selector: 'app-owner-edit-property',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './owner-edit-property.html',
  styleUrls: ['./owner-edit-property.css']
})
export class OwnerEditPropertyComponent implements OnInit {
  id!: number;

  property = {
    title: '',
    description: '',
    location: '',
    propertyType: '',
    pricePerNight: 0,
    features: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private propertyService: PropertyService
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.propertyService.getPropertyById(this.id).subscribe({
      next: (res: any) => {
        this.property = {
          title: res.title || '',
          description: res.description || '',
          location: res.location || '',
          propertyType: res.propertyType || '',
          pricePerNight: res.pricePerNight || 0,
          features: Array.isArray(res.features)
            ? res.features.join(', ')
            : (res.features || '')
        };
      },
      error: (err) => {
        console.error('Load property failed', err);
        alert('Could not load property');
      }
    });
  }

  update(): void {
    const dto = {
      title: this.property.title,
      description: this.property.description,
      location: this.property.location,
      propertyType: this.property.propertyType,
      pricePerNight: this.property.pricePerNight,
      features: this.property.features
    };

    this.propertyService.updateProperty(this.id, dto).subscribe({
      next: () => {
        alert('Property updated successfully');
        this.router.navigate(['/owner/dashboard']);
      },
      error: (err) => {
  console.error('Update error:', err);
  alert(err?.error?.message || 'Error updating');
}
    });
  }
}