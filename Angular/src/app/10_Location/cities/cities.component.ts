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
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrls: ['./cities.component.scss']
})

export class CitiesComponent implements OnInit {
  public cities; //holds list to populate cards
  public provinces; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public cityid: any;
  public nameInput: any;
  public provinceid: any;
  public provinceNameInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showAddSuccess() {
    this.toastr.success('City added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('City updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('City deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('City cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.cities = await this.service.Get('/city?token=' + this.token.token);
    this.provinces = await this.service.Get('/province?token=' + this.token.token);
    console.log(this.provinces);
    //Added
    this.showViewModal = false;
    this.nameInput = null;
    this.spinner.hide();
  }

  async add(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.nameInput, this.provinceid);
    await this.service.Post(`/city?token=${this.token.token}&cityname=${this.nameInput}&provinceid=${this.provinceid}`);
    this.showAddSuccess();
    this.spinner.hide();
  }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    let city = await this.service.Get('/city?token=' + this.token.token + '&id='+ id) as any;
    console.log(city);
    this.nameInput = city.CITYNAME;
    this.provinceNameInput = city.PROVINCENAME;
    this.cityid = city.CITYID;
    this.provinceid = city.PROVINCEID;

    this.showViewModal = true;
    this.spinner.hide();
    //console.log(this.descriptionInput);
  }

  async update(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/city?token=${this.token.token}&id=${id}&cityname=${this.nameInput}&provinceid=${this.provinceid}`);
    this.showViewModal = false; //Added
    this.spinner.hide();
    this.showUpdateSuccess();
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let city = await this.service.Get('/city?token=' + this.token.token + '&id='+ id) as any;
    this.nameInput = city.CITYNAME;
    this.cityid = city.CITYID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let response = await this.service.Delete('/city?token=' + this.token.token + '&id='+ id);
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
    if(this.nameInput !== null){
      //console.log(this.descriptionInput);
      $("#confirmAddModal").modal('show');
      $("#addModal").modal('hide');
    }
  }

  //Update form validation
    async submitUpdate(){
      if(this.nameInput != ""){
        //console.log(this.descriptionInput);
        $("#editModal").modal('hide');
        $("#confirmEditModal").modal('show');
      }
    }

}
