import {Component, Inject} from '@angular/core';
import {Router} from '@angular/router'
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public url: string;

  constructor(private router: Router, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
  }

  GompertzButton() {
    this.router.navigateByUrl('/Gompertz');
  }

  MakehamButton() {
    this.router.navigateByUrl('/Makeham');
  }
}
