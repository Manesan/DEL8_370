import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
//import { jsPDF } from "jspdf";
import 'jspdf-autotable';
declare var jsPDF: any;
import html2canvas from 'html2canvas';
import { url } from 'inspector';

@Component({
  selector: 'app-audit-report',
  templateUrl: './audit-report.component.html',
  styleUrls: ['./audit-report.component.scss']
})
export class AuditReportComponent implements OnInit {
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
  public PropertyCount: any;
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
  public TotalPropertySales: any;
  public AverageSalePrice: any;
  public MaxSalePrice: any;
  public MinSalePrice: any;
  public TotalPropertiesRented: any;
  public AverageRentalPrice: any;
  public MaxRentalPrice: any;
  public MinRentalPrice: any;
  public lineBreak = "\r\n";

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit() {
    // this.service._startDate.subscribe(startDate => this.startDate = startDate);
    // this.service._endDate.subscribe(endDate => this.endDate = endDate);
    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
    let reportDetails = await this.service.Get(`/auditreport?token=${this.token.token}`) as any;

    this.TotalPropertySales = reportDetails.TotalPropertySales;
    this.AverageSalePrice = this.number_format(reportDetails.AverageSalePrice);
    this.MaxSalePrice = this.number_format(reportDetails.MaxSalePrice.SALEAMOUNT);
    this.MinSalePrice = this.number_format(reportDetails.MinSalePrice.SALEAMOUNT);
    this.TotalPropertiesRented = reportDetails.TotalPropertiesRented;
    this.AverageRentalPrice = this.number_format(reportDetails.AverageRentalPrice);
    this.MaxRentalPrice = this.number_format(reportDetails.MaxRentalPrice.PRICEAMOUNT);
    this.MinRentalPrice = this.number_format(reportDetails.MinRentalPrice.PRICEAMOUNT);
    this.PropertyCount = reportDetails.PropertyCount;
    console.log(reportDetails.MaxSalePrice)
    /*console.log(reportDetails.CurrentUser)
    this.rentals = reportDetails.rentals;
    this.sales = reportDetails.sales;
    console.log(reportDetails.CurrentUser[0].USERNAME);*/
    this.name = reportDetails.CurrentUser[0].USERNAME;
    this.surname = reportDetails.CurrentUser[0].USERSURNAME;
    
    
    //this.Defects = reportDetails.Defects
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser[0];
    /*console.log(reportDetails.Properties[0])
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

    console.log(this.houses[0].Municipal?.PROPERTYDOCUMENT1, this.houses[0].TitleDeed?.PROPERTYDOCUMENT1, this.houses[0].MunicipalReport?.PROPERTYDOCUMENT1,
      this.houses[0].Valuation?.PROPERTYDOCUMENT1, this.houses[0].LightsWater?.PROPERTYDOCUMENT1, this.houses[0].Levies?.PROPERTYDOCUMENT1);*/
  }

  async SaveReport(){
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
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  

    /*let length = document.getElementsByClassName("tablelines").length;
    for(let i=0; i<length; i++){*/
      doc.autoTable({margin: { bottom: 10},startY: finalY + 20, html: '#table'/*+i*/, useCss: true, head: [
        ['Properties']]})
        finalY = doc.autoTable.previous.finalY;
    //}
    doc.text("***End of report***", (pageWidth / 2.5), finalY+20)

    var out = doc.output('blob');
    var reader = new FileReader();

    reader.readAsDataURL(out); 
    reader.onloadend = function() { // for blob to base64
        let base64data = reader.result; 
        console.log("base64 data is ");               
        console.log(base64data );
  }}

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
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  

    /*let length = document.getElementsByClassName("tablelines").length;
    for(let i=0; i<length; i++){*/
      doc.autoTable({margin: { bottom: 10},startY: finalY + 20, html: '#table'/*+i*/, useCss: true, head: [
        ['Properties']]})
        finalY = doc.autoTable.previous.finalY;
    //}
    doc.text("***End of report***", (pageWidth / 2.5), finalY+20)

    doc.save('Audit Report')

    
  }

  number_format(number) {
    let decimals = 2;
    let dec_point = '.';
    let thousands_sep = ' ';
    number = (number + '')
      .replace(/[^0-9+\-Ee.]/g, '');
    var n = !isFinite(+number) ? 0 : +number,
      prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
      sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
      dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
      s: any = '',
      toFixedFix = function(n, prec) {
        var k = Math.pow(10, prec);
        return '' + (Math.round(n * k) / k)
          .toFixed(prec);
      };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n))
      .split('.');
    if (s[0].length > 3) {
      s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '')
      .length < prec) {
      s[1] = s[1] || '';
      s[1] += new Array(prec - s[1].length + 1)
        .join('0');
    }
    return s.join(dec);
  }
}
