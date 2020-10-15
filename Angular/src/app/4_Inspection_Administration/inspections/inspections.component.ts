import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { DomSanitizer, SafeResourceUrl, SafeUrl} from '@angular/platform-browser';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

declare function editInspection(statusid): any;

@Component({
  selector: 'app-inspections',
  templateUrl: './inspections.component.html',
  styleUrls: ['./inspections.component.scss']
})
export class InspectionsComponent implements OnInit {
  public inspections;
  public inspectors;
  public defects;
  public spaces;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token
  public inspectionTypes: any;
  public inspectionStatuses: any;

  //Define all the variables to be used
  public inspectionid: any;
  public inspectorid: any;
  public statusid: any;
  public typeid: any;
  public inspectionDate: any;
  public propertyAddress: any;
  public inspectionDocument: any;
  public inspector: any;
  public inspectionTypeDescription: any;
  public inspectionStatusDescription: any;
  public totalDefectQuantity: any;
  public inspectionDefects: any;
  public comments: any;
  public counter: any;
  public startDate: any;
  public endDate: any;
  public defectDescriptions = [];
  public defectSpaces = [];
  public defectQuantities = [];
  //Added
  public reportDate: any;
  public reportUser: any;
  public reportInspectionDate: any;
  public reportPropertyAddress: any;
  public reportInspectionTypeDescription: any;
  public reportInspectorName: any;
  public reportInspectorSurname: any;
  public reportDefects: [];
  public reportInspectionDocument: any;
  public cleanDoc: any;
  public inspectiondoc: any;
  public documenttype: any;
  public defectDescriptionInput: any;
  public propertyDefects: any[][] = [];
  public propertyDefectQuantityInput: any;  
  public spaceDescriptionInput: any;


