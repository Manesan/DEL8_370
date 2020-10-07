import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-rental-agreement-extensions',
  templateUrl: './rental-agreement-extensions.component.html',
  styleUrls: ['./rental-agreement-extensions.component.scss']
})
export class RentalAgreementExtensionsComponent implements OnInit {
  public concludedRentalAgreements: any;//holds list to populate cards
  public rentedRentalAgreements: any;//holds list to populate cards
  public cancelRentalAgreements: any;//holds list to populate cards
  public pendingClientAcceptanceRentalAgreements: any;//holds list to populate cards
  public rentalRenewedRentalAgreements: any;//holds list to populate cards
  public rentalExtendedRentalAgreements: any;//holds list to populate cards
  public pendingAgentExtensionRentalAgreements: any; //holds list to populate cards
  public pendingClientExtensionRentalAgreements: any;//holds list to populate cards
  public pendingClientRenewalRentalAgreements: any;//holds list to populate cards

  // public rentalAgreementExtensions:any; //holds list to populate cards
  // public cancelRentalAgreements:any;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token
  public accepted: boolean;

  public address:any;
  public startdate:any;
  public enddate: any;
  public rentalstatus: any;
  public name:any;
  public surname:any;
  public email:any;
  public contact:any;
  public comment: any;
  public status:any;
  public note: any;
  public rentalAgreementDocument: any;
  public user: any;

