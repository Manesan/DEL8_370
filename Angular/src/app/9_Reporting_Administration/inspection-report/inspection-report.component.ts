import { Component, OnInit, Input } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
import {Chart} from 'chart.js';
import 'jspdf-autotable';
declare var jsPDF: any;
import html2canvas from 'html2canvas';
//import jsPDF from 'jspdf';
//import * as jsPDF from 'jspdf';

@Component({
  selector: 'app-inspection-report',
  templateUrl: './inspection-report.component.html',
  styleUrls: ['./inspection-report.component.scss']
})
export class InspectionReportComponent implements OnInit {
  public token: any; //holds user token
    public reportDate: any;
    public reportUser: any;
    public reportInspectionDate: any;
    public reportPropertyAddress: any;
    public reportInspectionTypeDescription: any;
    public reportInspectorName: any;
    public reportInspectorSurname: any;
    public reportDefects: [];
    public reportInspectionDocument: any;
    public inspections: any;
    public takeonInspections = [];
    public outgoingInspections = [];
    public allInspections: any;
    public startDate: any;
    public endDate: any;
    chart=[];
    public reportType = "Inspection Report";
    public lineBreak = "\r\n";
  

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit()
   {
    this.service._startDate.subscribe(startDate => this.startDate = startDate);
     this.service._endDate.subscribe(endDate => this.endDate = endDate);

     console.log("hit",this.startDate, this.endDate)
     this.token ={"token" : localStorage.getItem("37y7ffheu73")}    
    let reportDetails = await this.service.Get(`/inspectionreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`) as any;
    //this.inspections = reportDetails.Inspections;
    this.takeonInspections = reportDetails.takeonInspections;
    this.takeonInspections.forEach(e => {
      e.INSPECTIONDATE = e.INSPECTIONDATE.split("T")[0];
    })
    this.outgoingInspections = reportDetails.outgoingInspections;
    this.outgoingInspections.forEach(e => {
      e.INSPECTIONDATE = e.INSPECTIONDATE.split("T")[0];
    })
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser;
    console.log("hit", this.takeonInspections)


    //Chart
      // let keys = reportDetails["takeonInspections"].map(c => c.Count)
      // let values = reportDetails["outgoingInspections"].map(c => c.Count);
      // console.log(keys, values)

      // this.inspections = reportDetails["allInspections"];
      // console.log(this.allInspections)

        this.chart = new Chart ('pie-chart', {
          type: 'pie',
          data: {
            labels: ["Take-On Inspections", "Outgoing Inspections"],
            datasets: [{
              label: 'Number of Inspections per Inspection Type',
              data: [this.takeonInspections.length, this.outgoingInspections.length],
              barPercentage: 0.5,
              backgroundColor: [
                "#C18D21",
                "#E89C31",
            ],
            }]
          },
          options: {
            title: {
              display: true,
              text: 'Number of Inspections per Inspection Type'
            }
          }
        })
      console.log(reportDetails)
  }

  async DownloadReport(){
    window.scrollTo(0,0);
    var doc = new jsPDF("a4")
    
  //   let reportDetails = await this.service.Get(`/inspectionreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`);
  //     var doc = new jsPDF();
      var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
      var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();

      
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

    
    console.log("hit", contentDataURL)
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  

    //Date table
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#dateTable', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY;

            //small table 1
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#smallTable1', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

    //Main Table 1
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#tableTakeOn', useCss: true, head: [
        ['Inspections']]})
        finalY = doc.autoTable.previous.finalY;

        
            //small table 2
    //console.log("hit", contentDataURL)
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#smallTable2', useCss: true, head: [
      ['Report Dates']]})
      finalY = doc.autoTable.previous.finalY; 

    //Main Table 2
    doc.autoTable({margin: { bottom: 10},startY: finalY + 10, html: '#tableOutGoing', useCss: true, head: [
        ['Inspections']]})
        finalY = doc.autoTable.previous.finalY;

      //Pie chart
      var newCanvas = <HTMLCanvasElement>document.querySelector('#pie-chart');
      doc.addPage();
      var newCanvasImg = newCanvas.toDataURL("image/png", 1.0);
      doc.addImage(newCanvasImg, 'PNG', 0, 15, 210, 100);     
      doc.text("***End of report***", (pageWidth / 2.5), 130)

        doc.save('Inspection Report')
  //     console.log(pageWidth)

  //     let length = reportDetails["allInspections"].length;
  //     let titles = reportDetails["allInspections"].map(c => c.Name)
  //     this.inspections = reportDetails["allInspections"];

  //     let finalY = 150;
  //     var newCanvas = <HTMLCanvasElement>document.querySelector('#pie-chart');
  //     var newCanvasImg = newCanvas.toDataURL("image/png", 1.0);

  //     doc.setFontSize(40);
  //     doc.text("Inspection Report", (pageWidth / 2) - 50, 15)
  //     doc.addImage(newCanvasImg, 'PNG', 20, 20, 165, 100);
  //     doc.setFontSize(11);
  //     for (let i=0; i<length; i++){
  //       doc.text(titles[i]+" (Number of Products: " + this.inspections[i].ProductCount+")", (pageWidth / 2) - 20, finalY + 23);
  //       doc.autoTable({startY: finalY + 25, html: '#table'+i, useCss: true, head: [
  //         ['Product Name']]})
  //         finalY = doc.autoTable.previous.finalY;
  //     }
  //     doc.save('Report.pdf');



  //     console.log(x);
  //     var fileType = i==1?"application/pdf":"application/msword";
  //     var fileName = i==1?"Report.pdf":"Report.doc";
  //     var newBlob = new Blob([x], { type: fileType }); //Only Chrome works if blob with mime-type explicitly set not made

  //     if (window.navigator && window.navigator.msSaveOrOpenBlob){
  //       window.navigator.msSaveOrOpenBlob(newBlob);
  //       return;
  //     }

  //     //other browsers
  //     const data = window.URL.createObjectURL(newBlob);

  //     var link = document.createElement('a');
  //     link.href=data;
  //     link.download = fileName;
  //     link.dispatchEvent(new MouseEvent('click', {bubbles: true, cancelable: true, view: window}));

  //     setTimeout(function(){
  //       window.URL.revokeObjectURL(data);
  //       link.remove();
  //     }, 100);
      
  //   })
    
  }

}
