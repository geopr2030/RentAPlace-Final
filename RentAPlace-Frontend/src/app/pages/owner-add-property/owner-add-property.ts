import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PropertyService } from '../../services/property.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-owner-add-property',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './owner-add-property.html',
  styleUrls: ['./owner-add-property.css']
})
export class OwnerAddPropertyComponent {

  property: any = {
    title: '',
    description: '',
    location: '',
    propertyType: '',
    pricePerNight: 0,
    features: ''
  };

  selectedFiles: File[] = [];

  constructor(private propertyService: PropertyService, private router: Router) {}

  onFileChange(event: any) {
    this.selectedFiles = Array.from(event.target.files);
  }

  submit() {
    const formData = new FormData();

    formData.append('Title', this.property.title);
    formData.append('Description', this.property.description);
    formData.append('Location', this.property.location);
    formData.append('PropertyType', this.property.propertyType);
    formData.append('PricePerNight', this.property.pricePerNight);
    formData.append('Features', this.property.features);

    this.selectedFiles.forEach(file => {
      formData.append('images', file);
    });

    this.propertyService.createProperty(formData).subscribe({
      next: () => {
        alert('Property created successfully!');
        this.router.navigate(['/owner/dashboard']);
      },
      error: (err) => {
        console.error(err);
        alert('Error creating property');
      }
    });
  }
}