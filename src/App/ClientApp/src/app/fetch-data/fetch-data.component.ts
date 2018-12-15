import { Component, Inject } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public devices: IDevice[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IDevice[]>(baseUrl + 'api/DevicesData/GetDevices').subscribe(result => {
      this.devices = result;
    }, error => console.error(error));
  }
}

interface IDevice {
  ipAddress: string;
  name: string;
  status: boolean;
}
