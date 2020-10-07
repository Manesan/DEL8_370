import { Component, OnInit } from '@angular/core';

//added imports:
import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';


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

  constructor(private service: ApiService, private http: HttpClient, private router: Router) { }

  async ngOnInit()
  {
    this.service._startDate.subscribe(startDate => this.startDate = startDate);
    this.service._endDate.subscribe(endDate => this.endDate = endDate);
    console.log("hit",this.startDate, this.endDate)
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}  
   
    let reportDetails = await this.service.Get(`/valuationreport?token=${this.token.token}&startdate=${this.startDate}&enddate=${this.endDate}`) as any;
    this.valuations = reportDetails.valuations;
    this.reportDate = reportDetails.ReportDate.split("T")[0];
    this.reportUser = reportDetails.CurrentUser;
   
    console.log(reportDetails)
  }



  DownloadReport(){
    // this.reporting.getReportData(this.selection).subscribe( (x) => {
    //   console.log(x)
    //   var doc = new jsPDF();
    //   var pageHeight = doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
    //   var pageWidth = doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
    //   console.log(pageWidth)

    //   let length = x["Suppliers"].length;
    //   let titles = x["Suppliers"].map(c => c.Name)
    //   this.suppliers = x["Suppliers"];

    //   let finalY = 150;
    //   var newCanvas = <HTMLCanvasElement>document.querySelector('#canvas');
    //   var newCanvasImg = newCanvas.toDataURL("image/png", 1.0);

    //   doc.setFontSize(40);
    //   doc.text("Product Report", (pageWidth / 2) - 50, 15)
    //   doc.addImage(newCanvasImg, 'PNG', 20, 20, 165, 100);
    //   doc.setFontSize(11);
    //   for (let i=0; i<length; i++){
    //     doc.text(titles[i]+" (Number of Products: " + this.suppliers[i].ProductCount+")", (pageWidth / 2) - 20, finalY + 23);
    //     doc.autoTable({startY: finalY + 25, html: '#table'+i, useCss: true, head: [
    //       ['Product Name']]})
    //       finalY = doc.autoTable.previous.finalY;
    //   }
    //   doc.save('Report.pdf');


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
