  import { Component } from '@angular/core';
  import { FormsModule } from '@angular/forms';
  import { HttpClient } from '@angular/common/http';
  import { CommonModule } from '@angular/common';
  import { Router } from '@angular/router';

  @Component({
    selector: 'app-patient-dashboard',
    standalone: true,
    imports: [
      FormsModule,
      CommonModule
    ],
    templateUrl: './patient-dashboard.html',
    styleUrl: './patient-dashboard.css',
  })
  export class PatientDashboard {

    patientName = '';

    patientInitials = '';

    question = '';

    aiResponse = `👋 Hello,

I'm your AI Care Navigator assistant.

I can help you with:

• Summarize my health status
• Show my medications
• Show my care plans
• Explain my risk score
• Review my health timeline

How can I help you today?`;

    riskAssessment = '';

    healthScore = 80;

    timelineEvents: any[] = [];

    isAiAssistantOpen = false;

    showAssistantGreeting = true;

    constructor(
      private http: HttpClient,
      private router: Router
    ) {
        const email =
        localStorage.getItem('userEmail');

        if(email){

        this.patientName =
        email.split('@')[0];

        this.patientInitials =
        this.patientName
          .substring(0,2)
          .toUpperCase();
      }

        this.loadTimeline();
        this.loadRiskAssessment();

        setTimeout(() => {
        this.showAssistantGreeting = false;
      }, 5000); 
    }

    toggleAiAssistant() {
      this.isAiAssistantOpen = !this.isAiAssistantOpen;
    }

    loadTimeline() {

      this.http.get<any[]>(
        'http://localhost:5017/api/AICareNavigator/health-timeline/1'
      )
      .subscribe({

        next: (response) => {

          this.timelineEvents = response;
        },

        error: (error) => {

          console.error(error);
        }
      });
    }

    loadRiskAssessment() {

        this.http.get<any>(
          'http://localhost:5017/api/AICareNavigator/risk-assessment/1'
        )
    .   subscribe({

        next: (response) => {

        this.riskAssessment =
          response.aiResponse;

        const match =
          response.aiResponse.match(
            /Risk Score:\s*(\d+)\/100/
          );

        if (match) {

          this.healthScore =
            Number(match[1]);
        }
      },

      error: (error) => {

        console.error(error);
      }
    });
  }

    askAI() {

      const request = {
        patientId: 1,
        question: this.question
      };

      this.http.post<any>(
        'http://localhost:5017/api/AICareNavigator/chat',
        request
      )
      .subscribe({
        next: (response) => {

          this.aiResponse =
            response.aiResponse;
        },

        error: (error) => {

          console.error(error);

          this.aiResponse =
            'Unable to get response from AI.';
        }
      });
    }

    logout() {

      localStorage.removeItem('token');

      this.router.navigate(['']);

    }
  }