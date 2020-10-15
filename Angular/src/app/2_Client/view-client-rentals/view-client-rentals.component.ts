import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

declare function agreements(statusid): any;

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-view-client-rentals',
  templateUrl: './view-client-rentals.component.html',
  styleUrls: ['./view-client-rentals.component.scss']
})
export class ViewClientRentalsComponent implements OnInit {
  public rentalAgreements;
  public rentalApplications; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public propertyid: any;
  public rentalapplicationid: any;
  public rentalapplicationdate: any;
  public rentalaplicationterm: any;
  public rentalapplicationstatus: any;
  public rentalapplicationcomment: any;
  public propertyaddress: any;
  public accepted: boolean;

  public name: any;
  public surname: any;
  public userid: any;
  public email: any;
  public contact: any;

  public rentalstart: any;
  public rentalend: any;
  public rentalstatus: any;
  public rentalid: any;
  public statusid: any;
  public rentaldocument: any;
  public needToMaintain: boolean;
  public concluded: boolean;
  public terminate: boolean;

   //Rental Agreement Document
   public fileBase64rentalagreement: string;
   public fileExtensionrentalagreement: string;
   resultsrental: any;
  documenttype: string;


  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showTerminationNoticeSuccess()
  {
    this.toastr.success('You have submitted your notice of rental agreement termination', "",
     {
      timeOut: 2000,
    });
    setTimeout(location.reload.bind(location), 2000);
  }


  showAgreementExtensionSuccess()
  {
    this.toastr.success('You have submitted your rental agreement extension request', "", {
      timeOut: 2000,
    });
    setTimeout(location.reload.bind(location), 2000);
  }

  showAcceptSuccess()
  {
    this.toastr.success('You have submitted your rental agreement extension request', "", {
      timeOut: 2000,
    });
    setTimeout(location.reload.bind(location), 2000);
  }

  showRejectSuccess()
   {
    this.toastr.success('You have submitted your rental agreement extension request', "", {
      timeOut: 2000,
    });
    setTimeout(location.reload.bind(location), 2000);
  }


  async submit(){

  }
  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.rentalApplications = await this.service.Get('/rentalapplication?token=' + this.token.token);
    console.log(this.rentalApplications);

    this.rentalApplications.forEach(e => {
      e.RENTALAPPLICATIONDATE = e.RENTALAPPLICATIONDATE.split("T")[0];
    });

    this.showViewModal = false;
    this.rentalaplicationterm = null;
    this.propertyid =null;
    this.propertyaddress = null;
    this.rentalapplicationdate =null;
    this.rentalapplicationstatus = null;
    this.rentalapplicationcomment = null;
    this.rentalapplicationid = null;
    this.name= null;
    this.surname= null;
    this.userid= null;
    this.email= null;
    this.contact= null;
    this.needToMaintain = false;
    this.concluded = false;
    this.terminate = false;

    this.rentalAgreements = await this.service.Get('/rentalagreement?token=' + this.token.token);
   // console.log(this.rentalAgreements);

    this.showViewModal = false;
    this.rentalstart = null;
    this.rentalend = null;
    this.rentalstatus = null;
    this.rentalid = null;
    this.statusid = null;
    this.spinner.hide();
  }


  //RENTAL AGREEMENT DOCUMENT CONVERSION
