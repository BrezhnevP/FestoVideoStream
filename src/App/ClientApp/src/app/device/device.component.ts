import { Component, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { IDevice } from '../interfaces/idevice.type';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-device',
  templateUrl: './device.component.html'
})
export class DeviceComponent {
  public device: IDevice;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    http.get<IDevice>(baseUrl + 'api/Devices/' + this.route.snapshot.params['id']).subscribe(result => {
      this.device = result;
    }, error => console.error(error));
  }
}
