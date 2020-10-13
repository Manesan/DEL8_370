import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
declare var jsPDF: any;
//import autoTable from 'jspdf-autotable';
//import {jsPDF} from 'jspdf';
import 'jspdf-autotable';
import html2canvas from 'html2canvas';
// declare const require: any;
// const jsPDF = require('jspdf');
// require('jspdf-autotable');
import {bgpLogoBase64} from '../../../assets/bgplogo';
import { format } from 'path';

@Component({
  selector: 'app-agent-report',
  templateUrl: './agent-report.component.html',
  styleUrls: ['./agent-report.component.scss']
})

export class AgentReportComponent implements OnInit {
  public token: any; //holds user token
  public reportDate: any;
  public reportUser: any;
  public reportAgentDate: any;
  public reportPropertyID: any;
  public reportPropertyAddress: any;
  public reportPropertyOwnerID: any;
  public reportPropertyOwnerName: any;
  public reportPropertyOwnerSurname: any;
  // public reportMarketTypeID: any;
  // public reportMarketTypeDescription: any;
  public startDate: any;
  public endDate: any;
  public agents: any;
  chart=[];
  public reportType = "Agent Report";
  //public BGPLogoBase64 = "../assets/bgplogo.JPG";
  public lineBreak = "\r\n";

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit(){
    //this.service._startDate.subscribe(startDate => this.startDate = startDate);
    //this.service._endDate.subscribe(endDate => this.endDate = endDate);
    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
    let reportDetails = await this.service.Get(`/agentreport?token=${this.token.token}`) as any;

    this.agents = reportDetails.Agents;
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser;

    this.reportPropertyAddress = reportDetails.Agents.PROPERTYADDRESS;
    //this.reportMarketTypeDescription = reportDetails.Agents.MARKETTYPEDESCRIPTION;
    this.reportPropertyOwnerName = reportDetails.Agents.PROPERTYOWNERNAME;
    this.reportPropertyOwnerSurname = reportDetails.Agents.PROPERTYOWNERSURNAME;
    //this.reportDefects = reportDetails.Inspections.Defects;
    //this.reportInspectionDocument = reportDetails.Inspections.INSPECTIONDOCUMENT;
    console.log(this.reportUser)
  }

  async DownloadReport(){
    
    window.scrollTo(0,0);
    var doc = new jsPDF("a4"),
    margins = {
      top: 40,
      bottom: 60,
      left: 40,
      width: 522
    };
    var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
    var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();

    let length = this.agents.length;
    let titles = this.agents.map(c => c.USERNAME);

    let finalY = 100;
    console.log(pageHeight, pageWidth)

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

    //Header
    let data1 = document.getElementById("subHeading");  
    let contentDataURL1: any = await html2canvas(data1).then(canvas => {
      let contentDataURL1 = canvas.toDataURL('image/png'); 
      return contentDataURL1;
    });     
    console.log("hit", contentDataURL)
    doc.addImage(contentDataURL1, 'PNG', -115, 50, pageWidth+230, pageHeight-250);  

    //doc.setFontSize(40);
    //doc.text("Agent Report", (pageWidth / 2) - 50, 15)
    //doc.setFontSize(3);

    // Open PDF document in new tab
    console.log( titles)
    /*for (let i=0; i<length; i++){*/
      //doc.text(titles[i]+" (Number of Properties: " + this.agents[i].Properties.length+")", (pageWidth / 2) - 20, finalY + 23);
    // doc.autoTable({startY: finalY + 25, html: '#date', useCss: true, head: [
    //   [' Dates']]})
    //   finalY = doc.autoTable.previous.finalY;
    doc.autoTable({margin: { bottom: 10},startY: finalY + 20, html: '#table', useCss: true, head: [
        ['Property Address']]})
        finalY = doc.autoTable.previous.finalY;
    /*}*/
    doc.text("***End of report***", (pageWidth / 2.5), finalY+20)
    doc.save('Agent Report')

        /*(doc as any).autoTable({
      head: this.agents,
      body: titles,
      theme: 'plain',
      didDrawCell: data => {
        console.log(data.column.index)
      }
    })*/
/*
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
    
  }

 /* toDataURL(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback((reader.result));
        }
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();
}

 getImageFromUrl = function(url, callback) {
  var img = new Image();

  img.onerror = function() {
      alert('Cannot load image: "'+url+'"');
  };
  img.onload = function() {
      callback(img);
  };
  img.src = url;
}*/

}
