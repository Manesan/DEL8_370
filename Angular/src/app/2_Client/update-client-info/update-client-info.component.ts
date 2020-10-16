import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-update-client-info',
  templateUrl: './update-client-info.component.html',
  styleUrls: ['./update-client-info.component.scss']
})
export class UpdateClientInfoComponent implements OnInit {
  public clients;
  public token: any; //holds user token

 //Define all the variables to be used
 public name: any;
 public surname: any;
 public number: any;
 public address: any;
 public altnumber: any;
 public email: any;
 public idnumber: any;
 public passportnumber: any;
 public clientdocuments: any;
 public clientdocumenttypedescription: any;
 public documenttypes: any;
 public clientdocument: any;
 public documenttypeid: any;

 public SouthAfrican: any;


 //Client document
 public fileBase64clientdocument: string;
 public fileExtensionclientdocument: string;
 resultsclient: any;
  documenttype: string;
  clientDocument: any;


  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showUpdateSuccess() {
    this.toastr.success('Your profile has been updated successfully!', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

  showUploadSuccess()
   {
    this.toastr.success('Document uploaded successfully', "", {
      timeOut: 2000,
    });
    setTimeout(location.reload.bind(location), 2000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    let client = await this.service.Get('/clientinfo?token=' + this.token.token) as any;
    console.log(client);
    this.name = client[0].USERNAME;
    this.surname = client[0].USERSURNAME;
    this.number = client[0].USERCONTACTNUMBER;
    this.altnumber = client[0].USERALTCONTACTNUMBER;
    this.address = client[0].USERADDRESS;
    this.email = client[0].USEREMAIL;
    this.idnumber = client[0].USERIDNUMBER;
    this.passportnumber = client[0].USERPASSPORTNUMBER;

    if (this.idnumber != "null"){
      this.SouthAfrican = 'Yes';
    }
    else{
      this.SouthAfrican = 'No';
    }

    console.log(this.idnumber, this.passportnumber);

    this.clientdocumenttypedescription= client [0].USERDOCUMENTTYPEDESCRIPTION
    console.log(client[0].ClientDocuments);
    this.clientdocuments = client[0].ClientDocuments;
    console.log(this.clientdocuments);
    this.documenttypes = await this.service.Get('/documenttypes') as any;
    //console.log(this.documenttypes);
    this.clientdocument= client[0].CLIENTDOCUMENT;
    this.spinner.hide();
  }

  async update(){
    if(this.name !== null&& this.surname!==null && this.number!==null && this.address!==null && this.email!==null && this.idnumber!==null || this.passportnumber!==null)
    {
      $("#confirmAddModal").modal('show');
      this.token ={"token" : localStorage.getItem("37y7ffheu73")}
      console.log(this.number)
      await this.service.Patch(`/updateclientinfo?token=${this.token.token}&name=${this.name}&surname=${this.surname}&contactnumber=${this.number}
      &altcontactnumber=${this.altnumber}&address=${this.address}&idnumber=${this.idnumber}&passportnumber=${this.passportnumber}`);
      this.showUpdateSuccess();
    }

    if (this.idnumber != "null"){
      this.SouthAfrican = 'Yes';
    }
    else{
      this.SouthAfrican = 'No';
    }
  }


   //PROPERTY DOCUMENT CONVERSION
   clientDocumentChangeListener($event){
    this.readThisClientDocument($event.target);

    let img: any = document.getElementById("clientdocument");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultsclient = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisClientDocument(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensionclientdocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64clientdocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
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

  async upload()
  {
    let clientdoc = {
      "FileBase64" : this.fileBase64clientdocument,
      "FileExtension" : this.fileExtensionclientdocument
    };
    //console.log(clientdoc);
    $("#uploadDocsModal").modal('hide');
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    await this.service.Post(`/updateclientinfo?token=${this.token.token}&documenttypeid=${this.documenttypeid}`, clientdoc);
    this.showUploadSuccess();
  }
}
