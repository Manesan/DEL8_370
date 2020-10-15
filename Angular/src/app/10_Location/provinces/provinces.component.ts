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
  selector: 'app-provinces',
  templateUrl: './provinces.component.html',
  styleUrls: ['./provinces.component.scss']
})

export class ProvincesComponent implements OnInit {
  public provinces;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public provinceid: any;
  public nameInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { console.log }

  showAddSuccess() {
    this.toastr.success('Province added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Province updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Province deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('Province cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.provinces = await this.service.Get('/province?token=' + this.token.token);
    //Added
    this.showViewModal = false;
    this.nameInput = null;
    this.spinner.hide();
  }

  async add(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.nameInput);
    await this.service.Post(`/province?token=${this.token.token}&provincename=${this.nameInput}`);
    this.spinner.hide();
    this.showAddSuccess();
  }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    let province = await this.service.Get('/province?token=' + this.token.token + '&id='+ id) as any;
    this.nameInput = province.PROVINCENAME;
    this.provinceid = province.PROVINCEID;

    this.showViewModal = true;
    console.log(this.showViewModal);
    this.spinner.hide();
  }

  async update(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/province?token=${this.token.token}&id=${id}&provincename=${this.nameInput}`);
    this.showViewModal = false; //Added
    this.spinner.hide();
    this.showUpdateSuccess();
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let province = await this.service.Get('/province?token=' + this.token.token + '&id='+ id) as any;
    this.nameInput = province.PROVINCENAME;
    this.provinceid = province.PROVINCEID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let response = await this.service.Delete('/province?token=' + this.token.token + '&id='+ id);
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
