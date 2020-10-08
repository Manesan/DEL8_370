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
  selector: 'app-contact-agent',
  templateUrl: './contact-agent.component.html',
  styleUrls: ['./contact-agent.component.scss']
})

export class ContactAgentComponent implements OnInit {
  public token: any; //holds user token

  //Define all the variables to be used
  public email: any;
  public name: any;
  public surname: any;
  public subject: any;
  public message: any;
  public contact: any;
  public countryCode: any;
  public altCountryCode: any;
  public countryCodes = CountryCodes;
  public poppedNumber: any;
  public newContact: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

 // ngOnInit(): void { }

  showMessageSentSuccess() {
    this.toastr.success('Your message has been delivered. Keep an eye on your email for our response', "", {
      timeOut: 4000,
    });
    this.router.navigateByUrl('');
  }


async ngOnInit()
 {

}


async sendmessage()
{
  //console.log("hit")
  this.phoneNumberJoiner();
  this.token ={"token" : localStorage.getItem("37y7ffheu73")}
  //console.log(this.descriptionInput);
  let response = await this.service.Post(`/contactagent?token=${this.token.token}&name=${this.name}&surname=${this.surname}&email=${this.email}&subject=${this.subject}&message=${this.message}&contactnumber=${this.newContact}`);
  console.log(response)
  if (response == true){
    this.showMessageSentSuccess ();
  }
}

  phoneNumberJoiner()
  {
    if (this.contact != null){
      this.newContact = "%2B"+this.countryCode.substring(1)+this.contact.substring(1);
    }
  }
}
