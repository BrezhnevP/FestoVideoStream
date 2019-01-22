import { Component } from '@angular/core';
import { IDevice } from '../../models/idevice.type';
import { DeviceDataService } from '../../services/device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html'
})
export class DeviceListComponent {
  public devices: IDevice[];

  constructor(private dataService: DeviceDataService) {
    this.dataService.getDevices().subscribe(result => {
      this.devices = result;
    }, error => console.error(error));
  }

  deleteDevice(device: IDevice) {
    this.dataService.deleteDevice(device).subscribe(() => console.log('Device is deleted'));
  }
}
