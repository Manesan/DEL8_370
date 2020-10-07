import { Component } from '@angular/core';
import { ApiService } from '../environments/api.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'black-gold-properties';

  constructor(private service: ApiService){}

}

