import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PropertyService } from '../../services/property.service';

@Component({
  selector: 'app-admin-properties',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-properties.html',
  styleUrls: ['./admin-properties.css']
})
export class AdminPropertiesComponent implements OnInit {

  properties: any[] = [];

  constructor(private propertyService: PropertyService) {}

  ngOnInit() {
    this.loadProperties();
  }

  loadProperties() {
    this.propertyService.getAllPropertiesAdmin().subscribe({
      next: (data) => this.properties = data,
      error: (err) => console.error(err)
    });
  }

  deleteProperty(id: number) {
    if (confirm('Delete this property?')) {
      this.propertyService.deletePropertyAdmin(id).subscribe(() => {
        this.loadProperties();
      });
    }
  }
}