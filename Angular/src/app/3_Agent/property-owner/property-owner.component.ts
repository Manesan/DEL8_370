import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-property-owner',
  templateUrl: './property-owner.component.html',
  styleUrls: ['./property-owner.component.scss']
})
export class PropertyOwnerComponent implements OnInit {
  public propertyOwners;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public propertyOwnerid: any;
  public nameInput: any;
  public surnameInput: any;
  public emailInput: any;
  public idnumberInput: any;
  public passportnumberInput: any;
  public addressInput: any;
  public contactnumberInput: any;
  public altcontactnumberInput: any;
  public newContactNumber: any;
  public newAltContactNumber: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showUpdateSuccess() 
  {
    this.toastr.success('Property owner updated successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }


  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.propertyOwners = await this.service.Get('/propertyowner?token=' + this.token.token);
    //console.log(this.propertyowner);
    this.showViewModal=false;
    this.nameInput = null; 
    this.surnameInput = null;
    this.emailInput = null; 
    this.idnumberInput =null;
    this.addressInput = null; 
    this.contactnumberInput = null; 
    this.altcontactnumberInput =null;
    this.spinner.hide();
}

async view(id)
{
  
  this.spinner.show();
  $("#editModal").modal('show');

  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(id);
  let propertyOwner = await this.service.Get('/propertyowner?token=' + this.token.token + '&id='+ id) as any;
  console.log(propertyOwner);
  this.propertyOwnerid = propertyOwner.PROPERTYOWNERID, 
  this.nameInput = propertyOwner.PROPERTYOWNERNAME, 
  this.surnameInput = propertyOwner.PROPERTYOWNERSURNAME, 
  this.emailInput = propertyOwner.PROPERTYOWNEREMAIL, 
  this.idnumberInput = propertyOwner.PROPERTYOWNERIDNUMBER, 
  this.passportnumberInput = propertyOwner.PROPERTYOWNERPASSPORTNUMBER, 
  this.addressInput = propertyOwner.PROPERTYOWNERADDRESS, 
  this.contactnumberInput = propertyOwner.PROPERTYOWNERCONTACTNUMBER, 
  this.altcontactnumberInput = propertyOwner.PROPERTYOWNERALTCONTACTNUMBER,
  this.showViewModal = true;
  this.spinner.hide();
  
}

  //Add form validation
  async submitUpdate(){
    //console.log(this.descriptionInput);
    $("#confirmEditModal").modal('show');
    $("#editModal").modal('hide');
}

async update(id)
{
  this.spinner.show();
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}

  //console.log(id);
  this.phoneNumberUpdateJoiner();
  await this.service.Patch(`/propertyowner?token=${this.token.token}&id=${id}&name=${this.nameInput}&surname=${this.surnameInput}&email=${this.emailInput}&owneridnumber=${this.idnumberInput}&ownerpassportnumber=${this.passportnumberInput}&contactnumber=${this.newContactNumber}&altcontactnumber=${this.newAltContactNumber}&address=${this.addressInput}`);
  this.showViewModal = false;
  this.showUpdateSuccess();
  
  this.spinner.hide();
}

phoneNumberUpdateJoiner()
{
  if (this.contactnumberInput != null){
    this.newContactNumber = "%2B"+this.contactnumberInput.substring(1);
  }
  if (this.altcontactnumberInput != null){
    this.newAltContactNumber = "%2B"+this.altcontactnumberInput.substring(1);
  }
}
}
