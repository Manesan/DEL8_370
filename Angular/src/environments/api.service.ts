import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { NavBarComponent } from '../app/nav-bar/nav-bar.component'
import { Observable, BehaviorSubject, observable  } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

declare var $: any; //needed to use jQuery in ts

@Injectable({
  providedIn: 'root'
})
export class ApiService {
//url = 'https://localhost:44373/api';
url = 'https://blackgoldpropertiesapi.azurewebsites.net/api';
  constructor(private http: HttpClient,  private router: Router, private toastr: ToastrService) { }
  headers = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }


  //Read
  public async Get(endpoint: string){
    let response = await this.http.get(`${this.url}${endpoint}`).toPromise()
    .catch(e => {
      this.toastr.warning("Oops, something went wrong!");
      if (e.status === 401 || e.status === 400) {
        localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
      }
      else if (e.status === 404){
        window.location.reload();
      }
  });
  console.log(response)
    return response;
  }

  //Add
  public async Post(endpoint: string, body?: any) {
    if (body) {
      //console.log("hit");
      let response = await this.http.post(`${this.url}${endpoint}`, body, this.headers).toPromise()
      .catch(e => {
        if (e.status === 401 || e.status === 400) {
          this.toastr.warning("Oops, something went wrong!");
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          this.toastr.warning("Oops, something went wrong!");
          window.location.reload();
        }
        else if (e.status === 500){
          this.toastr.warning("Employee already exists!");
        }
    });
      return response;
    }
    else {
      let response = await this.http.post(`${this.url}${endpoint}`, null).toPromise()
      .catch(e => {
        this.toastr.warning("Oops, something went wrong!");
        if (e.status === 401 || e.status === 400) {
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          window.location.reload();
        }
    });
      return response;
    }
  }

  //Update
  public async Patch(endpoint: string, body?: any) {
    if (body) {
      let response = await this.http.patch(`${this.url}${endpoint}`, body, this.headers).toPromise()
      .catch(e => {
        this.toastr.warning("Oops, something went wrong!");
        if (e.status === 401 || e.status === 400) {
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          window.location.reload();
        }
    });
      return response;
    }
    else {
      let response = await this.http.patch(`${this.url}${endpoint}`, null).toPromise()
      .catch(e => {
        this.toastr.warning("Oops, something went wrong!");
        if (e.status === 401 || e.status === 400) {
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          window.location.reload();
        }
    });
      return response;
    }
  }

  //Delete
  public async Delete(endpoint: string) {
    let response = await this.http.delete(`${this.url}${endpoint}`).toPromise()
    .catch(e => {
      if (e.status === 401 || e.status === 400) {
        this.toastr.warning("Oops, something went wrong!");
        localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
      }
      else if (e.status === 404){
        this.toastr.warning("Oops, something went wrong!");
        window.location.reload();
      }
      else if (e.status === 409){
        return e.status;
      }
      else{
      this.toastr.warning("Oops, something went wrong!");
      }
  });
    return response;
  }

  //Login
  public async Login(endpoint: string, body?: any) {
    if (body) {
      let response = await this.http.post(`${this.url}${endpoint}`, body, this.headers).toPromise()
      .catch(e => {
        this.toastr.warning("Oops, something went wrong!");
        if (e.status === 401 || e.status === 400) {
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          window.location.reload();
        }
    });
      return response;
    }
    else {
      let response = await this.http.post(`${this.url}${endpoint}`, null).toPromise()
      .catch(e => {
        this.toastr.warning("Oops, something went wrong!");
        if (e.status === 401 || e.status === 400) {
          localStorage.setItem("37y7ffheu73", "");
          localStorage.setItem("8d9s8fhvh9f", "");
          this.router.navigate(['/login']);
        }
        else if (e.status === 404){
          window.location.reload();
        }
    });
      return response;
    }
  }

  //Additional Read
  public async Put(endpoint: string, body?: any) {
    if (body) {
      let response = await this.http.put(`${this.url}${endpoint}`, body, this.headers).toPromise();
      return response;
    }
    else {
      let response = await this.http.put(`${this.url}${endpoint}`, null).toPromise();
      console.log(response)
      return response;
    }
  }

   //Check is logged in
   loggedIn(){
    return !!localStorage.getItem('37y7ffheu73')
  }


  //Check is admin
  isAdmin(){
    return !!localStorage.getItem('8d9s8fhvh9f')
  }




  //Store information
  public x: any;
  public y: any;
  public async storeEmail(email: string){
    this.x = email;
    //console.log(this.x);
  }
  public async callEmail(){
    //console.log(this.x);
    return this.x;
  }
  public async storeOTP(otp: number){
    this.y = otp;
    //console.log(this.y);
  }
  public async callOTP(){
  //console.log(this.y);
  return this.y;
  }


  //Store search property information
  public marketType: any;
  public propertyType: any;
  public Area: any;
  public pricefrom: any;
  public priceto: any;
  public bedroom: any;
  public bathroom: any;


  public async storeSearchProperty(markettype: number, propertytype: number, area: string, pricefrom: number, priceto: number, bedroom: number, bathroom: number ) {
    this.marketType = markettype;
    this.propertyType = propertytype
    this.Area = area;
    this.pricefrom = pricefrom;
    this.priceto = priceto;
    this.bedroom = bedroom;
    this.bathroom = bathroom;
  }

  public async callSearchArea(){
    return (this.Area);
  }

  public async callSearchMarketType(){
    return (this.marketType);
  }

  public async callSearchPropertyType(){
    return (this.propertyType);
  }
  public async callSearchPriceFrom(){
    return (this.pricefrom);
  }

  public async callSearchPriceTo(){
    return (this.priceto);
  }

  public async callSearchBedroom(){
    return (this.bedroom);
  }
  public async callSearchBathroom(){
    return (this.bathroom);
  }


  private startDate = new BehaviorSubject("01/01/2000");
  _startDate = this.startDate.asObservable();
  private endDate = new BehaviorSubject("01/02/2000");
  _endDate = this.endDate.asObservable();

  //Inspection Report
  GenerateReport(startDate, endDate){
    this.startDate.next(startDate);
    this.endDate.next(endDate);
  }


}
