import { Component } from '@angular/core';
import {Router} from '@angular/router'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private router: Router) {
  }
  GompertzButton() {
    this.router.navigateByUrl('/Gompertz');
  }

  MakehamButton() {
    this.router.navigateByUrl('/Makeham');
  }
}
