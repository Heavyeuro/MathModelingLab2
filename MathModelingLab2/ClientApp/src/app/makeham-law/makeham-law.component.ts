import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {} from '../models/mortality-table-model'

@Component({
  selector: 'makeham-law',
  templateUrl: './makeham-law.component.html'
})
export class MakehamLawComponent {
  public forecasts: WeatherForecast[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