rentalAgreementChangeListener($event){
  this.readThisRentalAgreement($event.target);

  let img: any = document.getElementById("RentalAgreement");
  if(typeof (FileReader) !== 'undefined') {
    let reader = new FileReader();

    reader.onload = (e:any) => {
      this.resultsrental = e.target.result;
    }
    reader.readAsArrayBuffer(img.files[0]);
  }
 }
 //Method that reads the file information and parses it
 readThisRentalAgreement(inputValue: any): void{
   //Gets the actual file
   var file: File = inputValue.files[0];
   //Creates a reader that will read the file for information
   var myReader: FileReader = new FileReader();

   //Get the extension by splitting the name and popping the name off
   this.fileExtensionrentalagreement = file.name.split('.').pop();

   //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
   myReader.onloadend = (e) => {
     this.fileBase64rentalagreement = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
     //console.log(this.fileBase64);
   }
   //Read the file in and parse it to Base64
   myReader.readAsDataURL(file);
 }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#rentalApplicationModal").modal('show');
    //console.log(id);
    let rentalapplications = await this.service.Get('/rentalapplication?token=' + this.token.token + '&id='+ id) as any;
    this.propertyid = rentalapplications.PROPERTYID;
    this.propertyaddress = rentalapplications.PROPERTYADDRESS;
    this.rentalaplicationterm = rentalapplications.RENTALAPPLICATIONTERM;
    this.rentalapplicationdate = rentalapplications.RENTALAPPLICATIONDATE.split("T")[0];
    this.rentalapplicationstatus = rentalapplications.RENTALAPPLICATIONSTATUSDESCRIPTION;
    this.rentalapplicationcomment = rentalapplications.RENTALAPPLICATIONCOMMENT;
    //console.log(this.rentalapplicationdate);
    this.rentalapplicationid = rentalapplications.RENTALAPPLICATIONID;
    this.name= rentalapplications.USERNAME;
    //console.log(this.name);
    this.surname= rentalapplications.USERSURNAME;
    this.userid= rentalapplications.USERID;
    this.email= rentalapplications.USEREMAIL;
    this.contact= rentalapplications.USERCONTACTNUMBER;
    this.rentalstatus = rentalapplications.RENTALSTATUSDESCRIPTION;
    this.spinner.hide();
  }

  async viewagreement(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#rentalAgreementModal").modal('show');
    //console.log(id);
    let rentalAgreements = await this.service.Get('/rentalagreement?token=' + this.token.token + '&id='+ id) as any;
    //console.log(rentalAgreements);
    this.rentalid = rentalAgreements.RENTALID;
    this.propertyid = rentalAgreements.PROPERTYID;
    this.propertyaddress = rentalAgreements.PROPERTYADDRESS;
    this.rentalstart = rentalAgreements.RENTALDATESTART.split("T")[0];
    // .split("T")[0];
    this.rentalend = rentalAgreements.RENTALDATEEND.split("T")[0];
    // .split("T")[0];
    this.rentalstatus = rentalAgreements.RENTALSTATUSDESCRIPTION;
    if(this.rentalstatus == "Pending Client Acceptance"){
      this.needToMaintain = true;
    }
    if(this.rentalstatus == "Pending Client Extension Acceptance"){
      this.needToMaintain = true;
    }
    if(this.rentalstatus == "Pending Client Renewal Acceptance"){
      this.needToMaintain = true;
    }
    if(this.rentalstatus == "Concluded"){
      this.concluded = true;
    }

    this.name= rentalAgreements.USERNAME;
    this.surname= rentalAgreements.USERSURNAME;
    this.userid= rentalAgreements.USERID ;
    this.email= rentalAgreements.USEREMAIL;
    this.contact= rentalAgreements.USERCONTACTNUMBER;
    this.statusid = rentalAgreements.RENTALSTATUSID;
    this.rentaldocument = rentalAgreements.RentalAgreement.RENTALAGREEMENTDOCUMENT;
    //console.log(this.rentaldocument)
    this.documenttype = "RentalAgreement";
    this.rentaldocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    //console.log(this.rentaldocument);

    this.spinner.hide();
    //agreements(this.statusid);
  }

  getRentalAgreement(){
    const linkSource = 'data:application/pdf;base64,' + this.rentaldocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Rental Agreement ' + this.propertyaddress;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async openAccept(id)
  {
    console.log(id);
    $("#confirmAcceptanceModal").modal('show');
    $("#rentalAgreementMaintainModal").modal('hide');

  }


  async openReject(id)
  {
    $("#confirmRejectionModal").modal('show');
    $("#rentalAgreementMaintainModal").modal('hide');
    //console.log(id);
  }


  async accept(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};

    //console.log(this.note);
    this.accepted = true;
    //console.log("true");

    let rentalagreementdocument = {
      "FileBase64" : this.fileBase64rentalagreement,
      "FileExtension" : this.fileExtensionrentalagreement
    };
    console.log(rentalagreementdocument);
    console.log(id);

    $("#confirmAcceptanceModal").modal('hide');
    await this.service.Patch(`/rentalagreement?token=${this.token.token}&id=${id}&accepted=${this.accepted}`, rentalagreementdocument);
    this.spinner.hide();
    this.showAcceptSuccess();
  }

  async reject(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = false;
    //console.log("false");

    let rentalagreementdocument = {
      "FileBase64" : "",
      "FileExtension" : ""
    };

    $("#confirmRejectionModal").modal('hide');
    await this.service.Patch(`/rentalagreement?token=${this.token.token}&id=${id}&accepted=${this.accepted}`, rentalagreementdocument);
    this.spinner.hide();
    this.showRejectSuccess();
    //console.log(id);
    //console.log(this.note);
  }


  async maintain(id){
    //console.log(id);
    $("#rentalAgreementMaintainModal").modal('show');
  }

  async termination(id){
    this.spinner.show();
    //console.log(id);
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.terminate = true;
    await this.service.Delete(`/rentalagreement?token=${this.token.token}&rentalid=${id}&terminate=${this.terminate}`);
    this.spinner.hide();
    this.showTerminationNoticeSuccess();
  }

  async extend(id){
    this.spinner.show();
    //console.log(id);
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    await this.service.Post(`/rentalagreement?token=${this.token.token}&rentalid=${id}&termid=${id}`);
    this.spinner.hide();
    this.showAgreementExtensionSuccess();
  }


}
