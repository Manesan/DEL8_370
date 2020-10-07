import { Component, OnInit } from '@angular/core';
import { DefectComponent } from '../6_Property_Administration/defect/defect.component';
//added imports:
import {ApiService} from '../../environments/api.service';
import {HttpClient}  from '@angular/common/http';
import { Router } from '@angular/router';
import {Chart} from 'chart.js';

@Component({
  selector: 'app-admin-portal',
  templateUrl: './admin-portal.component.html',
  styleUrls: ['./admin-portal.component.scss']
})
export class AdminPortalComponent implements OnInit {
  public token: any; //holds user token
  chart=[];
  chart2=[];

  constructor(private service: ApiService, private http: HttpClient, private router: Router,) { }

  async ngOnInit(){
    if(!localStorage.getItem('8d9s8fhvh9f')){
      this.router.navigate(['']);
    }
    this.token ={"token" : localStorage.getItem("37y7ffheu73")}
    let graphsDetails = await this.service.Get(`/adminportalgraph?token=${this.token.token}`) as any;

    this.chart = new Chart ('pie-chart', {
      type: 'pie',
      data: {
        labels: ["Sales", "Rentals"],
        datasets: [{
          label: 'Current Sales and Rentals Overview',
          data: [graphsDetails.PropertySalesCount, graphsDetails.PropertyRentalsCount],
          barPercentage: 0.5,
          backgroundColor: [
            '#C18D21',
            '#E89C31',
        ],
        }]
      },
      options: {
        title: {
          display: true,
          text: 'Current Sales and Rentals Overview'
        }
      }
      });

      let popularArea1 = [graphsDetails.PopularLocations[0]?.SUBURBNAME];
      let popularArea2 = [graphsDetails.PopularLocations[1]?.SUBURBNAME];
      let popularArea3 = [graphsDetails.PopularLocations[2]?.SUBURBNAME];
      let popularArea4 = [graphsDetails.PopularLocations[3]?.SUBURBNAME];
      let popularArea5 = [graphsDetails.PopularLocations[4]?.SUBURBNAME];
      let keys = [popularArea1, popularArea2, popularArea3, popularArea4, popularArea5];

      let count1 = graphsDetails.PopularLocations[0]?.Count;
      let count2 = graphsDetails.PopularLocations[1]?.Count;
      let count3 = graphsDetails.PopularLocations[2]?.Count;
      let count4 = graphsDetails.PopularLocations[3]?.Count;
      let count5 = graphsDetails.PopularLocations[4]?.Count;
      let values = [count1, count2, count3, count4, count5];

      console.log(graphsDetails.PopularLocations[0]?.Count)

      this.chart2 = new Chart ('canvas', {
        type: 'bar',
        data: {
          labels: keys,
          datasets: [{
            label: "Popular Areas (Top 5)",
            data: values,
            backgroundColor:[
              '#E89C31',
              '#BCBFC6',
              '#083248',
              '#0B2838',
              '#031B28',
            ]

          }]
        },
        options: {
          scales:{
            xAxes: [{
              //stacked: true
            }],
            yAxes: [{
              //stacked: true,
              ticks: {
                min: 0
              }
            }],
          }
        }
      })
      console.log(graphsDetails);
  }


  callDefect(){
    this.router.navigate(['/defect']);
  }

}
