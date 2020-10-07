import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

@Component({
  selector: 'app-reset-password-new',
  templateUrl: './reset-password-new.component.html',
  styleUrls: ['./reset-password-new.component.scss']
})
export class ResetPasswordNewComponent implements OnInit {

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }
  public otp: any;
  public email: any;
  public password: any;

  showPasswordResetSuccess() {
    this.toastr.success('Password has been reset successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
    this.router.navigateByUrl('login');
  }

  ngOnInit(): void {
    this.email = this.service.callEmail();
    this.otp = this.service.callOTP();
    //console.log(this.email);
  }

  async resetPassword(){
    await this.service.Patch(`/resetpassword?email=${this.email.__zone_symbol__value}&otp=${this.otp.__zone_symbol__value}&password=${this.password}`);
    this.showPasswordResetSuccess();
  }
}
