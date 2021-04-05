import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {MortalityTableModel} from '../models/mortality-table-model'
import {FittingParameters} from "../models/fitting-parameters";
import {MakehamLawParams} from "../models/MakehamLawParams";

@Component({
  selector: 'makeham-law',
  templateUrl: './makeham-law.component.html'
})
export class MakehamLawComponent {
  public alpha = 0.001;
  public a = 0.001;
  public b = 0.001;
  public plotUrl: string;
  public switcher = 1;

  public fittingParameters: FittingParameters[];
  public mortalityTableModels: MortalityTableModel[];
  public absoluteError: number;

  constructor(private http: HttpClient) {
  }

  GetMortalityTable() {
    var makehamLawParams: MakehamLawParams = {Alpha: this.alpha, A: this.a, B: this.b};
    this.http.post<MortalityTableModel>('https://localhost:5001/api/MakehamComputing/GetMortalityTable', makehamLawParams).subscribe(data => {
      this.mortalityTableModels = data;
      console.log(this.mortalityTableModels);
      this.switcher = 1;
    });
  }

  GetPlot() {
    this.plotUrl = 'https://localhost:5001/api/MakehamComputing/GetPlot?alpha=' + this.alpha + '&a=' + this.a + '&b=' + this.b;
    this.switcher = 2;
  }

  CompareWithRealData() {
    this.plotUrl = 'https://localhost:5001/api/MakehamComputing/CompareWithRealData?alpha=' + this.alpha + '&a=' + this.a + '&b=' + this.b;
    this.switcher = 3;
  }

  FindAbsoluteError() {
    var makehamLawParams: MakehamLawParams = {Alpha: this.alpha, A: this.a, B: this.b};
    this.http.post<any>('https://localhost:5001/api/MakehamComputing/FindAbsoluteError', makehamLawParams).subscribe(data => {
      this.absoluteError = data;
      this.switcher = 4;
    })
  }

  FindAbsoluteErrorTable() {
    this.http.get<any>('https://localhost:5001/api/MakehamComputing/FindAbsoluteErrorTable').subscribe(data => {
      this.fittingParameters = data;
      console.log(this.fittingParameters);
      this.switcher = 5;
    });
  }

  FitParams() {
    this.http.get<any>('https://localhost:5001/api/MakehamComputing/FitParams').subscribe(data => {
      this.fittingParameters = data;
      console.log(this.fittingParameters);
      this.switcher = 6;
    });
  }
}
