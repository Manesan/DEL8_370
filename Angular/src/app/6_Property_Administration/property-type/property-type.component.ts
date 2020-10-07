import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-property-type',
  templateUrl: './property-type.component.html',
  styleUrls: ['./property-type.component.scss']
})

export class PropertyTypeComponent implements OnInit {
  public propertyTypes;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public propertyTypeid: any;
  public descriptionInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAddSuccess() {
    this.toastr.success('Property type added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Property type updated successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Property type deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('Property type cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.propertyTypes = await this.service.Get('/propertytype?token=' + this.token.token);
    //console.log(this.propertytypess);
    this.showViewModal=false;
    this.descriptionInput=null;
}

async view(id)
{
  $("#editModal").modal('show');
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(id);
  let propertyType = await this.service.Get('/propertytype?token=' + this.token.token + '&id='+ id) as any;
  //console.log(propertytype);
  this.descriptionInput = propertyType.PROPERTYTYPEDESCRIPTION;
  this.propertyTypeid = propertyType.PROPERTYTYPEID;
  this.showViewModal = true;
  //console.log(this.descriptionInput);
}

async update(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  console.log(this.descriptionInput);
  await this.service.Patch(`/propertytype?token=${this.token.token}&id=${id}&description=${this.descriptionInput}`);
  //this.showViewModal = false;
  this.showUpdateSuccess();
}

async add()
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(this.descriptionInput);
  await this.service.Post(`/propertytype?token=${this.token.token}&description=${this.descriptionInput}`);
  this.showAddSuccess();
}

async deleteBinding(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let propertyType = await this.service.Get('/propertytype?token=' + this.token.token + '&id='+ id) as any;
  this.descriptionInput = propertyType.PROPERTYTYPEDESCRIPTION;
  this.propertyTypeid = propertyType.PROPERTYTYPEID;
  //console.log(this.descriptionInput);
}

async delete(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let response = await this.service.Delete('/propertytype?token=' + this.token.token + '&id='+ id);
  console.log(response)
  if (response === 409){
    this.showDeleteFailure();
  }
  else{
    this.showDeleteSuccess();
  }
}


async confirmAccept(){
  $("#confirmAddModal").modal('show');
  $("#addModal").modal('hide');
}

async confirmUpdate(){
  $("#confirmEditModal").modal('show');
  $("#editModal").modal('hide');
}

}

