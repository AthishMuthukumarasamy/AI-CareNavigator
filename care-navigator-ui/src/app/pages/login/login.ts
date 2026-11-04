import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  email = '';

  password = '';

  rememberMe = false;

  constructor(
  private authService: AuthService,
  private router: Router
) {
  const savedEmail =
  localStorage.getItem('savedEmail');

  if (savedEmail) {

  this.email = savedEmail;

  this.rememberMe = true;
}
}

  login() {

    const request = {

      email: this.email,

      password: this.password
    };

    this.authService
      .login(request)
      .subscribe({

        next: (response) => {

          localStorage.setItem(
          'token',
          response.token
        );

          localStorage.setItem(
          'userEmail',
          this.email
        );

        alert('Login Successful');

        if (this.rememberMe) {

        localStorage.setItem(
        'savedEmail',
        this.email
        );
      } else {
        localStorage.removeItem(
        'savedEmail'
      );
  }

      this.router.navigate([
      '/patient-dashboard'
    ]);
      },

        error: (error) => {

          console.error(error);

          alert('Login Failed');
        }
      });
  }
}