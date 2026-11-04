import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-provider-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './provider-dashboard.html',
  styleUrl: './provider-dashboard.css'
})
export class ProviderDashboard {

  totalPatients = 245;

  highRiskPatients = 18;

  followUps = 32;

  activeCarePlans = 115;

  reportsPending = 12;
  
  highRiskList = [
    {
      name: 'Arun Kumar',
      score: 92,
      status: 'Critical'
    },
    {
      name: 'Priya Sharma',
      score: 88,
      status: 'High'
    },
    {
      name: 'Ravi Kumar',
      score: 84,
      status: 'High'
    }
  ];

  activities = [
    'Risk assessment generated',
    'New report uploaded',
    'Care plan updated',
    'Follow-up appointment scheduled'
  ];

}