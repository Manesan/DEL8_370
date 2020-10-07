import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-view-client-purchases',
  templateUrl: './view-client-purchases.component.html',
  styleUrls: ['./view-client-purchases.component.scss']
})
export class ViewClientPurchasesComponent implements OnInit {
  public saleAgreements;
  public purchases; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public propertyid: any;
  public purchaseofferid: any;
  public offerstatus: any;
  public offerdescription: any;
  public offercomment: any;
  public offerdate: any;
  public offeramount: any;
  public propertyaddress: any;
  public accepted: boolean;
  public saleamount: any;
  public saledateconcluded: any;
  public toBeMaintained: boolean;
  public salestatus: any;
  public saledocument: any;
  public saleid: any;

  //Sale Agreement Document
  public fileBase64saleagreement: string;
  public fileExtensionsaleagreement: string;
  resultssale: any;
  documenttype: string;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAcceptSuccess() {
    this.toastr.success('You have accepted your sale agreement', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeclineSuccess() {
    this.toastr.success('You have declined your sale agreement', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }


  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.purchases = await this.service.Get('/purchaseoffer?token=' + this.token.token);
    //console.log(this.purchases);
    this.purchases.forEach(e => {
      e.OFFERDATE = e.OFFERDATE.split("T")[0];
    });
    console.log(this.purchases)

    this.showViewModal = false;
    this.offerdescription = null;
    this.offercomment = null;
    this.propertyid =null;
    this.propertyaddress = null;
    this.offerdate =null;
    this.offeramount = null;
    this.offerstatus = null;
    this.purchaseofferid =null;
    //location.reload.bind(location);
    this.saleAgreements = await this.service.Get('/saleagreement?token=' + this.token.token);
    console.log(this.saleAgreements);
    this.saleAgreements.forEach(e => {
      e.SALEDATECONCLUDED = e.SALEDATECONCLUDED.split("T")[0];
    });

    this.showViewModal = false;
    this.saleamount = null;
    this.saledateconcluded =null;
    this.toBeMaintained = false;

  }


  //SALE AGREEMENT DOCUMENT CONVERSION
  saleAgreementChangeListener($event){
    this.readThisSaleDocument($event.target);

    let img: any = document.getElementById("SaleAgreement");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultssale = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisSaleDocument(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensionsaleagreement = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64saleagreement = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }

  async view(id)
  {
    this.purchaseofferid = id;
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#purchaseOfferModal").modal('show');
    //console.log(id);
    let purchases = await this.service.Get('/purchaseoffer?token=' + this.token.token + '&id='+ id) as any;
    this.offerdescription = purchases.OFFERDESCRIPTION;
    this.offerdescription = purchases.OFFERCOMMENT;
    this.propertyid = purchases.PROPERTYID;
    this.propertyaddress = purchases.PROPERTYADDRESS;
    this.offerdate = purchases.OFFERDATE.split("T")[0];;
    this.offeramount = purchases.OFFERAMOUNT;
    this.offerstatus = purchases.PURCHASEOFFERSTATUSDESCRIPTION;



    this.showViewModal = true;
    //console.log(this.showViewModal);
  }

  async viewsale(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#saleAgreementModal").modal('show');
    //console.log(id);
    let saleAgreements = await this.service.Get('/saleagreement?token=' + this.token.token + '&id='+ id) as any;
    console.log(saleAgreements)
    this.saleid = saleAgreements.SALEID;
    this.offerdescription = saleAgreements.OFFERDESCRIPTION;
    this.propertyid = saleAgreements.PROPERTYID;
    this.propertyaddress = saleAgreements.PROPERTYADDRESS;
    this.saledateconcluded = saleAgreements.OFFERDATE;
    //.split("T")[0];
    this.salestatus = saleAgreements.PURCHASEOFFERSTATUSDESCRIPTION;
    this.offeramount = saleAgreements.SALEAMOUNT;
    this.saledocument = saleAgreements.SALEAGREEMENTDOCUMENT;

    this.documenttype = "SaleAgreement";
    this.saledocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    //console.log(this.saledocument)
    if(this.salestatus == "Pending Client Sale Agreement Acceptance"){
      this.toBeMaintained = true;
    }
  }

  getSaleAgreement(){
    const linkSource = 'data:application/pdf;base64,' + this.saledocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Sale Agreement ' + this.propertyaddress;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }


  async openAccept(id){
    $("#saleAgreementMaintainModal").modal('hide');
    $("#confirmAcceptanceModal").modal('show');
    //console.log(id);
  }


  async openReject(id){
    $("#saleAgreementMaintainModal").modal('hide');
    $("#confirmRejectionModal").modal('show');
    //console.log(id);
  }


  async accept(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    //console.log(id);
    //console.log(this.note);

    let saleagreementdocument = {
      "FileBase64" : this.fileBase64saleagreement,
      "FileExtension" : this.fileExtensionsaleagreement
    };
    //console.log(saleagreementdocument);

    this.accepted = true;
    //console.log(this.accepted)
    $("#confirmAcceptanceModal").modal('hide');
    await this.service.Patch(`/saleagreement?token=${this.token.token}&id=${id}&accepted=${this.accepted}`, saleagreementdocument);
    this.showAcceptSuccess();
    //console.log(this.rentalagreement);
  }

  async reject(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = false;
    $("#confirmRejectionModal").modal('hide');

    let saleagreementdocument = {
      "FileBase64" : "",
      "FileExtension" : ""
    };

    await this.service.Patch(`/saleagreement?token=${this.token.token}&id=${id}&accepted=${this.accepted}`, saleagreementdocument);
    this.showDeclineSuccess();
    //console.log(id);
    //console.log(this.note);
  }
}
