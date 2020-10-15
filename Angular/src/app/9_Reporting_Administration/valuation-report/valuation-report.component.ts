import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
import 'jspdf-autotable';
declare var jsPDF: any;
import html2canvas from 'html2canvas';
import { NgxSpinnerService } from "ngx-spinner";



@Component({
  selector: 'app-valuation-report',
  templateUrl: './valuation-report.component.html',
  styleUrls: ['./valuation-report.component.scss']
})
export class ValuationReportComponent implements OnInit {
  public token: any; //holds user token
  public reportDate: any;
  public reportUser: any;
  public startDate: any;
  public endDate: any;
  public valuations: any;
  chart=[];

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private spinner: NgxSpinnerService) { }

  async ngOnInit()
  {
    
  this.spinner.show();
    this.service._startDate.subscribe(startDate => this.startDate = startDate);
    this.service._endDate.subscribe(endDate => this.endDate = endDate);
    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
 
    let reportDetails = await this.service.Get(`/valuationreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`) as any;
    this.valuations = reportDetails.valuations;
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser;
    this.valuations.forEach(e => {
      console.log(e)
      e.VALUATIONDATE = e.VALUATIONDATE.split("T")[0];
    });
   
    console.log(reportDetails)
    this.spinner.hide();
  }



  async DownloadReport(){
    
  this.spinner.show();
    window.scrollTo(0,0);
    var doc = new jsPDF("a4")



  
    var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
    var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
    //   console.log(pageWidth)

    //   let length = x["Suppliers"].length;
    //   let titles = x["Suppliers"].map(c => c.Name)
    //   this.suppliers = x["Suppliers"];

      let finalY = 100;
                //Header
    let data = document.getElementById("headerDiv"); 
    let headerDivWidth =  data.offsetWidth;
    let headerDivHeight =  data.offsetHeight;
    let hratio = headerDivHeight/headerDivWidth;
    let width = pageWidth*hratio
    let contentDataURL: any = await html2canvas(data).then(canvas => {
      let contentDataURL = canvas.toDataURL('image/png'); 
      return contentDataURL;
    });     
    console.log("hit", contentDataURL)
    doc.addImage(contentDataURL, 'PNG', 10, 12, pageWidth-20, pageHeight-280); 

    //Sub Header
    let data1 = document.getElementById("subHeading");  
    let contentDataURL1: any = await html2canvas(data1).then(canvas => {
      let contentDataURL1 = canvas.toDataURL('image/png'); 
      return contentDataURL1;
    });     

    //Date table
    console.log("hit", contentDataURL)
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  
    doc.autoTable({margin: { bottom: 10},startY: finalY + 20, html: '#dateTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY;

    //Main Table
    doc.autoTable({margin: { bottom: 10},startY: finalY + 20, html: '#table', useCss: true, head: [
        ['Inspections']]})
        finalY = doc.autoTable.previous.finalY;

        doc.text("***End of report***", (pageWidth / 2.5), finalY+20)

        doc.save('Valuation Report')
        this.spinner.hide();
  //     console.log(pageWidth)

  /*
      console.log(x);
      var fileType = i==1?"application/pdf":"application/msword";
      var fileName = i==1?"Report.pdf":"Report.doc";
      var newBlob = new Blob([x], { type: fileType }); //Only Chrome works if blob with mime-type explicitly set not made

      if (window.navigator && window.navigator.msSaveOrOpenBlob){
        window.navigator.msSaveOrOpenBlob(newBlob);
        return;
      }

      //other browsers
      const data = window.URL.createObjectURL(newBlob);

      var link = document.createElement('a');
      link.href=data;
      link.download = fileName;
      link.dispatchEvent(new MouseEvent('click', {bubbles: true, cancelable: true, view: window}));

      setTimeout(function(){
        window.URL.revokeObjectURL(data);
        link.remove();
      }, 100);
      */
   // })
    
  }

}
