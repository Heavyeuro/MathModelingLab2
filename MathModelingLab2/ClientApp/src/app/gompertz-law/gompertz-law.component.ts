import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {GompertzLawParams} from '../models/GompertzLawParams';
import {MortalityTableModel} from '../models/mortality-table-model';
import {FittingParameters} from '../models/fitting-parameters';

@Component({
  selector: 'gompertz-law',
  templateUrl: './gompertz-law.component.html'
})
export class GompertzLawComponent {
  public alpha = 0.001;
  public beta = 0.001;
  public ratePercents = 0.001;
  public plotUrl: string;
  public switcher = 1;

  public fittingParameters: FittingParameters[];
  public mortalityTableModels: MortalityTableModel[];
  public absoluteError: number;

  constructor(private http: HttpClient) {
  }

  GetMortalityTable() {
    var gompertzLawParams: GompertzLawParams = {Alpha: this.alpha, Beta: this.beta, RatePercents: this.ratePercents};
    this.http.post<any>('https://localhost:5001/api/GompertzComputing/GetMortalityTable', gompertzLawParams).subscribe(data => {
      this.mortalityTableModels = data;
      console.log(this.mortalityTableModels);
      this.switcher = 1;
    });
  }

  GetPlot() {
    this.plotUrl = 'https://localhost:5001/api/GompertzComputing/GetPlot?alpha=' + this.alpha + '&beta=' + this.beta + '&ratePercents=' + this.ratePercents;
    this.switcher = 2;
  }

  CompareWithRealData() {
    this.plotUrl = 'https://localhost:5001/api/GompertzComputing/CompareWithRealData?alpha=' + this.alpha + '&beta=' + this.beta + '&ratePercents=' + this.ratePercents;
    this.switcher = 3;
  }

  FindAbsoluteError() {
    this.switcher = 4;
    var gompertzLawParams: GompertzLawParams = {Alpha: this.alpha, Beta: this.beta, RatePercents: this.ratePercents};
    this.http.post<any>('https://localhost:5001/api/GompertzComputing/FindAbsoluteError', gompertzLawParams).subscribe(data => {
      this.absoluteError = data;
    })
  }

  FindAbsoluteErrorTable() {
    this.http.get<any>('https://localhost:5001/api/GompertzComputing/FindAbsoluteErrorTable').subscribe(data => {
      this.fittingParameters = data;
      console.log(this.fittingParameters);
      this.switcher = 5;
    });
  }

  FitParams() {
    this.http.get<any>('https://localhost:5001/api/GompertzComputing/FitParams').subscribe(data => {
      this.fittingParameters = data;
      console.log(this.fittingParameters);
      this.switcher = 6;
    });
  }
}


