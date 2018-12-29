import { Component, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { IDevice } from '../../models/idevice.type';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public device: IDevice;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IDevice>(baseUrl + 'api/DevicesData/GetDevice').subscribe(result => {
      this.device = result;
    }, error => console.error(error));
  }
}
