import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-reporting',
  templateUrl: './reporting.component.html',
  styleUrls: ['./reporting.component.scss']
})

export class ReportingComponent implements OnInit {
  public reports;

  //GENERATE AGENT REPORT
  public startDate: any;
  public endDate: any;
  public token: any;
  public reportDate: any;
  public reportUser: any;
  public reportpropertyid: any;
  public reportpropertyaddress: any;
  public reportownerid: any;
  public reportownername: any;
  public reportonwersurname: any;
  public reportmarkettypeid: any;
  public reportmarkettypedescription: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService) { }

  async ngOnInit() {
    //this.reports = await this.service.Get('/report');
    //console.log(this.reports);
  }

  //Generate agent report form validation
  async submitGenerateAgent(){
    console.log("hit")
      if (this.startDate != null && this.endDate != null  && this.startDate <= this.endDate){
      $("#agentreportModal").modal('hide');
      this.service.GenerateReport(this.startDate, this.endDate);
      this.router.navigateByUrl("agent-report");
    }
    else{
      this.toastr.warning("The End Date cannot be earlier than the Start Date")
    }
  }

  async submitGenerateAudit(){
    console.log("hit")
      //if (this.startDate != null && this.endDate != null  && this.startDate <= this.endDate){
      //$("#auditreportModal").modal('hide');
      //$("#confirmSaveReportModal").modal('show');
      //this.service.GenerateReport(this.startDate, this.endDate);
      this.router.navigateByUrl("audit-report");
    //}
    //else{
      //this.toastr.warning("The End Date cannot be earlier than the Start Date")
    //}
  }

  //AUDIT REPORT
  async save(){

  }

  showSaveSuccess() {
    this.toastr.success('Audit report saved successfully', "", {
      timeOut: 1000,
    });
    setTimeout(location.reload.bind(location), 1000);
  }

}
