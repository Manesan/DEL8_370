import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { timer } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-agent-sale-agreements',
  templateUrl: './agent-sale-agreements.component.html',
  styleUrls: ['./agent-sale-agreements.component.scss']
})
export class AgentSaleAgreementsComponent implements OnInit {
  public saleAgreements: any;//holds list to populate cards
  public showViewModal: boolean; //bool for view modal
  public token: any; //holds user token

  public saleAgreementDocument: any;
  documenttype: string;
  saleagreement: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private toastr: ToastrService, private spinner: NgxSpinnerService) { }

  async ngOnInit() {
    this.spinner.show();
    this.token ={"token" : localStorage.getItem("37y7ffheu73")};
    this.saleAgreements = await this.service.Get('/agentsaleagreement?token=' + this.token.token);
    this.spinner.hide();
    //console.log(this.saleAgreements)
  }

  async getSaleAgreement(id){
    this.spinner.show();
    //console.log(id)
    this.documenttype = "SaleAgreement";
    this.saleagreement = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;

    const linkSource = 'data:application/pdf;base64,' + this.saleagreement;
    const downloadLink = document.createElement("a");
    const fileName = 'Sale_Agreement';

    downloadLink.href = linkSource;
    downloadLink.download = fileName;
    downloadLink.click();
    this.spinner.hide();
  }

}
