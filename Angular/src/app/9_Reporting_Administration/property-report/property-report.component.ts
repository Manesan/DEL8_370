import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
//import { jsPDF } from "jspdf";
import {Chart} from 'chart.js';

@Component({
  selector: 'app-property-report',
  templateUrl: './property-report.component.html',
  styleUrls: ['./property-report.component.scss']
})
export class PropertyReportComponent implements OnInit {
  public token: any; //holds user token
  public reportDate: any;
  public reportUser: any;
  public reportAgentDate: any;
  public reportPropertyID: any;
  public reportPropertyAddress: any;
  public reportPropertyAddedDate: any;
  public reportPropertyTypeID: any;
  public reportMarketTypeID: any;
  public reportMarketTypeDescription: any;
  public reportPropertyOwnerID: any;
  public reportPropertyOwnerName: any;
  public reportPropertyOwnerSurname: any;
  public Defects: any;

  public startDate: any;
  public endDate: any;
  public Properties = [];
  public allProperties: any;
  public rentals: any;
  public sales: any;
  public houses = [];
  public apartments = [];
  public townhouses = [];
  chart=[];
  chart2=[];
  public name: any;
  public surname: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit() {
    this.service._startDate.subscribe(startDate => this.startDate = startDate);
    this.service._endDate.subscribe(endDate => this.endDate = endDate);
    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
    let reportDetails = await this.service.Get(`/propertyreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`) as any;
    this.Properties = reportDetails.Properties;
    console.log(reportDetails.CurrentUser)
    this.rentals = reportDetails.rentals;
    this.sales = reportDetails.sales;
    console.log(reportDetails.CurrentUser[0].USERNAME);
    this.name = reportDetails.CurrentUser[0].USERNAME;
    this.surname = reportDetails.CurrentUser[0].USERSURNAME;
    
    this.Defects = reportDetails.Defects
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser[0];
    console.log(reportDetails.Properties[0])
    reportDetails.Properties.forEach(e => {
      if (e.PROPERTYTYPEID == 1){
        this.houses.push(e);
      }
      if (e.PROPERTYTYPEID == 2){
        this.apartments.push(e);
      }
      if (e.PROPERTYTYPEID == 3){
        this.townhouses.push(e);
      }      
    });

    //console.log(this.Properties);
    console.log(reportDetails.Properties[0].Defects[0][0].DEFECTDESCRIPTION)
    console.log(  this.houses, this.houses.length)


    this.chart = new Chart ('pie-chart', {
    type: 'pie',
    data: {
      labels: ["Apartment", "House", "Townhouse"],
      datasets: [{
        label: 'Number of Properties per Property Type',
        data: [this.apartments.length, this.houses.length, this.townhouses.length,],
        barPercentage: 0.5,
        backgroundColor: [
          '#C18D21',
          '#E89C31',
          '#DBA858',

      ],
      }]
    },
    options: {
      title: {
        display: true,
        text: 'Number of Properties per Property Type'
      }
    }
    })
    console.log(reportDetails);


    //Chart
    let keys = ["Total Rentals", "Total Sales"];  // reportDetails["apartments"].map(c => c.Count)
    let keys2 = "Total Sales" // reportDetails["apartments"].map(c => c.Count)

    let values = [this.rentals.length, this.sales.length]; // reportDetails["townhouses"].map(c => c.ProductCount)
    let values2 = this.sales.length;

    // this.Properties = reportDetails["Properties"];
    // console.log(this.allProperties)

    this.chart2 = new Chart ('canvas', {
      type: 'bar',
      data: {
        labels: keys,
        datasets: [{
          label: 'Total Number of Sales and Rentals',
          data: values,
          barPercentage: 0.5,
          backgroundColor: [
            '#C18D21',
            '#E89C31',
        ],
        }]
      },
      options: {
        scales:{
          yAxes: [{
            ticks: {
              min: 0
            }
          }],
        }
      }
    })
  }

  DownloadReport(){
    
  }
}
