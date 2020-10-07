import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';


declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-space-types',
  templateUrl: './space-types.component.html',
  styleUrls: ['./space-types.component.scss']
})

export class SpaceTypesComponent implements OnInit {
  public spaceTypes;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public spaceTypeid: any;
  public descriptionInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAddSuccess() {
    this.toastr.success('Space type added successfully', "", {
      timeOut: 1000,
    });
    //setTimeout(location.reload.bind(location), 1000);
    this.ngOnInit();
  }

  showUpdateSuccess() {
    this.toastr.success('Space type updated successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
    
  }

  showDeleteSuccess() {
    this.toastr.success('Space type deleted successfully', "", {
      timeOut: 1000,
    });
    //setTimeout(location.reload.bind(location), 1000);
    this.ngOnInit();
  }

  showDeleteFailure() {
    this.toastr.error('Space type cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.spaceTypes = await this.service.Get('/spacetype?token=' + this.token.token);
    //console.log(this.spacetypes);
    //this.showViewModal=false;
    this.descriptionInput=null;
}

async view(id)
{
  $("#editModal").modal('show');
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(id);
  let spaceType = await this.service.Get('/spacetype?token=' + this.token.token + '&id='+ id) as any;
  //console.log(spacetype);
  this.descriptionInput = spaceType.SPACETYPEDESCRIPTION;
  this.spaceTypeid = spaceType.SPACETYPEID;
  this.showViewModal = true;
  //console.log(this.descriptionInput);
}

async update(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(id);
  await this.service.Patch(`/spacetype?token=${this.token.token}&id=${id}&description=${this.descriptionInput}`);
  this.showViewModal = false;
  this.showUpdateSuccess();
}

async add()
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(this.descriptionInput);
  await this.service.Post(`/spacetype?token=${this.token.token}&description=${this.descriptionInput}`);
  this.showAddSuccess();
}

async deleteBinding(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let spaceType = await this.service.Get('/spacetype?token=' + this.token.token + '&id='+ id) as any;
  this.descriptionInput = spaceType.SPACETYPEDESCRIPTION;
  this.spaceTypeid =spaceType.SPACETYPEID;
  //console.log(this.descriptionInput);
}

async delete(id)
{
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  let response = await this.service.Delete('/spacetype?token=' + this.token.token + '&id='+ id);
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
