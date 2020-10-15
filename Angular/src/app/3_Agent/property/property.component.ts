import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { Options } from 'ng5-slider';
import { CountryCodes } from '../../../assets/CountryCodes';
import {formatDate} from '@angular/common';
import { DomSanitizer, SafeResourceUrl, SafeUrl} from '@angular/platform-browser';
import { NgxSpinnerService } from "ngx-spinner";



// import { tempDocument } from '../../../assets/tempDocument'; //remove import and delete file after documents implemented

declare var $: any; //needed to use jQuery in ts


@Component({
  selector: 'app-property',
  templateUrl: './property.component.html',
  styleUrls: ['./property.component.scss']
})
export class PropertyComponent implements OnInit {
public properties;
public marketTypes;
public propertyTypes;
public buildingConditions;
public otherBuildingDetails;
public terms;
public suburbs;
public agents;
public mandateTypes;
public mandateDates = [];
public featureDescriptions;
public usageCategories;
public token: any; //holds user token

  //Define all the variables to be used
  public propertyID: any;
  public nameInput: any;
  // public provinceid: any;
  // public provinceNameInput: any;
  public agentInput: any;
  public agentid: any;
  public agent: any;
  public mandateTypeID: any;
  public mandateTypeDocInput: any;
  public mandateDateInput: any;
  public priceInput: any;
  public price: any;
  public marketTypeID: any;
  public propertyTypeID: any;
  public propertyTypeDescription: any;
  public buildingConditionID: any;
  public suburbID: any;
  public buildingConditionInput: any;
  public propertyAddressInput: any;
  public propertyAddress: any;
  public suburbName: any;
  public cityName: any;
  public provinceName: any;
  public propertyOwnerNameInput: any;
  public propertyOwnerName: any;
  public propertyOwnerSurnameInput: any;
  public propertyOwnerSurname: any;
  public propertyOwnerEmailInput: any;
  public propertyOwnerEmail: any;
  public propertyOwnerIdNumberInput: any;
  public propertyOwnerPassportNumberInput: any;
  public propertyOwnerAddressInput: any;
  public propertyOwnerAddress: any;
  public propertyOwnerContactNumberCodeInput: any;
  public propertyOwnerContactNumberInput: any;
  public propertyOwnerAltContactNumberInput: any;
  public propertyOwnerContactNumber: any;
  public propertyOwnerAltContactNumber: any;
  public propertyTerms: any;
  public bedroomCount: any;
  public bathroomCount: any;
  public municipalValuationAmount: any;
  //public ratesAndTaxes: any;
  public ratesAndTaxesInput: any;
  public levies: any;
  public leviesInput: any;
  public status: any;
  public features: any;
  public featureID: any;
  public pointsOfInterest: any;
  public mandates = [];
  public marketTypeDescription: any;
  public spaces: any;
  public bedrooms: any;
  public bathrooms: any;
  public marketType: any;
  public propertyFeatures: any[][] = [];
  public propertyFeatureQuantities = [];
  public propertyOtherBuildingDetails = [];
  public propertyZoningMunicipalValuation: any;
  public propertyZoningRatingPeriod: any;
  public propertyZoningUsageCategory: any;
  public propertyZoningYearOfValuation: any;
  public propertyZonintEstimatedMonthlyRates: any;
  public propertyZoningUsage: any;
  public propertydetails: any[] = [];
  public propertySpaces = [];
  public monthlyRates: any;
  public countryCode: any;
  public altCountryCode: any;
  public propertyListingPicture: any;
  public propertyFeaturesView = [];
  public propertyOtherBuildingDetailsView = [];
  public propertyAvailableDate: any;
  public dateToday: any;
  public valuationDate: any;
  public SouthAfrican: any;
  public propertyFeatureDescriptionInput: any;
  public propertyFeatureQuantityInput: any;
  public propertyOtherBuildingDetailInput: any;
  public listingPictures: any;

