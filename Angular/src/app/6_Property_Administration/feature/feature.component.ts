import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-feature',
  templateUrl: './feature.component.html',
  styleUrls: ['./feature.component.scss']
})

export class FeatureComponent implements OnInit {
  public features; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public featureid: any;
  public descriptionInput: any;



  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAddSuccess() {
    this.toastr.success('Feature added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  showUpdateSuccess() {
    this.toastr.success('Feature updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Feature deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  showDeleteFailure() {
    this.toastr.error('Feature cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }


  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.features = await this.service.Get('/feature?token=' + this.token.token);
    console.log(this.features);

    this.showViewModal = false;
    this.descriptionInput = null;
    //location.reload.bind(location);
  }

  async add(){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.descriptionInput);
    await this.service.Post(`/feature?token=${this.token.token}&description=${this.descriptionInput}`);
    this.showAddSuccess();
  }

  async view(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#editModal").modal('show');
    //console.log(id);
    let feature = await this.service.Get('/feature?token=' + this.token.token + '&id='+ id) as any;
    this.descriptionInput = feature.FEATUREDESCRIPTION;
    this.featureid = feature.FEATUREID;

    this.showViewModal = true;
    //console.log(this.showViewModal);
  }

  async update(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/feature?token=${this.token.token}&id=${id}&description=${this.descriptionInput}`);
    this.showUpdateSuccess();
    this.showViewModal = false;
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let feature = await this.service.Get('/feature?token=' + this.token.token + '&id='+ id) as any;
    this.descriptionInput = feature.FEATUREDESCRIPTION;
    this.featureid = feature.FEATUREID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let response = await this.service.Delete('/feature?token=' + this.token.token + '&id='+ id);
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
