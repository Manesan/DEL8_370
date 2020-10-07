import { Component, OnInit } from '@angular/core';
import { HttpClient }  from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../../../environments/api.service';
import { NavBarComponent } from '../../nav-bar/nav-bar.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  token: any;
  email:any;
  password:any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private route:ActivatedRoute, private navbar: NavBarComponent, private toastr: ToastrService) { }

  ngOnInit(): void {
    if (localStorage.getItem("37y7ffheu73")){
      this.router.navigate([""]);
      this.navbar.ngOnInit();
  }
}

async Login(){
  this.token = await this.service.Login(`/user?email=${this.email}&password=${this.password}`)

  if(this.token == null){
    this.failedLogin();
  }
  else{
     //Check if user is admin, director or agent
      if (this.token.roles){
        if(this.token.roles.indexOf(5) > -1 || this.token.roles.indexOf(1) > -1 || this.token.roles.indexOf(2) > -1){
          localStorage.setItem("8d9s8fhvh9f", this.token.roles)
        }
      }

      localStorage.setItem("37y7ffheu73", this.token.token)
      this.router.navigate([""])
  }

}


failedLogin() {
  this.toastr.error('Incorrect username or password', "", {
    timeOut: 2000,
  });
}
}
