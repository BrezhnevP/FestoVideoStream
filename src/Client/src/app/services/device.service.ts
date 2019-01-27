import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { IDevice } from '../models/idevice.type';
import { IDeviceDetails } from '../models/idevicedetails.type';

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
    return this.http.get<IDeviceDetails>(this.deviceUrl + id);
  }

  addDevice(device: IDeviceDetails) {
    return this.http.post<IDeviceDetails>(this.deviceUrl, device);
  }

  editDevice(device: IDeviceDetails) {
    return this.http.put<IDeviceDetails>(this.deviceUrl + device.id, device);
  }

  deleteDevice(device: IDevice) {
    return this.http.delete<IDevice>(this.deviceUrl + device.id);
  }
}
