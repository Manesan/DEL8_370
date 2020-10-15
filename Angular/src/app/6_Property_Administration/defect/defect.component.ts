import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-defect',
  templateUrl: './defect.component.html',
  styleUrls: ['./defect.component.scss']
})
export class DefectComponent implements OnInit {
  public defects; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public defectid: any;
  public descriptionInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showAddSuccess() {
    this.toastr.success('Defect added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  showUpdateSuccess() {
    this.toastr.success('Defect updated successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  showDeleteSuccess() {
    this.toastr.success('Defect deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  showDeleteFailure() {
    this.toastr.error('Defect cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
      this.token ={"token" : localStorage.getItem("37y7ffheu73")}
      //console.log(this.token.token);
      this.defects = await this.service.Get('/defect?token=' + this.token.token);
      //console.log(this.defects);
      //console.log(this.defects[1].DEFECTDESCRIPTION);
      this.showViewModal = false;
      this.descriptionInput = null;
      //location.reload.bind(location);
    this.spinner.hide();
  }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#editModal").modal('show');
    //console.log(id);
    let defect = await this.service.Get('/defect?token=' + this.token.token + '&id='+ id) as any ;
    //console.log(defect);
    this.descriptionInput = defect.DEFECTDESCRIPTION;
    this.defectid = defect.DEFECTID;
    this.showViewModal = true;
    //console.log(this.descriptionInput);
    this.spinner.hide();
  }

  async update(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/defect?token=${this.token.token}&id=${id}&description=${this.descriptionInput}`);
    this.spinner.hide();
    this.showUpdateSuccess();
    this.showViewModal = false;
  }

  async add(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.descriptionInput);
    await this.service.Post(`/defect?token=${this.token.token}&description=${this.descriptionInput}`);
    this.spinner.hide();
    this.showAddSuccess();
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let defect = await this.service.Get('/defect?token=' + this.token.token + '&id='+ id) as any;
    this.descriptionInput = defect.DEFECTDESCRIPTION;
    this.defectid = defect.DEFECTID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    let response = await this.service.Delete('/defect?token=' + this.token.token + '&id='+ id);
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
    if(this.descriptionInput !== null){
      //console.log(this.descriptionInput);
      $("#confirmAddModal").modal('show');
      $("#addModal").modal('hide');
    }
  }

//Update form validation
  async submitUpdate(){
    if(this.descriptionInput != ""){
      //console.log(this.descriptionInput);
      $("#editModal").modal('hide');
      $("#confirmEditModal").modal('show');
    }
  }

}
