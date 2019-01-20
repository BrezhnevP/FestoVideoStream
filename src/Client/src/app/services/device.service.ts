import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { IDevice } from '../models/idevice.type';

@Injectable()
export class DeviceDataService {

  deviceUrl: string;

  constructor(private http: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.deviceUrl = apiUrl + '/devices/';
  }

  getDevices() {
    return this.http.get<IDevice[]>(this.deviceUrl);
  }

  getDevice(id: string) {
    return this.http.get<IDevice>(this.deviceUrl + id);
  }

  addDevice(device: IDevice) {
    return this.http.post<IDevice>(this.deviceUrl, device);
  }

  editDevice(device: IDevice) {
    return this.http.put<IDevice>(this.deviceUrl + device.id, device);
  }

  deleteDevice(device: IDevice) {
    return this.http.delete<IDevice>(this.deviceUrl + device.id);
  }
}
