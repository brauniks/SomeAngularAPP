import { AlertifyService } from './../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authservice: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.authservice.login(this.model).subscribe(data => {
      this.alertify.success('logged');
     }, error => {
      this.alertify.error('Failed to login');
     }, () => {
      this.router.navigate(['/members']);
     });
  }

  logout() {
      this.authservice.userToken = null;
      localStorage.removeItem('token');
      this.alertify.message('logged out');
      this.router.navigate(['/home']);
  }

  loggedIn() {
      return this.authservice.loggedIn();
  }

}

