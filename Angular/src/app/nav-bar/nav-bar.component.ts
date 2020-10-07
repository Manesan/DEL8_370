import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../../environments/api.service'
import { LoginComponent } from '../1_User/login/login.component'
declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
public loggedOut: boolean;
public loggedIn: boolean;
public token: any;
public roles: any;
public admin: any;

  constructor(private router: Router, public service: ApiService) { }

  async ngOnInit(){
    if(localStorage.getItem("37y7ffheu73")){
      this.token ={"token" : localStorage.getItem("37y7ffheu73")}

      // this.roles =  await this.service.Get('/tokenroles?token=' + this.token.token) as any;
      // //console.log(this.roles);
      // if(this.roles.indexOf(5) !== -1 || this.roles.indexOf(6) !== -1 ){  //--CHECK FOR THE CORRECT ROLES HERE
      //   //console.log("admin");
      //   this.admin = true;
      // }
      // else{
      //   this.admin = false;
      // }
    }
  }

  Logout(){
    localStorage.setItem("37y7ffheu73", "");
    localStorage.setItem("8d9s8fhvh9f", "");
    this.router.navigate(['/login']);
  }

  showAdminPortal(){
    $("#adminbutton").show();
  }

}
