import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { Login } from './pages/login/login';
import { PatientDashboard } from './pages/patient-dashboard/patient-dashboard';
import { ProviderDashboard } from './pages/provider-dashboard/provider-dashboard';

export const routes: Routes = [

  {
    path: '',
    component: Login
  },

  {
  path: 'patient-dashboard',
  component: PatientDashboard,
  canActivate: [authGuard]
 },

 {
  path: 'provider-dashboard',
  component: ProviderDashboard,
  canActivate: [authGuard]
},

  {
    path: '**',
    redirectTo: ''
  }

];