  public rentalid:any;
  public rentalapplicationid: any;
  public rentalstatusid: any;
  public propertyid: any;
  public clientid: any;
  documenttype: string;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showExtendSuccess() {
    this.toastr.success('Rental agreement extended successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showRejectExtensionSuccess() {
    this.toastr.success('Rental agreement extension rejected successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};

    this.concludedRentalAgreements = await this.service.Get('/agentrentalagreement?token=' + this.token.token); //status 1 --fine
    this.rentedRentalAgreements = await this.service.Get('/agentrentalagreement2?token=' + this.token.token); //status 2 --fine
    this.cancelRentalAgreements = await this.service.Get('/rentalagreementterminations?token=' + this.token.token); //status 3 --fine
    this.pendingClientAcceptanceRentalAgreements = await this.service.Get('/agentrentalagreement4?token=' + this.token.token); //status 4
    this.rentalRenewedRentalAgreements = await this.service.Get('/agentrentalagreement5?token=' + this.token.token); //status 5
    this.rentalExtendedRentalAgreements = await this.service.Get('/agentrentalagreement6?token=' + this.token.token); //status 6
    this.pendingClientExtensionRentalAgreements = await this.service.Get('/agentrentalagreement7?token=' + this.token.token); //status 7
    this.pendingClientRenewalRentalAgreements = await this.service.Get('/agentrentalagreement8?token=' + this.token.token); //status 8 --fine

    this.pendingAgentExtensionRentalAgreements = await this.service.Get('/rentalagreementextensions?token=' + this.token.token); //pending agent extensions

    console.log(this.concludedRentalAgreements, this.rentedRentalAgreements, this.cancelRentalAgreements, this.pendingClientAcceptanceRentalAgreements,
      this.rentalRenewedRentalAgreements, this.pendingClientExtensionRentalAgreements, this.pendingClientRenewalRentalAgreements);
    //this.showViewModal = false;
    //this.descriptionInput = null;
  }

  async view(id){
    $("#viewModal").modal('show');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    console.log(id);
    let agentrentalagreement = await this.service.Get('/agentrentalagreement?token=' + this.token.token + '&id='+ id) as any;
    console.log(agentrentalagreement);

    this.propertyid = agentrentalagreement.PROPERTYID;
    this.address = agentrentalagreement.PROPERTYADDRESS;

    this.rentalid = agentrentalagreement.RENTALAPPLICATIONID;
    this.startdate = agentrentalagreement.RENTALDATESTART.split("T")[0];
    this.enddate = agentrentalagreement.RENTALDATEEND.split("T")[0];
    this.rentalAgreementDocument = agentrentalagreement.RentalAgreementDocument.RENTALAGREEMENTDOCUMENT;

    this.documenttype = "RentalAgreement";
    this.rentalAgreementDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    //console.log(this.rentalAgreementDocument);
    //this.rentalstatusid = agentrentalagreement.RENTALSTATUSID;
    this.status = agentrentalagreement.RENTALSTATUSDESCRIPTION;

    this.clientid = agentrentalagreement.USERID;
    this.name = agentrentalagreement.USERNAME;
    this.surname = agentrentalagreement.USERSURNAME;
    this.contact = agentrentalagreement.USERCONTACTNUMBER;
    this.email = agentrentalagreement.USEREMAIL;
    //console.log(this.descriptionInput);
  }






  //  RENTAL AGREEMENTS TO EXTEND
  async viewExtend(id){
    $("#extendModal").modal('show');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    let rentalagreementextensions = await this.service.Get('/rentalagreementextensions?token=' + this.token.token + '&id='+ id) as any;
    //console.log(rentalagreementextensions);

    this.propertyid = rentalagreementextensions.PROPERTYID;
    this.address = rentalagreementextensions.PROPERTYADDRESS;

    this.rentalid = rentalagreementextensions.RENTALAPPLICATIONID;
    //console.log(this.rentalid);
    this.rentalAgreementDocument = rentalagreementextensions.RENTALAPPLICATIONDOCUMENT;

    //console.log(this.rentalAgreementDocument)

    this.rentalstatusid = rentalagreementextensions.RENTALSTATUSID;
    this.status = rentalagreementextensions.RENTALSTATUSDESCRIPTION;

    this.clientid = rentalagreementextensions.USERID;
    this.user = rentalagreementextensions.USERNAME + " " + rentalagreementextensions.USERSURNAME;
    this.contact = rentalagreementextensions.USERCONTACTNUMBER;
    this.email = rentalagreementextensions.USEREMAIL;
    //this.showViewModal = true;
    //console.log(this.descriptionInput);
  }

  showAcceptSuccess() {
    this.toastr.success('Rental agreement extension accepted successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showRejectSuccess() {
    this.toastr.success('Rental agreement extension rejected successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  async openAccept(id){
    if(this.note != ""){
    $("#extendModal").modal('hide');
    $("#confirmAcceptExtendModal").modal('show');
    }
    //console.log(id);
  }

  async openReject(id){
    $("#extendModal").modal('hide');
    $("#confirmRejectExtendModal").modal('show');
    //console.log(id);
  }

  getRentalAgreement(){
    const linkSource = 'data:application/pdf;base64,' + this.rentalAgreementDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Rental Agreement ' + this.address;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async accept(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = true;
    $("#confirmAcceptExtendModal").modal('hide');
    await this.service.Patch(`/agentrentalapplication?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`);
    this.showAcceptSuccess();
  }

  async reject(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = false;
    $("#confirmRejectExtendModal").modal('hide');
    await this.service.Patch(`/agentrentalapplication?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`);
    this.showRejectSuccess();
  }




  //RENTAL AGREEMENTS TO CANCEL
  async viewCancel(id){
    $("#viewCancelModal").modal('show');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    let rentalagreementterminations = await this.service.Get('/rentalagreementterminations?token=' + this.token.token + '&id='+ id) as any;
    //console.log(rentalagreementterminations);

    this.propertyid = rentalagreementterminations.PROPERTYID;
    this.address = rentalagreementterminations.PROPERTYADDRESS;

    this.status = rentalagreementterminations.RENTALSTATUSDESCRIPTION;
    this.rentalid = id;
    console.log(this.rentalid)
    this.startdate = rentalagreementterminations.RENTALDATESTART.split("T")[0];
    this.enddate = rentalagreementterminations.RENTALDATEEND.split("T")[0];
    this.rentalAgreementDocument = rentalagreementterminations.RentalAgreementDocument.RENTALAGREEMENTDOCUMENT;

    this.documenttype = "RentalAgreement";
    this.rentalAgreementDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    //this.comment = rentalagreementterminations.RENTALAPPLICATIONSTATUSDESCRIPTION;
    this.clientid = rentalagreementterminations.USERID;
    this.name = rentalagreementterminations.USERNAME;
    this.surname = rentalagreementterminations.USERSURNAME;
    this.contact = rentalagreementterminations.USERCONTACTNUMBER;
    this.email = rentalagreementterminations.USEREMAIL;
    //this.showViewModal = true;
    //console.log(this.descriptionInput);
  }

  showCancelSuccess() {
    this.toastr.success('Rental agreement canceled successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let rentalagreementterminations = await this.service.Get('/rentalagreementterminations?token=' + this.token.token + '&id='+ id) as any;
    this.rentalid = rentalagreementterminations.RENTALID;
  }

  async delete(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    await this.service.Delete('/rentalagreementterminations?token=' + this.token.token + '&id='+ this.rentalid)
    this.showCancelSuccess();
  }

}
