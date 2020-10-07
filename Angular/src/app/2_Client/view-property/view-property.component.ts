import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import {formatDate} from '@angular/common';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-view-property',
  templateUrl: './view-property.component.html',
  styleUrls: ['./view-property.component.scss']
})
export class ViewPropertyComponent implements OnInit {
  public rentalApplications;
  public id: any;
  public token: any; //holds user token

  public propertyid: any;
  public address: any;
  public price: any;
  public markettype: any;
  public propertytype: any;
  public bathrooms: any;
  public bedrooms: any;
  public parking: any;
  public suburb:any;
  public city: any;
  public province: any;
  public features: any;
  public otherbuildingdetails: any;
  public pointsofinterest: any;
  public term: any;
  public startdate: any;
  public dateToday: any;
  public forsale: any;
  public offeramount: any;

  public photos: any[] = [];

  public termid: any;
  public terms: any;
  //public applicationDocuments: Array<any>;

  //Rental document
  public fileBase64rentaldocument: string;
  public fileExtensionrentaldocument: string;
  resultsrental: any;

  //Bank document
  public fileBase64bankdocument: string;
  public fileExtensionbankdocument: string;
  resultsbank: any;

  //ID document
  public fileBase64iddocument: string;
  public fileExtensioniddocument: string;
  resultsid: any;
  documenttype: string;
  propertyListingPicture: any;
  pictureid: any;
  agentemail: any;
  agentsurname: any;
  agentname: any;

  constructor(public service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService,  private route:ActivatedRoute, private sanitizer: DomSanitizer) { }

  showRentApplySuccess() {
    this.toastr.success('You have successfully applied to rent this property', "", {
      timeOut: 1500,
    });
    setTimeout(location.reload.bind(location), 1500);
  }

