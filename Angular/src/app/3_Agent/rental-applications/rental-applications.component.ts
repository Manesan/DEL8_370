import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-rental-applications',
  templateUrl: './rental-applications.component.html',
  styleUrls: ['./rental-applications.component.scss']
})
export class RentalApplicationsComponent implements OnInit {
 // public rentalApplications: any;//holds list to populate cards
 public pendingRentalapplication: any;//holds list to populate cards
 public approvedRentalApplications: any;//holds list to populate cards
 public rejectedRentalApplications: any;//holds list to populate cards
 public agentExtensionRentalApplications: any;//holds list to populate cards
 public agentRenewalRentalApplications: any;//holds list to populate cards
 public clientRejectionRentalApplications: any;//holds list to populate cards
 public approvedExtensionRentalApplications: any;//holds list to populate cards
 public approvedRenewalRentalApplications: any;//holds list to populate cards

 public showViewModal: boolean; //bool for view modal
 public token: any; //holds user token

  public address:any;
  public date:any;
  public name:any;
  public surname:any;
  public email:any;
  public contact:any;
  public rentalid:any;
  public clientid: any;
  public accepted: boolean;
  public note: any;
  public status: any;
  public rentalagreement: any;
  public rentalapplicationdocument: any;
  public propertyid: any;
  public term: any;
  public suburb: any;
  public clientdocuments: any;
  public clientDocument: any;

   //Rental Agreement Document
   public fileBase64rentalagreement: string;
   public fileExtensionrentalagreement: string;
   resultsrental: any;
   documenttype: string;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAcceptSuccess() {
    this.toastr.success('Rental application accepted successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showRejectSuccess() {
    this.toastr.success('Rental application rejected successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.pendingRentalapplication = await this.service.Get('/agentrentalapplication?token=' + this.token.token);
    this.approvedRentalApplications = await this.service.Get('/agentrentalapplication2?token=' + this.token.token);
    this.rejectedRentalApplications = await this.service.Get('/agentrentalapplication3?token=' + this.token.token);
    this.agentExtensionRentalApplications = await this.service.Get('/agentrentalapplication4?token=' + this.token.token);
    this.agentRenewalRentalApplications = await this.service.Get('/agentrentalapplication5?token=' + this.token.token);
    this.clientRejectionRentalApplications = await this.service.Get('/agentrentalapplication6?token=' + this.token.token);
    this.approvedExtensionRentalApplications = await this.service.Get('/agentrentalapplication7?token=' + this.token.token);
    this.approvedRenewalRentalApplications = await this.service.Get('/agentrentalapplication8?token=' + this.token.token);
    
    console.log(this.pendingRentalapplication,this.approvedRentalApplications, this.rejectedRentalApplications, this.agentExtensionRentalApplications,
      this.agentRenewalRentalApplications, this.clientRejectionRentalApplications, this.approvedExtensionRentalApplications, this.approvedRenewalRentalApplications  );
    this.showViewModal = false;
    //this.descriptionInput = null;
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
    $("#viewModal").modal('show');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    let rentalapplication = await this.service.Get('/agentrentalapplication?token=' + this.token.token + '&id='+ id) as any;
    console.log(rentalapplication);
    this.rentalid = rentalapplication.RENTALAPPLICATIONID;
    this.clientid = rentalapplication.USERID;
    this.propertyid = rentalapplication.PROPERTYID;
    this.name = rentalapplication.USERNAME;
    this.surname = rentalapplication.USERSURNAME;
    this.contact = rentalapplication.USERCONTACTNUMBER;
    this.email = rentalapplication.USEREMAIL;
    this.date = rentalapplication.RENTALAPPLICATIONDATE;
    this.address = rentalapplication.PROPERTYADDRESS;
    this.status = rentalapplication.RENTALAPPLICATIONSTATUSDESCRIPTION;
    this.note = rentalapplication.RENTALAPPLICATIONNOTE;
    this.rentalapplicationdocument = rentalapplication.RENTALAPPLICATIONDOCUMENT;
    this.suburb = rentalapplication.SUBURBNAME;
    this.clientdocuments = rentalapplication.ClientDocuments;

    this.documenttype = "RentalApplication";
    this.rentalapplicationdocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;


    this.term = rentalapplication.TERMDESCRIPTION + " months";
  }

  async openAccept(id){

    $("#viewModal").modal('hide');
    $("#confirmAcceptanceModal").modal('show');

    //console.log(id);
  }


  async openReject(id){
    $("#viewModal").modal('hide');
    $("#confirmRejectionModal").modal('show');
    //console.log(id);
  }

  getRentalApplication(){
    const linkSource = 'data:application/pdf;base64,' + this.rentalapplicationdocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Rental Application ' + this.address;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async getClientDocument(id){

    this.documenttype = "ClientDocument";
    this.clientDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    const linkSource = 'data:application/pdf;base64,' + this.clientDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Client Document ' + this.name + this.surname ;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
   }


  async accept(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    let rentalagreementdocument = {
      "FileBase64" : this.fileBase64rentalagreement,
      "FileExtension" : this.fileExtensionrentalagreement
    };
    //console.log(rentalagreementdocument);

    //console.log(id);
    //console.log(this.note);
    this.accepted = true;
    $("#confirmAcceptanceModal").modal('hide');
    await this.service.Patch(`/agentrentalapplication?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`, rentalagreementdocument);
    this.showAcceptSuccess();
    //console.log(this.rentalagreement);
  }

  async reject(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = false;
    $("#confirmRejectionModal").modal('hide');
    let rentalagreementdocument = {
      "FileBase64" : "",
      "FileExtension" : ""
    };
    await this.service.Patch(`/agentrentalapplication?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`, rentalagreementdocument);
    this.showRejectSuccess();

  }

}
