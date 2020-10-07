import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
public email: any;
public otp: number;

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  async startReset(){
    await this.service.Post(`/resetpassword?email=${this.email}`);
  }
  async storeData(){
    this.service.storeEmail(this.email);
    this.service.storeOTP(this.otp);
  }
}