  showPurchaseApplySuccess() {
    this.toastr.success('You have successfully made a purchase offer for this property', "", {
      timeOut: 1500,
    });
    setTimeout(location.reload.bind(location), 1500);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.forsale = false;
    //console.log(this.id)
    this.id = this.route.snapshot.params['id'];
    let property = await this.service.Get('/browseproperties?id=' + this.id) as any ;
    console.log(property);

    this.propertyid = property.PROPERTYID;
    this.address = property.PROPERTYADDRESS;
    this.price = property.Price;
    this.suburb = property.SUBURBNAME;
    this.city = property.CITYNAME;
    this.province = property.PROVINCENAME;

    // this.propertyListingPicture = property.Picture?.LISTINGPICTUREIMAGE;
    // this.pictureid = property.Picture[0]?.LISTINGPICTUREID;

    this.documenttype = "ListingPicture";
      for(let x = 0; x < property.Picture.length; x++){
      //console.log(this.properties[x]?.Picture?.LISTINGPICTUREIMAGE);
      this.photos[x] = property.Picture[x]?.LISTINGPICTUREIMAGE
      this.photos[x] = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ property.Picture[x]?.LISTINGPICTUREID) as any);
      console.log(this.photos)
    }


    this.propertytype = property.PROPERTYTYPEDESCRIPTION;
    this.markettype = property.MARKETTYPEDESCRIPTION;
    if(this.markettype == "For Sale"){
      this.forsale = true;
    }
    console.log(this.forsale);

    this.bedrooms = property.Bedrooms?.PROPERTYSPACEQUANTITY;
    this.bathrooms = property.Bathrooms?.PROPERTYSPACEQUANTITY;
    this.parking = property.Parking?.PROPERTYFEATUREQUANTITY;
    this.pointsofinterest = property?.PropertyPOI;
    this.features = property.PropertyFeatures;
    this.otherbuildingdetails = property.Otherbuildingdetail;

    this.agentname = property?.Agent?.USERNAME;
    this.agentsurname = property?.Agent?.USERSURNAME;
    this.agentemail = property?.Agent?.USEREMAIL;

    this.terms = property?.Terms;

    //  this.documenttype = "ListingPicture";
    //  this.propertyListingPicture = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.pictureid) as any;
    //  this.propertyListingPicture = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + this.propertyListingPicture);
    this.dateToday = formatDate(new Date(), 'yyyy-MM-dd', 'en');
  }


  // getPicture(){

  //   return this.sanitizer.bypassSecurityTrustResourceUrl(
  //     'data:image;base64,' + this.propertyListingPicture
  //   );
  // }


  //RENTAL APPLICATION DOCUMENT CONVERSION
 rentalApplicationChangeListener($event){
   this.readThisRentalApplication($event.target);

   let img: any = document.getElementById("rentalapplicationdocument");
   if(typeof (FileReader) !== 'undefined') {
     let reader = new FileReader();

     reader.onload = (e:any) => {
       this.resultsrental = e.target.result;
     }
     reader.readAsArrayBuffer(img.files[0]);
   }
  }
  //Method that reads the file information and parses it
  readThisRentalApplication(inputValue: any): void{
    //Gets the actual file
    var file: File = inputValue.files[0];
    //Creates a reader that will read the file for information
    var myReader: FileReader = new FileReader();

    //Get the extension by splitting the name and popping the name off
    this.fileExtensionrentaldocument = file.name.split('.').pop();

    //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
    myReader.onloadend = (e) => {
      this.fileBase64rentaldocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
      //console.log(this.fileBase64);
    }
    //Read the file in and parse it to Base64
    myReader.readAsDataURL(file);
  }



  //BANK STATEMENT DOCUMENT CONVERSION
  bankStatementChangeListener($event){
    this.readThisBankStatement($event.target);

    let img: any = document.getElementById("bankstatement");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultsbank = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisBankStatement(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensionbankdocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64bankdocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }



  //ID DOCUMENT CONVERSION
  idDocumentChangeListener($event){
    this.readThisID($event.target);

    let img: any = document.getElementById("iddocument");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultsid = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisID(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensioniddocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64iddocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }


   async openRentApplication(id){
     this.propertyid = id;
     //console.log(this.propertyid);
     $("#rentalModal").modal('show');
   }


  async submitApplicationToRent(){
      $("#confirmRentalModal").modal('show');
      $("#rentalModal").modal('hide');
  }

  async applyToRent(){
    //Build up the class that is being POST'ed to the API
    //This has to match with the class you created in the API
    let rentalapplication = {
      "FileBase64" : this.fileBase64rentaldocument,
      "FileExtension" : this.fileExtensionrentaldocument
    };
    //console.log(rentalapplication);

    let bankstatement = {
      "FileBase64" : this.fileBase64bankdocument,
      "FileExtension" : this.fileExtensionbankdocument
    };
    //console.log(bankstatement);

    let iddocument = {
      "FileBase64" : this.fileBase64iddocument,
      "FileExtension" : this.fileExtensioniddocument
    };
    //console.log(iddocument);

    let documents=[];
    documents[0] = rentalapplication;
    documents[1] = bankstatement;
    documents[2] = iddocument;
    //console.log(documents);
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

        //ADDED
    // let term = await this.service.Get('/term?token=' + this.token.token ) as any;
    // console.log(term)
    // this.termid = this.terms.TERMID;

    await this.service.Post(`/rentalapplication?token=${this.token.token}&propertyid=${this.propertyid}&termid=${this.term}&start=${this.startdate}&termid=${this.termid}`, documents);
    this.showRentApplySuccess();

  }

  submitPurchase(){
    $("#purchaseModal").modal('hide');
    $("#confirmPurchaseModal").modal('show');
  }





  async applyPurchase(){
    $("#confirmPurchaseModal").modal('hide');
    let bankstatement = {
      "FileBase64" : this.fileBase64bankdocument,
      "FileExtension" : this.fileExtensionbankdocument
    };
    //console.log(bankstatement);

    let iddocument = {
      "FileBase64" : this.fileBase64iddocument,
      "FileExtension" : this.fileExtensioniddocument
    };
    //console.log(iddocument);

    let documents=[];
    documents[0] = bankstatement;
    documents[1] = iddocument;

    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    // console.log(this.propertyid);
    // console.log(this.token.token);
    // console.log(this.offeramount);
    // console.log(documents);

    await this.service.Post(`/purchaseoffer?token=${this.token.token}&propertyid=${this.propertyid}&offeramount=${this.offeramount}`, documents);
    this.showPurchaseApplySuccess();

  }

  getRentalApplicationForm(){
    //console.log("hit")
    window.open('../../../assets/Rental-Application-Form.pdf', '_blank')
  }
}
