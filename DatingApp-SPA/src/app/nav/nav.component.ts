import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    // console.log(this.model);
    this.authService.login(this.model).subscribe(next => {
      this.alertify.sucess('Login Method is succesfull');
    }, error => {
      this.alertify.error('Login Method failed');
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    // localStorage.clear();
    localStorage.removeItem('token');
    this.alertify.sucess('Logged out successfully.');
  }

}
