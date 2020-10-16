import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-upload-property-docs',
  templateUrl: './upload-property-docs.component.html',
  styleUrls: ['./upload-property-docs.component.scss']
})

export class UploadPropertyDocsComponent implements OnInit {
  public propertyDocuments;
  public properties: any;
  public token: any;
  public propertyid: any;
  public documenttypeid: any;
  public address: any;
  public documenttypes;
  public documenttypebound: any;

  //Property document
  public fileBase64propertydocument: string;
  public fileExtensionpropertydocument: string;
  resultsproperty: any;
  documenttype: string;
  propertyDocument: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  async ngOnInit() {
    this.spinner.show();
    this.properties = await this.service.Get('/propertydocument');
    this.documenttypes = await this.service.Get('/propertydocumenttypes');
    this.spinner.hide();
    console.log(this.properties);
  }

  //PROPERTY DOCUMENT CONVERSION
  propertyDocumentChangeListener($event){
  this.readThisPropertyDocument($event.target);

  let img: any = document.getElementById("propertydocument");
  if(typeof (FileReader) !== 'undefined') {
    let reader = new FileReader();

    reader.onload = (e:any) => {
      this.resultsproperty = e.target.result;
    }
    reader.readAsArrayBuffer(img.files[0]);
  }
 }
 //Method that reads the file information and parses it
 readThisPropertyDocument(inputValue: any): void{
   //Gets the actual file
   var file: File = inputValue.files[0];
   //Creates a reader that will read the file for information
   var myReader: FileReader = new FileReader();

   //Get the extension by splitting the name and popping the name off
   this.fileExtensionpropertydocument = file.name.split('.').pop();

   //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
   myReader.onloadend = (e) => {
     this.fileBase64propertydocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
     //console.log(this.fileBase64);
   }
   //Read the file in and parse it to Base64
   myReader.readAsDataURL(file);
 }

  showUploadSuccess() {
    this.toastr.success('Property document uploaded successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
  }

  async openUpload(id){
    this.propertyid = id;
    let property = await this.service.Get('/propertydocument?id=' + id) as any ;
    this.address = property.PROPERTYADDRESS;
    //console.log(property.PROPERTYADDRESS);
    $("#docsModal").modal('show');
  }

  async confirmUpload(){
    this.documenttypebound = await this.service.Get('/propertydocumenttypes?id=' + this.documenttypeid) as any ;
    //console.log(this.documenttypebound);
    $("#docsModal").modal('hide');
    $("#confirmUploadModal").modal('show');
  }

  async getPropertyDocument(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.documenttype = "PropertyDocument";
    this.propertyDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    const linkSource = 'data:application/pdf;base64,' + this.propertyDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Property Document';

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async upload(){
    this.spinner.show();
    let propertydocument = {
      "FileBase64" : this.fileBase64propertydocument,
      "FileExtension" : this.fileExtensionpropertydocument
    };
    //console.log(propertydocument);

    $("#confirmUploadModal").modal('hide');

    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    await this.service.Post(`/propertydocument?token=${this.token.token}&propertyid=${this.propertyid}&documenttype=${this.documenttypeid}`, propertydocument);
    this.spinner.hide();
    this.showUploadSuccess();
  }

}
