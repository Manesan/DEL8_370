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
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.scss']
})

export class ClientComponent implements OnInit {
  public clients; //holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public userid: any;
  public clienttypeid: any;
  public nameInput: any;
  public surnameInput: any;
  public contactNumberInput: any;
  public alternativeContactNumberInput: any;
  public emailInput: any;
  public idNumberInput: any;
  public passportNumberInput: any;
  public addressInput: any;
  public passwordInput: any;
  public clientTypeInput: any;
  public propertyAddressInput: any;
  public creditCheckReportDocument: any;
  public copyOfIdDocument: any;
  public copyOfPassportDocument: any;
  public proofOfResidenceDocument: any;
  public users: any;
  public clientid: any;

  public credituploaddate: any;
  public creditexpirydate: any;

   //Credit check document
   public fileBase64creditdocument: string;
   public fileExtensioncreditdocument: string;
   resultscredit: any;


  public residenceuploaddate: any;
  public residenceexpirydate: any;
  public iduploaddate: any;
  public idexpirydate: any;
  public passportuploaddate: any;
  public passportexpirydate: any;
  documenttype: string;
  propertyDocument: any;
  creditCheckReportID: any;
  proofOfResidenceID: any;
  copyOfIdDocumentID: any;
  clientDocument: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  showUploadSuccess() {
    this.toastr.success('Document uploaded successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    // setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.clients = await this.service.Get('/viewclient?token=' + this.token.token);
    //console.log(this.clients);

    //added
    this.showViewModal = false
    this.nameInput = null;
    this.surnameInput = null;
    this.alternativeContactNumberInput = null;
    this.emailInput = null;
    this.idNumberInput = null;
    this.passportNumberInput = null;
    this.addressInput = null;
    this.passwordInput = null;
    this.clientTypeInput = null;
    this.propertyAddressInput = null;
    this.spinner.hide();
  }

  //PROPERTY DOCUMENT CONVERSION
  creditCheckChangeListener($event){
    this.readThisCreditCheck($event.target);

    let img: any = document.getElementById("creditCheckReportDocument");
    if(typeof (FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e:any) => {
        this.resultscredit = e.target.result;
      }
      reader.readAsArrayBuffer(img.files[0]);
    }
   }
   //Method that reads the file information and parses it
   readThisCreditCheck(inputValue: any): void{
     //Gets the actual file
     var file: File = inputValue.files[0];
     //Creates a reader that will read the file for information
     var myReader: FileReader = new FileReader();

     //Get the extension by splitting the name and popping the name off
     this.fileExtensioncreditdocument = file.name.split('.').pop();

     //Once the reader has loaded, load the Base64 string of the file into the variable declared earlier
     myReader.onloadend = (e) => {
       this.fileBase64creditdocument = myReader.result.toString().substr(myReader.result.toString().indexOf(',') + 1);
       //console.log(this.fileBase64);
     }
     //Read the file in and parse it to Base64
     myReader.readAsDataURL(file);
   }

  async view(id){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#editModal").modal('show');
    this.clientid = id;
    //console.log(id);
    let user = await this.service.Get('/viewclient?token=' + this.token.token + '&id='+ id) as any;
    console.log(user[0]);
    this.nameInput = user[0].USERNAME;
    this.surnameInput = user[0].USERSURNAME;
    this.contactNumberInput = user[0].USERCONTACTNUMBER;
    this.alternativeContactNumberInput = user[0].USERALTCONTACTNUMBER;
    this.emailInput = user[0].USEREMAIL;
    this.idNumberInput = user[0].USERIDNUMBER;
    this.passportNumberInput = user[0].USERPASSPORTNUMBER;
    this.addressInput = user[0].USERADDRESS;
    this.clientTypeInput = user[0].CLIENTTYPEDESCRIPTION;
    this.users = user[0].USERNAME + " " + user[0].USERSURNAME;
    this.userid = user.USERID;
    this.clienttypeid = user.CLIENTTYPEID;
    this.spinner.hide();
    this.creditCheckReportDocument = user[0].creditcheckreport.CLIENTDOCUMENT1;
    this.creditCheckReportID = user[0].creditcheckreport.CLIENTDOCUMENTID;
    this.credituploaddate = user[0].creditcheckreport.CLIENTDOCUMENTUPLOADDATE;
    this.creditexpirydate = user[0].creditcheckreport.CLIENTDOCUMENTUPLOADEXPIRY;
    this.proofOfResidenceDocument = user[0].proofofresidence.CLIENTDOCUMENT1;
    this.proofOfResidenceID = user[0].proofofresidence.CLIENTDOCUMENTID;
    this.residenceuploaddate = user[0].proofofresidence.CLIENTDOCUMENTUPLOADDATE;
    this.copyOfIdDocument = user[0].idcopy.CLIENTDOCUMENT1;
    this.copyOfIdDocumentID = user[0].idcopy.CLIENTDOCUMENTID;
    this.iduploaddate = user[0].idcopy.CLIENTDOCUMENTUPLOADDATE;
    // this.copyOfPassportDocument = user[0].passportcopy.CLIENTDOCUMENT1;
    // this.passportuploaddate = user[0].passportcopy.CLIENTDOCUMENTUPLOADDATE;
  }

  async getClientCreditDocument(id){
    //console.log(id)
    this.documenttype = "ClientDocument";
    this.clientDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    const linkSource = 'data:application/pdf;base64,' + this.clientDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Credit Check Report' + this.nameInput + this.surnameInput;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async getClientIDDocument(id){
    //console.log(id)
    this.documenttype = "ClientDocument";
    this.clientDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    const linkSource = 'data:application/pdf;base64,' + this.clientDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Copy of ID/Passport' + this.nameInput + this.surnameInput;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

 async getClientResidenceDocument(id){
  //console.log(id)
    this.documenttype = "ClientDocument";
    this.clientDocument = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
    const linkSource = 'data:application/pdf;base64,' + this.clientDocument;
    const downloadLink = document.createElement("a");
    const fileName = 'Proof of Residence' + this.nameInput + this.surnameInput;

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
  }

  async upload(){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}

    let creditcheckreport = {
      "FileBase64" : this.fileBase64creditdocument,
      "FileExtension" : this.fileExtensioncreditdocument
    };
    //console.log(creditcheckreport);
    await this.service.Post(`/viewclient?token=${this.token.token}&clientid=${this.clientid}`, creditcheckreport);
    this.showUploadSuccess();
  }

  //Upload form validation
  async submitUpload(){
      //console.log(this.creditcheckreport);
      $("#uploadModal").modal('hide');
      $("#confirmUploadModal").modal('show');
  }



}
