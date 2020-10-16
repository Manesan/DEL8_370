import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { CountryCodes } from '../../../assets/CountryCodes';
import { AbstractControl } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-register-client',
  templateUrl: './register-client.component.html',
  styleUrls: ['./register-client.component.scss']
})
export class RegisterClientComponent implements OnInit {


  public clients;
  public email: any;
  public name: any;
  public surname: any;
  public number: any;
  public altnumber: any;
  public newNumber: any;
  public newAltnumber: any;
  public idnumber: any;
  public address: any;
  public password: any;
  public confirmpassword: any;
  public token: any;
  public countryCode: any;
  public altCountryCode: any;
  public countryCodes = CountryCodes;
  public poppedNumber1: any;
  public poppedNumber2: any;

  public SouthAfrican: any;
  public clientIdNumberInput: any;
  public clientPassportNumberInput:any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }



  showRegisterSuccess() {
    this.toastr.success('You are successfully registered', "", {
      timeOut: 1000,
    });
    this.router.navigateByUrl('');
  }

  async ngOnInit() {
  //   this.clients = await this.service.Get('/client');
  //   console.log(this.clients);
  }

  async register(){
    if(this.password == this.confirmpassword){
      this.spinner.show();
      this.phoneNumberJoiner();
      this.token = await this.service.Post(`/registerclient?email=${this.email}&name=${this.name}&surname=${this.surname}&contactnumber=${this.newNumber}
      &altcontactnumber=${this.newAltnumber}&address=${this.address}&password=${this.password}&idnumber=${this.clientIdNumberInput}
      &passportnumber=${this.clientPassportNumberInput}`);
      localStorage.setItem("37y7ffheu73", this.token.token)
      this.spinner.hide();
      this.showRegisterSuccess();
    }
    else{
      this.toastr.warning("The passwords do not match")
    }

  }

  phoneNumberJoiner()
  {
    if (this.number != null){
      this.newNumber = "%2B"+this.countryCode.substring(1)+this.number.substring(1);
    }
    if (this.altnumber != null){
      this.newAltnumber = "%2B"+this.altCountryCode.substring(1)+this.altnumber.substring(1);
    }
  }

  allowSubmit(){
    console.log("passed")
  }

}
