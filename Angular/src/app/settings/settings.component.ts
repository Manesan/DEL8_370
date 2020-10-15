import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  public token: any;
  public duration: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  async ngOnInit(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let time = await this.service.Get('/timer?token=' + this.token.token) as any;
    this.duration = time.USERLOGINTIMEOUTDESCRIPTION;
    this.spinner.hide();
  }

  showUpdateSuccess() {
    this.toastr.success('Session time updated successfully', "", {
      timeOut: 1000,
    });
    //this.ngOnInit();
    setTimeout(location.reload.bind(location), 1000);
  }

  async changeTimer(){
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    //console.log(id);
    await this.service.Patch(`/timer?token=${this.token.token}&duration=${this.duration}`);
    this.spinner.hide();
    this.showUpdateSuccess();
  }

}
