import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { IDevice } from '../interfaces/idevice.type';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html'
})
export class FetchDataComponent {
  private devices: IDevice[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    http.get<IDevice[]>(baseUrl + 'api/Devices').subscribe(result => {
      this.devices = result;
    }, error => console.error(error));

  }
  private goToDevice = function (id: string) {
    this.router.navigateByUrl(this.baseUrl + 'devices/:id');
  };
}
