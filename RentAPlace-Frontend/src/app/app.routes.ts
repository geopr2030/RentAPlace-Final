import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { HomeComponent } from './pages/home/home.component';
import { OwnerDashboardComponent } from './pages/owner-dashboard/owner-dashboard';
import { SearchResultsComponent } from './pages/search-results/search-results.component';
import { PropertyDetailComponent } from './pages/property-detail/property-detail.component';
import { MyReservationsComponent } from './pages/my-reservations/my-reservations.component';
import { authGuard } from './guards/auth.guard';

// add these after creating components
import { OwnerReservationsComponent } from './pages/owner-reservation/owner-reservation';
import { InboxComponent } from './pages/inbox/inbox';
import { AdminPropertiesComponent } from './pages/admin-properties/admin-properties';
import { roleGuard } from './guards/auth.guard';
import { OwnerAddPropertyComponent } from './pages/owner-add-property/owner-add-property';


export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'search', component: SearchResultsComponent },
  { path: 'property/:id', component: PropertyDetailComponent },
  { path: 'my-reservations', component: MyReservationsComponent, canActivate: [authGuard] },
  { path: 'owner/dashboard', component: OwnerDashboardComponent, canActivate: [authGuard] },
  { path: 'owner/reservations', component: OwnerReservationsComponent, canActivate: [authGuard] },
  { path: 'inbox', component: InboxComponent, canActivate: [authGuard] },
  { path: 'admin/properties', component: AdminPropertiesComponent, canActivate: [authGuard, roleGuard], data: { roles: ['Admin'] } },
  {path: 'owner/add-property', component: OwnerAddPropertyComponent, canActivate: [authGuard]},
  {path: 'owner/edit-property/:id', loadComponent: () => import('./pages/owner-edit-property/owner-edit-property').then(m => m.OwnerEditPropertyComponent), canActivate: [authGuard]},
  { path: '**', redirectTo: '' }
];