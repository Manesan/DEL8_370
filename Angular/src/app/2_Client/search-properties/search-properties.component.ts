import { Component, OnInit } from '@angular/core';

import {ApiService} from '../../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';

import { NavBarComponent } from '../../nav-bar/nav-bar.component'
import { DomSanitizer } from '@angular/platform-browser';

import { NbThemeModule, NbLayoutModule, NbPopoverModule } from '@nebular/theme';

@Component({
  selector: 'app-search-properties',
  templateUrl: './search-properties.component.html',
  styleUrls: ['./search-properties.component.scss'],

})
export class SearchPropertiesComponent implements OnInit {


  public propertytypes: any;
  public markettypes: any;

  public area: string;
  public propertytype: number;
  public markettype: number;
  public pricefrom: any;
  public priceto: any;
  public bedroom: any;
  public bathroom: any;

  public featuredproperties: any;

  public propertyid1: any;
  public propertyid2: any;
  public propertyid3: any;

  public documenttype: any;
  public photo1: any;
  public photo2: any;
  public photo3: any;

  public photos: any[] = [];

    //Variable for auto-popup
    public areas = [];


  constructor(private service: ApiService, private http: HttpClient, private router: Router, private navbar: NavBarComponent, private sanitizer: DomSanitizer, private popover: NbPopoverModule) { }

  public token: any;

  async ngOnInit() {
    await this.service.Get('/propertystatuscheck')

    this.propertytypes = await this.service.Get('/propertytype') as any;
    this.markettypes = await this.service.Get('/markettype') as any;
    this.areas = await this.service.Get('/areas') as any;
    this.featuredproperties = await this.service.Get('/featured') as any;

    this.propertyid1 = this.featuredproperties[0]?.Picture?.LISTINGPICTUREID;
    this.propertyid2 = this.featuredproperties[1]?.Picture?.LISTINGPICTUREID;
    this.propertyid3 = this.featuredproperties[2]?.Picture?.LISTINGPICTUREID;

    this.documenttype = "ListingPicture";
    this.photo1 = this.featuredproperties[0]?.Picture?.LISTINGPICTUREIMAGE;
    this.photo2 = this.featuredproperties[1]?.Picture?.LISTINGPICTUREIMAGE;
    this.photo3 = this.featuredproperties[2]?.Picture?.LISTINGPICTUREIMAGE;

    this.token ="placeholder";

    this.photo1 = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.propertyid1) as any;
    this.photo2 = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.propertyid2) as any;
    this.photo3 = await this.service.Get('/downloadfile?token=' + this.token.token + '&documenttype=' + this.documenttype + '&id='+ this.propertyid3) as any;

    this.photo1 = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + this.photo1);
    this.photo2 = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + this.photo2);
    this.photo3 = this.sanitizer.bypassSecurityTrustResourceUrl('data:image;base64,' + this.photo3);

    this.photos[0] = this.photo1;
    this.photos[1] = this.photo2;
    this.photos[2] = this.photo3;

  }

  async search(){
    if(this.markettype != null && this.propertytype != null && this.area !== null){
      if(this.priceto == undefined){
         this.priceto = 999; //Not provided
      }
      if(this.pricefrom == undefined){
         this.pricefrom = 999; //Not provided
      }
      if(this.bedroom == undefined){
         this.bedroom = 999; //Not provided
      }
      if(this.bathroom == undefined){
         this.bathroom = 999; //Not provided
      }
    this.service.storeSearchProperty(this.markettype, this.propertytype, this.area, this.pricefrom, this.priceto, this.bedroom, this.bathroom)
    this.router.navigate(['browse'])
    }
  }

  async view(id){
    console.log(id);
    this.router.navigateByUrl(`viewproperty/${id}`);
    //console.log(id);
  }

}