   //Inspection document
   public fileBase64inspectiondocument: string;
   public fileExtensioninspectiondocument: string;
   resultsinspection: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private sanitizer: DomSanitizer, private spinner: NgxSpinnerService) { }

  showSaveSuccess() {
    this.toastr.success('Inspection saved successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showAssignSuccess() {
    this.toastr.success('Inspector assigned to inspection successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.inspections = await this.service.Get('/inspection?token=' + this.token.token);
    this.inspectors = await this.service.Get('/inspectors?token=' + this.token.token);
    this.inspectionTypes = await this.service.Put('/inspectiontype?token=' + this.token.token);
    this.inspectionStatuses = await this.service.Put('/ivstatus?token=' + this.token.token);
    this.defects = await this.service.Get('/defect?token=' + this.token.token);
    this.spaces = await this.service.Get('/space?token=' + this.token.token);
    // this.inspections.forEach(i => {
    //   i.INSPECTIONDATE = i.INSPECTIONDATE.split("T")[0];
    // });
    this.defectDescriptions = [];
    this.defectQuantities = [];
    this.defectSpaces = [];
    this.inspections.forEach(e => {      
      e.INSPECTIONDATE = e.INSPECTIONDATE.split("T")[0];
    });
    console.log(this.inspections)
    this.spinner.hide();
  }

  async view(id){
    this.spinner.show();
    this.defectDescriptions = [];
    this.defectQuantities = [];
    this.defectSpaces = [];
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let inspection = await this.service.Get('/inspection?token=' + this.token.token + '&id='+ id) as any;
    this.propertyAddress = inspection.PROPERTYADDRESS;
    this.inspectionDocument = inspection.INSPECTIONDOCUMENT;

    this.documenttype = "Inspection";
    this.inspectionDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    this.inspectionDate = inspection.INSPECTIONDATE.split("T")[0];
    this.inspector = inspection.USERNAME + " " + inspection.USERSURNAME;
    this.inspectionTypeDescription = inspection.INSPECTIONTYPEDESCRIPTION;
    this.inspectionStatusDescription = inspection.IVSTATUSDESCRIPTION;
    this.counter = inspection.PropertyDefects.length;
    this.inspectionDefects = inspection.PropertyDefects;
    this.comments = inspection.INSPECTIONCOMMENT;
    this.inspectionid = inspection.INSPECTIONID;
    this.inspectorid = inspection.USERID;
    this.typeid = inspection.INSPECTIONTYPEID;
    this.statusid = inspection.IVSTATUSID;
    /*inspection.PropertyDefects.forEach(e => {
    this.defectDescriptions.push(e.DEFECTDESCRIPTION);
    this.defectSpaces.push(e.SPACEDESCRIPTION);
    this.defectQuantities.push(e.PROPERTYDEFECTQUANTITY);
    });*/
    this.propertyDefects = [];
    this.defectQuantities = [];
    inspection.PropertyDefects.forEach(e => {
      this.propertyDefects.push([e]);
      //this.defectQuantities.push(e.PROPERTYDEFECTQUANTITY);
      this.defects.forEach(f => {
        if (f.DEFECTID == e.DEFECTID && f.DEFECTDESCRIPTION == e.DEFECTDESCRIPTION){
          this.defectDescriptionInput = f;
        }
      });
      this.spaces.forEach(f => {
        if (f.SPACEID == e.SPACEID && f.SPACEDESCRIPTION == e.SPACEDESCRIPTION){
          this.spaceDescriptionInput = f;
        }
      });
      this.propertyDefectQuantityInput = e.PROPERTYDEFECTQUANTITY;
    });
    //console.log(inspection, inspection.PropertyDefects, this.propertyDefects)
    this.spinner.hide();
  }

  getInspectionDocument(){
    const linkSource = 'data:application/pdf;base64,' + this.inspectionDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Inspection Document ' + this.propertyAddress;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

//INSPECTION DOCUMENT CONVERSION
inspectionDocumentChangeListener($event){
  this.readThisInspectionDocument($event.target);

  let img: any = document.getElementById("uploadButton");
  if(typeof (FileReader) !== 'undefined') {
    let reader = new FileReader();

    reader.onload = (e:any) => {
      this.resultsinspection = e.target.result;
    }
    reader.readAsArrayBuffer(img.files[0]);
  }
 }
 //Method that reads the file information and parses it
 readThisInspectionDocument(inputValue: any): void{
   //Gets the actual file
   var file: File = inputValue.files[0];
   //Creates a reader that will read the file for information
   var myReader: FileReader = new FileReader();

   //Get the extension by splitting the name and popping the name off
   this.fileExtensioninspectiondocument = file.name.split('.').pop();

   //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
   myReader.onloadend = (e) => {
     this.fileBase64inspectiondocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
     //console.log(this.fileBase64);
   }
   //Read the file in and parse it to Base64
   myReader.readAsDataURL(file);
 }


  async save (id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    let inspectiondocument = {
      "FileBase64" : this.fileBase64inspectiondocument,
      "FileExtension" : this.fileExtensioninspectiondocument
    };
    //console.log(inspectiondocument);

    let defectsToSave = [];
    //console.log(this.defectDescriptions);
    defectsToSave.push(this.propertyDefects, inspectiondocument);
    console.log(defectsToSave);
    await this.service.Patch(`/inspection?token=${this.token.token}&id=${id}&date=${this.inspectionDate}&comment=${this.comments}&typeid=${this.typeid}&userid=${this.inspectorid}&IVid=${this.statusid}`, defectsToSave);
    //console.log("hit",this.inspectionDocument);
    this.defectDescriptions = [];
    this.defectQuantities = [];
    this.defectSpaces = [];
    this.spinner.hide();
    this.showSaveSuccess();
  }

  async assign(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    console.log(id);
    await this.service.Post(`/assigninspector?token=${this.token.token}&inspectionid=${id}&userid=${this.inspectorid}`);
    this.showViewModal = false; //Added
    this.spinner.hide();
    this.showAssignSuccess();
  }

  editInspection(){
    editInspection(this.statusid)
  }

  //Added
  async generateReport(){
    if (this.startDate != null && this.endDate != null  && this.startDate <= this.endDate){
    $("#inspectionreportModal").modal('hide');
    this.service.GenerateReport(this.startDate, this.endDate);
    this.router.navigateByUrl("inspection-report")
    }
    else{
      this.toastr.warning("The End Date cannot be earlier than the Start Date")
    }
  }

  addDefect(){
    if (this.defectDescriptionInput != null && this.spaceDescriptionInput != null && this.propertyDefectQuantityInput != null){
      let check = false;
      console.log('hit', this.propertyDefects)
    this.propertyDefects.forEach(e => {
      if (e[0].DEFECTDESCRIPTION == this.defectDescriptionInput.DEFECTDESCRIPTION && e[0].SPACEDESCRIPTION == this.spaceDescriptionInput.SPACEDESCRIPTION){
        check = true;
      }
    });
    if (check == false){
      this.propertyDefects.push([{"DEFECTID":this.defectDescriptionInput.DEFECTID, "DEFECTDESCRIPTION":this.defectDescriptionInput.DEFECTDESCRIPTION, "PROPERTYDEFECTQUANTITY":this.propertyDefectQuantityInput, "SPACEID":this.spaceDescriptionInput.SPACEID, "SPACEDESCRIPTION":this.spaceDescriptionInput.SPACEDESCRIPTION}])
    }
    console.log(this.propertyDefects);
    }
  }

  deleteDefect(i){
    this.propertyDefects.splice(i, 1)
    console.log(this.propertyDefects);
  }
}



