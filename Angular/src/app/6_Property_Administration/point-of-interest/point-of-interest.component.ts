import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-point-of-interest',
  templateUrl: './point-of-interest.component.html',
  styleUrls: ['./point-of-interest.component.scss']
})

export class PointOfInterestComponent implements OnInit {
  public pointsofinterest; //holds list to populate cards
  public pointofinteresttypes; //holds list to populate cards
  public suburbs; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public pointofinterestid: any;
  public nameInput: any;
  public typeid: any;
  public pointTypeNameInput: any;
  public suburbid: any;
  public suburbNameInput: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAddSuccess() {
    this.toastr.success('Point of interest added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Point of interest updated successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Point of interest deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.pointsofinterest = await this.service.Get('/pointofinterest?token=' + this.token.token);
    this.suburbs = await this.service.Get('/suburb?token=' + this.token.token);
    this.pointofinteresttypes = await this.service.Put('/pointofinterest?token=' + this.token.token);
    console.log(this.pointsofinterest);

    this.showViewModal = false;
    this.nameInput = null;
    //this.typeInput = null;
    //this.suburbInput = null;
    //location.reload.bind(location);
  }

  async add(){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.nameInput, this.suburbid, this.typeid);
    await this.service.Post(`/pointofinterest?token=${this.token.token}&name=${this.nameInput}&suburbid=${this.suburbid}&typeid=${this.typeid}`);
    this.showAddSuccess();
  }

  async view(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#editModal").modal('show');
    //console.log(id);
    let pointofinterest = await this.service.Get('/pointofinterest?token=' + this.token.token + '&id='+ id) as any;
    console.log(pointofinterest)
    this.nameInput = pointofinterest.POINTOFINTERESTNAME;
    this.pointTypeNameInput = pointofinterest.POINTOFINTERESTTYPEDESCRIPTION;
    this.suburbNameInput = pointofinterest.SUBURBNAME
    this.pointofinterestid = pointofinterest.POINTOFINTERESTID;
    this.typeid = pointofinterest.POINTOFINTERESTTYPEID;
    this.suburbid = pointofinterest.SUBURBID;

    this.showViewModal = true;
    console.log(this.pointTypeNameInput);
  }

  async update(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/pointofinterest?token=${this.token.token}&id=${id}&name=${this.nameInput}&typeid=${this.typeid}&suburbid=${this.suburbid}`);
    this.showUpdateSuccess();
    this.showViewModal = false;
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let pointofinterest = await this.service.Get('/pointofinterest?token=' + this.token.token + '&id='+ id) as any;
    this.nameInput = pointofinterest.POINTOFINTERESTNAME;
    this.pointofinterestid = pointofinterest.POINTOFINTERESTID;
    //console.log(this.descriptionInput);
  }

  async delete(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    await this.service.Delete('/pointofinterest?token=' + this.token.token + '&id='+ id)
    this.showDeleteSuccess();
  }

  //Add form validation
  async submitAdd(){
    if(this.nameInput !== null && this.pointTypeNameInput !== null && this.suburbNameInput !== null){
      //console.log(this.descriptionInput);
      $("#confirmAddModal").modal('show');
      $("#addModal").modal('hide');
    }
  }

  //Update form validation
    async submitUpdate(){
      if(this.nameInput != "" && this.pointTypeNameInput != "" && this.suburbNameInput != ""){
        //console.log(this.descriptionInput);
        $("#editModal").modal('hide');
        $("#confirmEditModal").modal('show');
      }
    }
}
