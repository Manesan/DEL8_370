import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { NgxSpinnerService } from 'ngx-spinner';

declare var $: any; //needed to use jQuery in ts

@Component({
  selector: 'app-browse-properties',
  templateUrl: './browse-properties.component.html',
  styleUrls: ['./browse-properties.component.scss']
})
export class BrowsePropertiesComponent implements OnInit {
public markettype: any;
public propertytype: any;
public area: any;

public properties: any;

public address: any;
public suburb: any;
public city: any;
public propertyListingPicture: any;
public pictureid: any;
public documenttype: any;
public token: any;
public photos: any[] = [];
public results: boolean = false;
public pricefrom: any;
public priceto: any;
public bedroom: any;
public bathroom: any;

public counter: any;

  constructor(private service: ApiService, private http: HttpClient, private router: Router, private sanitizer: DomSanitizer, private spinner: NgxSpinnerService) { }

   async ngOnInit(){
    this.spinner.show();
    this.markettype = this.service.callSearchMarketType();
    this.propertytype = this.service.callSearchPropertyType();
    this.area = this.service.callSearchArea();
    this.pricefrom = this.service.callSearchPriceFrom();
    this.priceto = this.service.callSearchPriceTo();
    this.bedroom = this.service.callSearchBedroom();
    this.bathroom = this.service.callSearchBathroom();

    //console.log(this.pricefrom.__zone_symbol__value, this.priceto.__zone_symbol__value, this.bedroom.__zone_symbol__value, this.bathroom.__zone_symbol__value)

    this.properties = await this.service.Get('/searchproperties?markettype=' + this.markettype.__zone_symbol__value + '&propertytype='+ this.propertytype.__zone_symbol__value + '&area='+ this.area.__zone_symbol__value + '&pricefrom='+ this.pricefrom.__zone_symbol__value + '&priceto=' + this.priceto.__zone_symbol__value + '&bedroom='+ this.bedroom.__zone_symbol__value + '&bathroom='+ this.bathroom.__zone_symbol__value);
    this.counter = this.properties.length;

    if (this.properties != ""){
      this.results = true;
    }
    console.log(this.properties)

    this.token ={"token" : localStorage.getItem("37y7ffheu73")};

    this.documenttype = "ListingPicture";
    for(let x = 0; x < this.properties.length; x++){
      //console.log(this.properties[x]?.Picture?.LISTINGPICTUREIMAGE);
      this.photos[x] = this.properties[x]?.Picture?.LISTINGPICTUREIMAGE
      this.photos[x] = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.properties[x]?.Picture?.LISTINGPICTUREID) as any);

    }
    console.log(this.photos)
    this.spinner.hide();


  }


//   async getPicture(id){
//     //console.log(id);
//     this.documenttype = "ListingPicture";
//     this.propertyListingPicture =  await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ id) as any;
//     //console.log(this.propertyListingPicture)
//     return this.sanitizer.bypassSecurityTrustResourceUrl(
//       'data:image;base64,' + this.propertyListingPicture
//      );
// }

  async view(id){
    this.router.navigateByUrl(`viewproperty/${id}`);
    //console.log(id);
  }


}
