import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-agent-purchase-offers',
  templateUrl: './agent-purchase-offers.component.html',
  styleUrls: ['./agent-purchase-offers.component.scss']
})
export class AgentPurchaseOffersComponent implements OnInit {
  public purchaseOffers: any;//holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
   public address:any;
   public date: any;
   public amount: any

   public purchaseofferid:any;
   public propertyid: any;

   public accepted: boolean;
   public note: any;
   public saleagreementdocument: any;

  //Sale Agreement Document
  public fileBase64saledocument: string;
  public fileExtensionsaledocument: string;
  resultssale: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAcceptSuccess() {
    this.toastr.success('Purchase offer accepted successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showRejectSuccess() {
    this.toastr.success('Purchase offer rejected successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.purchaseOffers = await this.service.Get('/agentpurchaseoffer?token=' + this.token.token);
    console.log(this.purchaseOffers);

    this.showViewModal = false;
    this.address = null;
    //this.date = null;
    this.amount = null;
    this.purchaseOffers.forEach(i => {
      i.OFFERDATE = i.OFFERDATE.split("T")[0];
    });
    //location.reload.bind(location);
  }

  async view(id){
    $("#viewModal").modal('show');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let agentpurchaseoffer = await this.service.Get('/agentpurchaseoffer?token=' + this.token.token + '&id='+ id) as any;
    console.log(agentpurchaseoffer);

    this.purchaseofferid = id;
    this.amount = agentpurchaseoffer.OFFERAMOUNT;
    this.date = agentpurchaseoffer.OFFERDATE.split("T")[0];

    this.propertyid = agentpurchaseoffer.PROPERTYID;
    this.address = agentpurchaseoffer.PROPERTYADDRESS;

    //this.showViewModal = true;
    console.log(this.purchaseofferid)
  }

  async openAccept(id){
    if(this.note != ""){
    $("#viewModal").modal('hide');
    $("#confirmAcceptanceModal").modal('show');
    }
    //console.log(id);
  }

  async openReject(id){
    $("#viewModal").modal('hide');
    $("#confirmRejectionModal").modal('show');
    //console.log(id);
  }

  //SALE DOCUMENT CONVERSION
  saleAgreementChangeListener($event){
    this.readThisSaleDocument($event.target);

    let img: any = document.getElementById("saleagreementdocument");
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
     this.fileExtensionsaledocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64saledocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }

  async accept(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};

    let saleagreement = {
      "FileBase64" : this.fileBase64saledocument,
      "FileExtension" : this.fileExtensionsaledocument
    };
    //console.log(saleagreement);

    this.accepted = true;
    await this.service.Patch(`/agentpurchaseoffer?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`, saleagreement);
    this.showAcceptSuccess();
  }

  async reject(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.accepted = false;
    $("#confirmRejectionModal").modal('hide');
    console.log(id);
    await this.service.Patch(`/agentpurchaseoffer?token=${this.token.token}&id=${id}&accepted=${this.accepted}&note=${this.note}`);
    this.showRejectSuccess();
  }

  //Accept form validation
  async submitAccept(){
    if(this.saleagreementdocument != "" ){
      //console.log(this.descriptionInput);
      $("#assignModal").modal('hide');
      $("#confirmCaptureAssignmentModal").modal('show');
    }
  }


}
