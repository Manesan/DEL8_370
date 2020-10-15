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
  selector: 'app-suburbs',
  templateUrl: './suburbs.component.html',
  styleUrls: ['./suburbs.component.scss']
})

export class SuburbsComponent implements OnInit {
  public suburbs; //holds list to populate cards
  public cities; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public suburbid: any;
  public nameInput: any;
  public cityid: any;
  public cityNameInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showAddSuccess() {
    this.toastr.success('Suburb added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Suburb updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Suburb deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('Suburb cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.suburbs = await this.service.Get('/suburb?token=' + this.token.token);
    this.cities = await this.service.Get('/city?token=' + this.token.token);
    console.log(this.suburbs);
    //Added
    //this.showViewModal = false;
    this.cityNameInput = null;
    //this.suburbid = null;
    this.nameInput = null;
    //this.cityid = null;
    this.spinner.hide();
  }

  async add(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.nameInput, this.cityid);
    await this.service.Post(`/suburb?token=${this.token.token}&suburbname=${this.nameInput}&cityid=${this.cityid}`);
    this.spinner.hide();
    this.showAddSuccess();
  }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    $("#editModal").modal('show');
    let suburb = await this.service.Get('/suburb?token=' + this.token.token + '&id='+ id) as any;
    console.log(suburb);
    this.nameInput = suburb.SUBURBNAME;
    this.cityNameInput = suburb.CITYNAME;
    this.suburbid = suburb.SUBURBID;
    this.cityid = suburb.CITYID;

    this.showViewModal = true;
    //console.log(this.descriptionInput);
    this.spinner.hide();
  }

  async update(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#confirmEditModal").modal('hide');
    //console.log(id);
    await this.service.Patch(`/suburb?token=${this.token.token}&id=${id}&suburbname=${this.nameInput}&cityid=${this.cityid}`);
    //this.showViewModal = false; //Added
    this.spinner.hide();
    this.showUpdateSuccess();
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let suburb = await this.service.Get('/suburb?token=' + this.token.token + '&id='+ id) as any;
    this.nameInput = suburb.SUBURBNAME;
    this.suburbid = suburb.SUBURBID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let response = await this.service.Delete('/suburb?token=' + this.token.token + '&id='+ id);
    this.spinner.hide();
    console.log(response)
    if (response === 409){
      this.showDeleteFailure();
    }
    else{
      this.showDeleteSuccess();
    }
  }

  //Add form validation
  async submitAdd(){

      $("#confirmAddModal").modal('show');
      $("#addModal").modal('hide');

  }

  //Update form validation
  async submitUpdate(){
    if(this.nameInput != "" && this.cityNameInput != ""){
      //console.log(this.descriptionInput);
      $("#editModal").modal('hide');
      $("#confirmEditModal").modal('show');
    }
  }

}
