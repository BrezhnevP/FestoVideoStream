import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IDevice } from '../../models/idevice.type';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html'
})
export class DeviceListComponent {
  public devices: IDevice[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IDevice[]>(baseUrl + 'api/Devices/').subscribe(result => {
      this.devices = result;
    }, error => console.error(error));
  }
}
