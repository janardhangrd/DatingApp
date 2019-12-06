import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    // console.log(this.model);
    this.authService.login(this.model).subscribe(next => {
      console.log('Login Method is succesfull');
    }, error => {
      console.log('Login Method failed');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token; // returns true if token is not empty else false.
  }

  logout() {
    // localStorage.clear();
    localStorage.removeItem('token');
    console.log('Logged out successfully.');
  }

}
