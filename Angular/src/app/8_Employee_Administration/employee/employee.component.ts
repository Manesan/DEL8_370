import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { CountryCodes } from '../../../assets/CountryCodes';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})

export class EmployeeComponent implements OnInit {
  public employees; //holds list to populate cards
  public employeetypes;
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  //Define all the variables to be used
  public userid: any;
  public employeetypeid: any;
  public name: any;
  public surname: any;
  public contactnumber: any;
  public altcontactnumber: any;
  public email: any;
  // public idorpassportnumber: any;
  // public passportnumber: any;
  public address: any;
  public banking: any;
  public employeeTypeDescriptionInput: any;
  public employeeTypeDescriptions = [];
  public dateemployed: any;
  public renumeration: any;
  public password: any;
  public countryCode: any;
  public altCountryCode: any;
  public countryCodes = CountryCodes;
  public poppedNumber: any;
  public SouthAfrican: any;
  public idnumber: any;
  public passportnumber: any;
  public newContactNumber: any;
  public newAltContactNumber: any;


  public counter: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  showAddSuccess() {
    this.toastr.success('Employee added successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    // setTimeout(location.reload.bind(location), 1000);
  }

  showUpdateSuccess() {
    this.toastr.success('Employee updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteSuccess() {
    this.toastr.success('Employee deleted successfully', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    // setTimeout(location.reload.bind(location), 1000);
  }

  showDeleteFailure() {
    this.toastr.error('Employee cannot be deleted', "", {
      timeOut: 1000,
    });
    this.ngOnInit();
    //setTimeout(location.reload.bind(location), 1000);
  }

  async ngOnInit() {
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(this.token.token);
    this.employees = await this.service.Get('/employee?token=' + this.token.token);
    console.log(this.employees);
    this.employeetypes = await this.service.Get('/employeetype?token=' + this.token.token);
    console.log(this.employeetypes);

    //added
    this.showViewModal = false
    this.name = null;
    this.surname = null;
    this.contactnumber = null;
    this.altcontactnumber = null;
    this.email = null;
    this.idnumber = null;
    this.passportnumber = null;
    this.address = null;
    this.banking = null;
    this.employeetypeid = null
    this.dateemployed = null;
    this.renumeration = null;
  }

  async add(){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.phoneNumberJoiner();
    let employeeroles = this.employeeTypeDescriptions;
    console.log(this.employeeTypeDescriptions);
    console.log(this.employees);
    console.log(this.email);
    console.log(this.idnumber, this.passportnumber);

    await this.service.Post(`/employee?token=${this.token.token}&name=${this.name}&surname=${this.surname}&contactnumber=${this.newContactNumber}
    &altcontactnumber=${this.newAltContactNumber}&email=${this.email}&idnumber=${this.idnumber}&passportnumber=${this.passportnumber}
    &address=${this.address}&banking=${this.banking}&dateemployed=${this.dateemployed}
    &remuneration=${this.renumeration}&password=${this.password}` , employeeroles);
    this.showAddSuccess();
  }

  async view(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    $("#editModal").modal('show');
    this.phoneNumberJoiner();
    //console.log(id);
    let user = await this.service.Get('/employee?token=' + this.token.token + '&id='+ id) as any;
    console.log(user.USERNAME);
    this.name = user.USERNAME;
    this.surname = user.USERSURNAME;
    this.contactnumber = user.USERCONTACTNUMBER;
    //console.log(this.contactnumber);
    this.altcontactnumber = user.USERALTCONTACTNUMBER;
    this.email = user.USEREMAIL;
    this.idnumber = user.USERIDNUMBER;
    this.passportnumber = user.USERPASSPORTNUMBER;
    if (this.idnumber != null){
      this.SouthAfrican = 'Yes';
    }
    else if (this.passportnumber != null){
      this.SouthAfrican = 'No';
    }
    this.address = user.USERADDRESS;
    this.password = user.USERPASSWORD;
    this.renumeration = user.EMPLOYEERENUMERATON;
    this.dateemployed = user.EMPLOYEEDATEEMPLOYED.split("T")[0];
    this.banking = user.EMPLOYEEBANKINGDETAILS;

    this.userid = user.USERID;
    //this.employeetypeid = user.EmployeeType[0].EMPLOYEETYPEID;
    user.EmployeeType.forEach(e => {
      this.employeeTypeDescriptions.push(e);
      this.employeetypes.forEach(f => {
        if (f.EMPLOYEETYPEID == e.EMPLOYEETYPEID && f.EMPLOYEETYPEDESCRIPTION == e.EMPLOYEETYPEDESCRIPTION){
          this.employeeTypeDescriptionInput = f;
        }
      });
      
      });
    console.log(this.idnumber, this.passportnumber, user)
  //   for (let i = 0; i < this.employeeTypeDescription.length; i++){
  //     switch(this.employeeTypeDescription[i].EMPLOYEETYPEDESCRIPTION){
  //       case "Director":
  //         this.employeeTypeDescriptionInput[0] = true;
  //         break;
  //       case "Agent":
  //         this.employeeTypeDescriptionInput[1] = true;
  //         break;
  //       case "Home Inspector":
  //         this.employeeTypeDescriptionInput[2] = true;
  //         break;
  //       case "Valuer":
  //         this.employeeTypeDescriptionInput[3] = true;
  //         break;
  //       case "Administrator":
  //         this.employeeTypeDescriptionInput[4] = true;
  //         break;
  //       case "Secretary":
  //         this.employeeTypeDescriptionInput[5] = true;
  //         break;
  //   }
  // }
    //this.showViewModal = true;
    console.log(this.employeetypes, this.employeeTypeDescriptionInput)
  }

  async update(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    this.phoneNumberUpdater();
    let employeeroles = this.employeeTypeDescriptions;
    console.log(this.employeeTypeDescriptions);
    console.log(this.employees);
    console.log(this.passportnumber);

    await this.service.Patch(`/employee?token=${this.token.token}&id=${id}&name=${this.name}&surname=${this.surname}&contactnumber=${this.newContactNumber}&altcontactnumber=${this.newAltContactNumber}&email=${this.email}&idnumber=${this.idnumber}&passportnumber=${this.passportnumber}
    &address=${this.address}&banking=${this.banking}
    &dateemployed=${this.dateemployed}&remuneration=${this.renumeration}` , employeeroles);
    this.showViewModal = false; //Added
    this.showUpdateSuccess();
  }

  async deleteBinding(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let user = await this.service.Get('/employee?token=' + this.token.token + '&id='+ id) as any;
    this.name = user.USERNAME;
    this.surname = user.USERSURNAME;
    this.userid = user.USERID;
    //console.log(this.nameInput & surnameInput);
  }

  async delete(id){
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let response = await this.service.Delete('/employee?token=' + this.token.token + '&id='+ id);
    console.log(response)
    if (response === 409){
      this.showDeleteFailure();
    }
    else{
      this.showDeleteSuccess();
    }
  }

    //Add form validation
    async submitAdd(){
    
        //console.log(this.descriptionInput);
        $("#confirmAddModal").modal('show');
        $("#addModal").modal('hide');
    
    }

    //Update form validation
      async submitUpdate(){
          //console.log(this.descriptionInput);
          $("#editModal").modal('hide');
          $("#confirmEditModal").modal('show');
      }

    phoneNumberJoiner()
    {
      if (this.contactnumber != null){
        this.newContactNumber = "%2B"+this.countryCode.substring(1)+this.contactnumber.substring(1);
      }
      if (this.altcontactnumber != null){
        this.newAltContactNumber = "%2B"+this.altCountryCode.substring(1)+this.altcontactnumber.substring(1);
      }
    }

    phoneNumberUpdater(){
      if (this.contactnumber != null){
        this.poppedNumber = this.contactnumber.substring(1);
        this.contactnumber = "%2B"+this.poppedNumber;
        console.log(this.contactnumber)
      }
      if (this.altcontactnumber != null){
        this.poppedNumber = this.altcontactnumber.substring(1);
        this.altcontactnumber = "%2B"+this.poppedNumber;
      }
    }

    addEmployeeType(){
      if (this.employeeTypeDescriptionInput != null){
        let check = false;
      this.employeeTypeDescriptions.forEach(e => {
        console.log(e, this.employeeTypeDescriptionInput)
        if (e.EMPLOYEETYPEDESCRIPTION ==  this.employeeTypeDescriptionInput.EMPLOYEETYPEDESCRIPTION){
          check = true;
        }
      });
      if (check == false){
        this.employeeTypeDescriptions.push(this.employeeTypeDescriptionInput)
      console.log(this.employeeTypeDescriptions);
      }
      }
    }
  
    deleteEmployeeType(i){
      this.employeeTypeDescriptions.splice(i, 1)
      console.log(this.employeeTypeDescriptions);
    }

}
