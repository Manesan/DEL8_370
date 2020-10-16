import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-valuations',
  templateUrl: './valuations.component.html',
  styleUrls: ['./valuations.component.scss']
})

export class ValuationsComponent implements OnInit {
  public valuations;
  public valuers;
  public properties;
  public valuationStatuses;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

      //Define all the variables to be used
      public valuationid: any;
      public valuationDocument: any;
      public valuationDescription: any;
      public valuationStatusDescription: any;
      public valuerid: any;
      public statusid: any;
      public valuer: any;
      public valuationDate: any;
      public propertyAddress: any;

    public startDate: any;
    public endDate: any;

  //Valuation document
  public fileBase64valuationdocument: string;
  public fileExtensionvaluationdocument: string;
  resultsvaluation: any;

  public documenttype: any;


  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showCaptureSuccess() {
    this.toastr.success('Valuation captured successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showAssignSuccess() {
    this.toastr.success('Valuer assigned to valuation successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.valuations = await this.service.Get('/valuation?token=' + this.token.token);
    this.spinner.hide();
    this.valuers = await this.service.Put('/employee?token=' + this.token.token);
    this.valuationStatuses = await this.service.Put('/ivstatus?token=' + this.token.token);
    this.valuations.forEach(i => {
      i.VALUATIONDATE = i.VALUATIONDATE?.split("T")[0];
    });
    this.properties = await this.service.Get('/property?token=' + this.token.token);
    console.log(this.valuers);
    this.showViewModal = false;
  }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let valuation = await this.service.Get('/valuation?token=' + this.token.token + '&id='+ id) as any;
    this.propertyAddress = valuation.PROPERTYADDRESS;
    this.valuationDocument = valuation.VALUATIONDOCUMENT;

    this.documenttype = "Valuation";
    this.valuationDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    this.valuationDescription = valuation.VALUATIONDESCRIPTION;
    this.valuationDate = valuation.VALUATIONDATE?.split("T")[0];
    this.valuer = valuation.USERNAME + " " + valuation.USERSURNAME;
    this.valuationStatusDescription = valuation.IVSTATUSDESCRIPTION;
    this.valuationid = valuation.VALUATIONID;
    this.valuerid = valuation.USERID;
    this.statusid = valuation.IVSTATUSID;
    this.showViewModal = true;
    console.log(this.valuerid)
    this.spinner.hide();
  }

  getValuationDocument(){
    const linkSource = 'data:application/pdf;base64,' + this.valuationDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Valuation Document ' + this.propertyAddress;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  //VALUATION DOCUMENT CONVERSION
  valuationDocumentChangeListener($event){
    this.readThisValuationDocument($event.target);

    let img: any = document.getElementById("uploadButton");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultsvaluation = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisValuationDocument(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensionvaluationdocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64valuationdocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }


  async capture(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(this.valuerid);
    let valuationdocument = {
      "FileBase64" : this.fileBase64valuationdocument,
      "FileExtension" : this.fileExtensionvaluationdocument
    };
    //console.log(valuationdocument);
    await this.service.Patch(`/valuation?token=${this.token.token}&id=${id}&date=${this.valuationDate}&description=${this.valuationDescription}&userid=${this.valuerid}&IVid=${this.statusid}`, valuationdocument);
    this.spinner.hide();
    this.showCaptureSuccess();
  }

  async assign(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#confirmCaptureAssignmentModal").modal('hide');
    //console.log(id);
    await this.service.Patch(`/assignvaluer?token=${this.token.token}&valuationid=${id}&userid=${this.valuerid}`);
    //this.showViewModal = false; //Added
    this.spinner.hide();
    this.showAssignSuccess();
  }

  async submitCapture(){
      $("#captureModal").modal('hide');
      $("#confirmCaptureModal").modal('show');
  }


  //Assign form validation
  async submitAssign(){
    if(this.valuerid != "" ){
      //console.log(this.descriptionInput);
      $("#assignModal").modal('hide');
      $("#confirmCaptureAssignmentModal").modal('show');
    }
  }

    //Generate report form validation
    async submitGenerateValuation(){
    console.log(this.startDate, this.startDate)
    if (this.startDate != null && this.endDate != null  && this.startDate <= this.endDate){
      $("#valuationreportModal").modal('hide');
      this.service.GenerateReport(this.startDate, this.endDate);
      this.router.navigateByUrl("valuation-report");
    }
    else{
      this.toastr.warning("The End Date cannot be earlier than the Start Date")
    }

  }
}
