import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
//import { jsPDF } from "jspdf";
import {Chart} from 'chart.js';
import 'jspdf-autotable';
declare var jsPDF: any;
import html2canvas from 'html2canvas';

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
  public SaleProperties: any;
  public RentalProperties: any;
  public houses = [];
  public apartments = [];
  public townhouses = [];
  chart=[];
  chart2=[];
  public name: any;
  public surname: any;
  public lineBreak = "\r\n";

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit() {
    this.service._startDate.subscribe(startDate => this.startDate = startDate);
    this.service._endDate.subscribe(endDate => this.endDate = endDate);

    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
    let reportDetails = await this.service.Get(`/propertyreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`) as any;
    this.SaleProperties = reportDetails.SaleProperties;
    this.SaleProperties.forEach(e => {
      e.PROPERTYADDEDDATE = e.PROPERTYADDEDDATE.split('T')[0];
    });
    this.RentalProperties = reportDetails.RentalProperties;
    this.RentalProperties.forEach(e => {
      e.PROPERTYADDEDDATE = e.PROPERTYADDEDDATE.split('T')[0];
    });
    console.log(reportDetails.CurrentUser)
    this.rentals = reportDetails.rentals;
    this.sales = reportDetails.sales;
    console.log(reportDetails.CurrentUser[0].USERNAME);
    this.name = reportDetails.CurrentUser[0].USERNAME;
    this.surname = reportDetails.CurrentUser[0].USERSURNAME;
    
    this.Defects = reportDetails.Defects
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser[0];
    //console.log(reportDetails.Properties[0])
    this.SaleProperties.forEach(e => {
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
    this.RentalProperties.forEach(e => {
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

    console.log(this.RentalProperties, this.SaleProperties);
    console.log( this.apartments.length , this.houses.length, this.townhouses.length)


    this.chart = new Chart ('pie-chart', {
    type: 'pie',
    data: {
      labels: ["Apartment", "House", "Townhouse"],
      datasets: [{
        label: 'Number of Properties per Property Type',
        data: [this.apartments.length, this.houses.length, this.townhouses.length],
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

    let values = [this.SaleProperties.length, this.RentalProperties.length]; // reportDetails["townhouses"].map(c => c.ProductCount)
    let values2 = this.sales.length;

    // this.Properties = reportDetails["Properties"];
    // console.log(this.allProperties)

    this.chart2 = new Chart ('canvas', {
      type: 'bar',
      data: {
        labels: keys,
        datasets: [{
          label: 'Total Sales and Rentals',
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

  async DownloadReport(){
    
    window.scrollTo(0,0);
    var doc = new jsPDF("a4")
    var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
    var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
    let finalY = 100;

    //Header
    let data = document.getElementById("headerDiv"); 
    let contentDataURL: any = await html2canvas(data).then(canvas => {
      let contentDataURL = canvas.toDataURL('image/png'); 
      return contentDataURL;
    });     
    //console.log("hit", contentDataURL)
    doc.addImage(contentDataURL, 'PNG', 10, 12, pageWidth-20, pageHeight-280); 

    //Sub Header
    let data1 = document.getElementById("subHeading");  
    let contentDataURL1: any = await html2canvas(data1).then(canvas => {
      let contentDataURL1 = canvas.toDataURL('image/png'); 
      return contentDataURL1;
    });     
    //console.log("hit", contentDataURL)
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  

    //Date table
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#dateTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

      //small sale table
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#smallSaleTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

    //Sales tables
    let length1 = this.SaleProperties.length;
    for(let i=0; i<length1; i++){
      doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#saleTable'+i, useCss: true, head: [
        ['Sale Properties']]})
        finalY = doc.autoTable.previous.finalY;
    }

    //small rental table
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#smallRentalTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

    //Rental tables
    let length2 = this.RentalProperties.length;
    for(let i=0; i<length2; i++){
      doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#rentalTable'+i, useCss: true, head: [
        ['Rental Properties']]})
        finalY = doc.autoTable.previous.finalY;
    }

    //small total table
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#smallTotalTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

    //Pie chart
    var newCanvas = <HTMLCanvasElement>document.querySelector('#pie-chart');
    doc.addPage();
    var newCanvasImg = newCanvas.toDataURL("image/png", 1.0);
    doc.addImage(newCanvasImg, 'PNG', 0, 15, 210, 100); 

    //Bar chart
    var newCanvas = <HTMLCanvasElement>document.querySelector('#canvas');
    var newCanvasImg = newCanvas.toDataURL("image/png", 1.0);
    doc.addImage(newCanvasImg, 'PNG', 0, 130, 210, 100); 
    doc.text("***End of report***", (pageWidth / 2.5), 240)

    doc.save('Property Report')


  }
}