  //Mandate document
  public fileBase64mandatedocument: string;
  public fileExtensionmandatedocument: string;
  resultsmandate: any;
  public newPOContactNumber: any;
  public newPOAltContactNumber: any;

  //Listing picture
  public fileBase64picturedocument: any = [];
  public fileExtensionpicturedocument: any = [];
  resultspicture: any;
  public photos: any[] = [];

  public countryCodes = CountryCodes;

  counter1: number;
  counter2: number;

  //Range picker options
  value: number = 6;
  highValue: number = 6;
  options: Options = {
    floor: 6,
    ceil: 60,
    step: 6,
    showTicks: true,
    showTicksValues: true,
  };

  ////////TEST
  public counter: 10;
  public defectDescriptions = [];
  public defects;
  mandatedocument: any;
  documenttype: string;
  mandateid: any;
  pictureid: any;
  startDate: string;
  endDate: string;
  public addedPicture: any;
  photo: any;


  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private sanitizer: DomSanitizer, private spinner: NgxSpinnerService) { }

  showAddSuccess() {
    this.toastr.success('Property added successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Property updated successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Property deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('Property cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  showNoDuplicateImage() {
    this.toastr.error('A duplicate image cannot be added', "", {
      timeOut: 1000,
    });
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.properties = await this.service.Get('/property?token=' + this.token.token);
    this.features = await this.service.Get('/feature?token=' + this.token.token);
    this.otherBuildingDetails = await this.service.Get('/otherbuildingdetail?token=' + this.token.token);
    this.spinner.hide();
    this.agents = await this.service.Get('/agents?token=' + this.token.token);
    this.suburbs = await this.service.Get('/suburb?token=' + this.token.token);
    this.marketTypes = await this.service.Get('/markettype?token=' + this.token.token);
    this.propertyTypes = await this.service.Get('/propertytype?token=' + this.token.token);
    
    this.terms = await this.service.Get('/term?token=' + this.token.token);
    this.options.floor = this.terms[0].TERMDESCRIPTION;
    this.value = this.terms[0].TERMDESCRIPTION;
    this.options.ceil = this.terms[this.terms.length-1].TERMDESCRIPTION;
    this.highValue = this.terms[this.terms.length-1].TERMDESCRIPTION;
    this.buildingConditions = await this.service.Get('/buildingcondition?token=' + this.token.token);
    this.mandateTypes = await this.service.Get('/mandatetype?token=' + this.token.token);
    this.dateToday = formatDate(new Date(), 'yyyy-MM-dd', 'en');
  }

  async add(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    let mandatedocument = {
      "FileBase64" : this.fileBase64mandatedocument,
      "FileExtension" : this.fileExtensionmandatedocument
    };
    //console.log(mandatedocument);

    let listingpictureList =  [];
    let i = 0;
    this.fileBase64picturedocument.forEach(e => {
      listingpictureList.push({"FileBase64":this.fileBase64picturedocument[i], "FileExtension" : this.fileExtensionpicturedocument[i]});
      i++;
      console.log(e)
    });
    console.log(listingpictureList);

    this.propertydetails = [];
    this.propertySpaces = [];
    //this.mandateTypeDocInput = tempDocument; // Delete this line once documents implemented
    this.propertySpaces.push(this.bedroomCount, this.bathroomCount);
    this.propertyFeatures.forEach(e => {
      e[0] = e[0].FEATUREID;
    });
    this.propertyOtherBuildingDetails.forEach(e => {
      e = e.OTHERBUILDINGDETAILID;
    });
    this.propertydetails.push(this.propertyOtherBuildingDetails, this.propertyFeatures, this.propertySpaces);
    console.log(this.propertydetails);
    this.phoneNumberJoiner();
    this.getMarketTypeID(); ////////////////////New for markettype
    // if (this.marketTypeID = 1){
    //   this.value = null;
    //   this.highValue = null;
    // }
    console.log("hit1")
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    await this.service.Post(`/property?token=${this.token.token}&address=${this.propertyAddressInput}&price=${this.priceInput}&ownername=${this.propertyOwnerNameInput}&ownersurname=${this.propertyOwnerSurnameInput}&owneremail=${this.propertyOwnerEmailInput}&owneraddress=${this.propertyOwnerAddressInput}&owneridnumber=${this.propertyOwnerIdNumberInput}&ownerpassportnumber=${this.propertyOwnerPassportNumberInput}&ownercontactnumber=${this.newPOContactNumber}&owneraltcontactnumber=${this.newPOAltContactNumber}&markettypeid=${this.marketTypeID}&propertytypeid=${this.propertyTypeID}&availabledate=${this.propertyAvailableDate}&suburbid=${this.suburbID}&mandatetypeid=${this.mandateTypeID}&mandatedate=${this.mandateDateInput}&agentid=${this.agentid}&minterm=${this.value}&maxterm=${this.highValue}&ratesandtax=${this.ratesAndTaxesInput}&condition=${this.buildingConditionID}&municipalvaluation=${this.propertyZoningMunicipalValuation}&monthlyrates=${this.propertyZonintEstimatedMonthlyRates}&period=${this.propertyZoningRatingPeriod}&usagecategory=${this.propertyZoningUsageCategory}&yearofvaluation=${this.propertyZoningYearOfValuation}&zoningusage=${this.propertyZoningUsage}&levies=${this.leviesInput}`, this.propertydetails);

    console.log("hit2")

    await this.service.Post(`/propertyfile?token=${this.token.token}&mandatetypeid=${this.mandateTypeID}&mandatedate=${this.mandateDateInput}`, mandatedocument);

    console.log("hit3")
    await this.service.Patch(`/propertyfile?token=${this.token.token}`, listingpictureList);


    console.log("hit14")
    this.spinner.hide();
    this.showAddSuccess();
  }

  //MANDATE DOCUMENT CONVERSION
mandateDocumentChangeListener($event){
  this.readThisMandateDocument($event.target);

  let img: any = document.getElementById("Mandatedocument");
  if(typeof (FileReader) !== 'undefined') {
    let reader = new FileReader();

    reader.onload = (e:any) => {
      this.resultsmandate = e.target.result;
    }
    reader.readAsArrayBuffer(img.files[0]);
  }
 }
 //Method that reads the file information and parses it
 readThisMandateDocument(inputValue: any): void{
   //Gets the actual file
   var file: File = inputValue.files[0];
   //Creates a reader that will read the file for information
   var myReader: FileReader = new FileReader();

   //Get the extension by splitting the name and popping the name off
   this.fileExtensionmandatedocument = file.name.split('.').pop();

   //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
   myReader.onloadend = (e) => {
     this.fileBase64mandatedocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
     //console.log(this.fileBase64);
   }
   //Read the file in and parse it to Base64
   myReader.readAsDataURL(file);
 }


   //LISTING PICTURE CONVERSION
pictureChangeListener($event){
  this.readThisPicture($event.target);

  let img: any = document.getElementById("PropertyImage");
  if(typeof (FileReader) !== 'undefined') {
    let reader = new FileReader();

    reader.onload = (e:any) => {
      this.resultsmandate = e.target.result;
    }
    reader.readAsArrayBuffer(img.files[0]);
  }
 }
 //Method that reads the file information and parses it
 readThisPicture(inputValue: any): void{
   //Gets the actual file
   var file: File = inputValue.files[0];
   //Creates a reader that will read the file for information
   var myReader: FileReader = new FileReader();

   //Get the extension by splitting the name and popping the name off
   this.fileExtensionpicturedocument.push(file.name.split('.').pop());

   //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
   myReader.onloadend = (e) => {
     let readerValue = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);

     let duplicate = false;
      this.fileBase64picturedocument.forEach(e => {
        if (e == readerValue){
          duplicate = true;
        }        
      });
      if (duplicate == false){
        this.fileBase64picturedocument.push(readerValue);
        console.log(this.fileExtensionpicturedocument);
      }
      else{
        this.showNoDuplicateImage();
      }
     
   }
   //Read the file in and parse it to Base64
   myReader.readAsDataURL(file);
 }

  //Used to display picture in modal dynamically when adding
  public imagePath;
  imgURL: any = [];
  imgURLDisplay: any = [];
  imageNames: any = [];
  public message: string;
  preview(files) {
    console.log(files[0])
    if (files.length === 0)
      return;
    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.message = "Only images are supported.";
      return;
    }
    var reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]);
    reader.onload = (_event) => {
      let readerResult = reader.result;
      let duplicate = false;
      this.imgURL.forEach(e => {
        if (e?.changingThisBreaksApplicationSecurity != undefined){
          e = e.changingThisBreaksApplicationSecurity.toString().split(',').pop();
          // console.log(e)
          // console.log(readerResult.toString().split(',').pop())
        }
        if(e == readerResult.toString().split(',').pop()){
          duplicate = true
        }
      });
      if(duplicate == false){
        this.imgURL.push(reader.result);
        this.imageNames.push(files[0].name)
      }
      else{
      }
      
      console.log(this.imgURL)
    }
  }

  deleteImage(i){
    this.imgURL.splice(i, 1);
    this.fileBase64picturedocument.splice(i, 1);
    this.fileExtensionpicturedocument.splice(i, 1);
    console.log(this.imgURL, this.fileBase64picturedocument)
  }


  async view(id){    
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.token);
    let property = await this.service.Get('/property?token=' + this.token.token + '&id='+ id) as any;
    console.log(property);
    //this.propertyListingPicture = property.Picture?.LISTINGPICTUREIMAGE;
    //this.pictureid = property.Picture[0]?.LISTINGPICTUREID;
    //console.log(this.propertyListingPicture);
    //console.log(this.pictureid)
    this.propertyID = property.PROPERTYID;
    this.priceInput = property.Price?.PRICEAMOUNT;
    this.agentid = property.Agent[0].USERID;
    //console.log(this.propertyID);
    this.propertyAddressInput = property.PROPERTYADDRESS;
    this.status = property.PROPERTYSTATUSDESCRIPTION;
    this.propertyOwnerNameInput = property.PROPERTYOWNERNAME;
    this.propertyOwnerSurnameInput = property.PROPERTYOWNERSURNAME;
    this.propertyOwnerEmailInput = property.PROPERTYOWNEREMAIL;
    this.propertyOwnerAddressInput = property.PROPERTYADDRESS;
    this.propertyOwnerIdNumberInput = property.PROPERTYOWNERIDNUMBER;
    this.propertyOwnerPassportNumberInput = property.PROPERTYOWNERPASSPORTNUMBER;
    if (this.propertyOwnerIdNumberInput != null){
      this.SouthAfrican = 'Yes';
    }
    else if (this.propertyOwnerPassportNumberInput != null){
      this.SouthAfrican = 'No';
    }

    this.propertyOwnerContactNumberInput = property.PROPERTYOWNERCONTACTNUMBER;
    this.propertyOwnerAltContactNumberInput = property.PROPERTYOWNERALTCONTACTNUMBER;
    this.pointsOfInterest = property.Pointsofinterest;

    this.mandatedocument = property.Mandates.MANDATEDOCUMENT;
    this.mandateid = property.Mandates.MANDATEID;
    console.log(this.propertyOwnerContactNumberInput)


    let tempMandates = [];
    tempMandates.push(property.Mandates);
    if (property.Mandates != null){
      tempMandates.forEach(e => {
        e.MANDATEDATE = e.MANDATEDATE.split("T")[0];
        this.mandateDates.push(e.MANDATEDATE);
        this.mandates.push(e);
      });
    }
    this.agentInput = property.Agent;
    this.marketTypeDescription = property.MARKETTYPEDESCRIPTION;
    this.marketTypeID = property.MARKETTYPEID;
    this.propertyTypeDescription = property.PROPERTYTYPEDESCRIPTION;
    this.propertyTypeID = property.PROPERTYTYPEID;
    this.propertyAvailableDate = property.PROPERTYAVAILABLEDATE.split("T")[0];
    this.suburbName = property.SUBURBNAME;
    this.suburbID = property.SUBURBID;
    this.cityName = property.CITYNAME;
    this.provinceName = property.PROVINCENAME;
    this.spaces = property.Spaces;
    if (property.Bedrooms != null){
      this.bedroomCount = property.Bedrooms?.PROPERTYSPACEQUANTITY;
    }
    if (property.Bathrooms != null){
      this.bathroomCount = property.Bathrooms?.PROPERTYSPACEQUANTITY;
    }
    this.buildingConditionInput = property.BUILDINGCONDITION?.BUILDINGCONDITIONDESCRIPTION;
    this.buildingConditionID = property.BUILDINGCONDITIONID;
    this.propertyTerms = property.Terms;
    this.value = property?.Mintermdescription;
    this.highValue = property?.Maxtermdescription;
    this.propertyFeatures = [];
    this.propertyFeatureQuantities = [];
    property.PropertyFeatures.forEach(e => {
      this.propertyFeatures.push([e, e.PROPERTYFEATUREQUANTITY]);
      this.propertyFeatureQuantities.push(e.PROPERTYFEATUREQUANTITY);
      this.features.forEach(f => {
        if (f.FEATUREID == e.FEATUREID && f.FEATUREDESCRIPTION == e.FEATUREDESCRIPTION){
          this.propertyFeatureDescriptionInput = f;
        }
      });
      this.propertyFeatureQuantityInput = e.PROPERTYFEATUREQUANTITY;
    });
    console.log(property.PropertyFeatures)
    this.propertyOtherBuildingDetails = [];
    property.Otherbuildingdetails.forEach(e => {
      this.propertyOtherBuildingDetails.push(e);
      this.otherBuildingDetails.forEach(f => {
        if (f.OTHERBUILDINGDETAILID == e.OTHERBUILDINGDETAILID && f.OTHERBUILDINGDETAILDESCRIPTION == e.OTHERBUILDINGDETAILDESCRIPTION){
          this.propertyOtherBuildingDetailInput = f;
        }
      });
    });
    this.counter1 = this.propertyFeatures.length;
    this.counter2 = this.propertyOtherBuildingDetails.length;
    this.propertyZoningMunicipalValuation = property.Zoning?.ZONINGMUNICIPALVALUATION;
    this.propertyZoningRatingPeriod = property.Zoning?.ZONINGRATINGPERIOD;
    this.propertyZoningUsageCategory = property.Zoning?.ZONINGUSAGECATEGORY;
    this.propertyZoningYearOfValuation = property.Zoning?.ZONINGYEAROFVALUATION.split("-")[0];;
    this.propertyZonintEstimatedMonthlyRates = property.Zoning?.ZONINGESTIMATEDMONTHLYRATES;
    this.propertyZoningUsage = property.Zoning?.ZONINGUSAGE;
    this.ratesAndTaxesInput = property.PROPERTYRATEANDTAX;
    this.leviesInput = property.PROPERTYLEVIES;

    this.documenttype = "Mandate";
    this.mandatedocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.mandateid) as any;
    console.log(this.ratesAndTaxesInput)

    //Adding sanitizer for multiple pictures:
    this.documenttype = "ListingPicture";
    for(let x = 0; x < property.Picture.length; x++){
      //this.photos[x] = this.properties[x]?.Picture?.LISTINGPICTUREIMAGE
      let base64Header: string = 'data:image;base64,';
      let base64: string = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ property.Picture[x]?.LISTINGPICTUREID) as any;
      let base64Combined = base64Header + base64;
      this.photos[x] = this.sanitizer.bypassSecurityTrustResourceUrl
      (base64Combined);

      
      this.fileBase64picturedocument.push(base64);
      this.fileExtensionpicturedocument.push(property.Picture[x].LISTINGPICTUREIMAGE.split('.').pop());
      this.imgURL[x] = this.sanitizer.bypassSecurityTrustResourceUrl
      (base64Combined);
    }
    console.log(this.imgURL)
    
    this.spinner.hide();
  }

  getMandateDocument(){
    const linkSource = 'data:application/pdf;base64,' + this.mandatedocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Mandate Document ' + this.propertyAddressInput;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }



  async update(id){
    
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.propertydetails = [];
    //this.mandateTypeDocInput = tempDocument; // Delete this line once documents implemented

    let mandatedocument = {
    "FileBase64" : this.fileBase64mandatedocument,
    "FileExtension" : this.fileExtensionmandatedocument
    };
    console.log(mandatedocument);

    let listingpictureList =  [];
    let i = 0;
    this.fileBase64picturedocument.forEach(e => {
      listingpictureList.push({"FileBase64":this.fileBase64picturedocument[i], "FileExtension" : this.fileExtensionpicturedocument[i]});
      i++;
    });
    console.log(listingpictureList, listingpictureList[0]);

    this.propertySpaces.push(this.bedroomCount, this.bathroomCount);
    this.propertyFeatures.forEach(e => {
      e[0] = e[0].FEATUREID;
    });
    console.log(this.bedroomCount, this.bathroomCount)
    this.propertyOtherBuildingDetails.forEach(e => {
      e = e.OTHERBUILDINGDETAILID;
    });
    this.propertydetails.push(this.propertyOtherBuildingDetails, this.propertyFeatures, this.propertySpaces);
    this.getMarketTypeID(); ////////////////////New for markettype
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(id);

    this.phoneNumberUpdateJoiner();

    if (mandatedocument.FileBase64 != undefined){
      await this.service.Post(`/propertyfileupdate?token=${this.token.token}&propertyid=${id}&mandatetypeid=${this.mandateTypeID}&mandatedate=${this.mandateDateInput}`, mandatedocument);
    }
    console.log(mandatedocument, listingpictureList)
    if (listingpictureList[0] != undefined){
      await this.service.Patch(`/propertyfileupdate?token=${this.token.token}&propertyid=${id}`, listingpictureList);
    }
    
    await this.service.Patch(`/property?token=${this.token.token}&id=${id}&address=${this.propertyAddressInput}&price=${this.priceInput}&ownername=${this.propertyOwnerNameInput}&ownersurname=${this.propertyOwnerSurnameInput}&owneremail=${this.propertyOwnerEmailInput}&owneraddress=${this.propertyOwnerAddressInput}&owneridnumber=${this.propertyOwnerIdNumberInput}&ownerpassportnumber=${this.propertyOwnerPassportNumberInput}&ownercontactnumber=${this.newPOContactNumber}&owneraltcontactnumber=${this.newPOContactNumber}&markettypeid=${this.marketTypeID}&propertytypeid=${this.propertyTypeID}&availabledate=${this.propertyAvailableDate}&suburbid=${this.suburbID}&mandatetypeid=${this.mandateTypeID}&mandatedate=${this.mandateDateInput}&agentid=${this.agentid}&minterm=${this.value}&maxterm=${this.highValue}&ratesandtax=${this.ratesAndTaxesInput}&condition=${this.buildingConditionID}&municipalvaluation=${this.propertyZoningMunicipalValuation}&monthlyrates=${this.monthlyRates}&period=${this.propertyZoningRatingPeriod}&usagecategory=${this.propertyZoningUsageCategory}&yearofvaluation=${this.propertyZoningYearOfValuation}&zoningusage=${this.propertyZoningUsage}&levies=${this.leviesInput}`, this.propertydetails);
    this.spinner.hide();
    this.showUpdateSuccess();
  }

  async deleteBinding(id){
    console.log(id);
    this.propertyID = id;
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let property = await this.service.Get('/property?token=' + this.token.token + '&id='+ id) as any;
    this.propertyAddress = property.PROPERTYADDRESS;
  }

  async delete(id){
    
    this.spinner.show();
    console.log(id);
    $("#deleteModal").modal('hide');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.token)
    let response = await this.service.Delete('/property?token=' + this.token.token + '&id='+ id);
    this.spinner.hide();
    console.log(response)
    if (response === 409){
      this.showDeleteFailure();
    }
    else{
      this.showDeleteSuccess();
    }
  }

  phoneNumberJoiner()
  {
    if (this.propertyOwnerContactNumberInput != null){
      this.newPOContactNumber = "%2B"+this.countryCode.substring(1)+this.propertyOwnerContactNumberInput.substring(1);
    }
    if (this.propertyOwnerAltContactNumberInput != null){
      this.newPOAltContactNumber = "%2B"+this.altCountryCode.substring(1)+this.propertyOwnerAltContactNumberInput.substring(1);
    }
  }

  phoneNumberUpdateJoiner()
  {
    if (this.propertyOwnerContactNumberInput != null){
      this.newPOContactNumber = "%2B"+this.propertyOwnerContactNumberInput.substring(1);
    }
    if (this.propertyOwnerAltContactNumberInput != null){
      this.newPOAltContactNumber = "%2B"+this.propertyOwnerAltContactNumberInput.substring(1);
    }
  }

  //Add form validation
  async submitAdd(){
      //console.log(this.descriptionInput);
      $("#confirmAddModal").modal('show');
      $("#addModal").modal('hide');
  }

  //Added
  async generateReport(){
    if (this.startDate != "" && this.endDate != ""  && this.startDate <= this.endDate){
    $("#propertyreportModal").modal('hide');
    this.service.GenerateReport(this.startDate, this.endDate)
    this.router.navigateByUrl("property-report")
    }
    else{
      this.toastr.warning("The End Date cannot be earlier than the Start Date")
    }
  }


  //Update form validation
  async submitUpdate(){
    console.log("update");
      $("#confirmEditModal").modal('show');
      $("#updateModal").modal('hide');
  }

  fn(i){
    console.log(i)
  }

  getMarketTypeID(){ ////////////////////New for markettype
    this.marketTypes.forEach(e => {
      if (this.marketTypeDescription == e.MARKETTYPEDESCRIPTION){
        this.marketTypeID = e.MARKETTYPEID;
      }
    });
  }

  addFeature(){
    if (this.propertyFeatureDescriptionInput != null && this.propertyFeatureQuantityInput != null){
      let check = false;
    this.propertyFeatures.forEach(e => {
      console.log(e)
      if (e[0] == this.propertyFeatureDescriptionInput){
        check = true;
      }
    });
    if (check == false){
      this.propertyFeatures.push([this.propertyFeatureDescriptionInput, this.propertyFeatureQuantityInput])
    }
    console.log(this.propertyFeatures);
    }
  }

  deleteFeature(i){
    this.propertyFeatures.splice(i, 1)
    console.log(this.propertyFeatures);
  }

  addOtherBuildingDetail(){
    if (this.propertyOtherBuildingDetailInput != null){
      let check = false;
    this.propertyOtherBuildingDetails.forEach(e => {
      if (e ==  this.propertyOtherBuildingDetailInput){
        check = true;
      }
    });
    if (check == false){
      this.propertyOtherBuildingDetails.push(this.propertyOtherBuildingDetailInput)
    console.log(this.propertyOtherBuildingDetails);
    }
    }
  }

  deleteOtherBuildingDetail(i){
    this.propertyOtherBuildingDetails.splice(i, 1)
    console.log(this.propertyOtherBuildingDetails);
  }

  
}
