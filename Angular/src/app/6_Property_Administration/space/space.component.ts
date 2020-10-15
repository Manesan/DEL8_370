import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-space',
  templateUrl: './space.component.html',
  styleUrls: ['./space.component.scss']
})
export class SpaceComponent implements OnInit {
  public spaces;
  public spacetypes;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public spaceid: any;
  public descriptionInput: any;
  public spacetypeid: any;
  public spacetypeInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showAddSuccess() {
    this.toastr.success('Space  added successfully', "", {
      timeOut: 1000,
    });
    //setTimeout(location.reload.bind(location), 1000);
    this.ngOnInit();
  }

  showUpdateSuccess() {
    this.toastr.success('Space updated successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);

  }

  showDeleteSuccess()
   {
    this.toastr.success('Space deleted successfully', "", {
      timeOut: 1000,
    });
    //setTimeout(location.reload.bind(location), 1000);
    this.ngOnInit();
  }

  showDeleteFailure() {
    this.toastr.error('Space cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit()
   {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.spaces = await this.service.Get('/space?token=' + this.token.token);
    this.spacetypes= await this.service.Get('/spacetype?token='+ this.token.token);
    //console.log(this.spacetypes);
    //this.showViewModal=false;
    this.descriptionInput=null;
    this.spacetypeInput=null;
    this.spinner.hide();
}

async view(id)
{
  this.spinner.show();
  $("#editModal").modal('show');
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(id);
  let space = await this.service.Get('/space?token=' + this.token.token + '&id='+ id) as any;
  //console.log(spacetype);
  this.descriptionInput = space.SPACEDESCRIPTION;
  this.spaceid = space.SPACEID;
  this.spacetypeid = space.SPACETYPEID;
  this.spacetypeInput = space.SPACETYPEDESCRIPTION;
  this.showViewModal = true;
  //console.log(this.descriptionInput);
    this.spinner.hide();
}

async update(id)
{
  this.spinner.show();
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  $("#confirmEditModal").modal('hide');
  //console.log(id);
  await this.service.Patch(`/space?token=${this.token.token}&id=${id}&description=${this.descriptionInput}&spacetypeid=${this.spacetypeid}`);
  this.showViewModal = false;
  this.spinner.hide();
  this.showUpdateSuccess();
}

async add()
{
  this.spinner.show();
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(this.descriptionInput);
  await this.service.Post(`/space?token=${this.token.token}&description=${this.descriptionInput}&spacetypeid=${this.spacetypeid}`);
  this.spinner.hide();
  this.showAddSuccess();
}

async deleteBinding(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let space = await this.service.Get('/space?token=' + this.token.token + '&id='+ id) as any;
  this.descriptionInput = space.SPACEDESCRIPTION;
  this.spaceid =space.SPACEID;
  //console.log(this.descriptionInput);
}

async delete(id)
{
  this.spinner.show();
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let response = await this.service.Delete('/space?token=' + this.token.token + '&id='+ id);
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
    $("#editModal").modal('hide');
    $("#confirmEditModal").modal('show');
}
}
