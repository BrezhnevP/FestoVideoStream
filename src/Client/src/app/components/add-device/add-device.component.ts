import { Component } from '@angular/core';
import { IDevice } from '../../models/idevice.type';
import { DeviceDataService } from '../../services/device.service';

@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html'
})
export class AddDeviceComponent {
  public device: IDevice;

  constructor(private dataService: DeviceDataService) {
  }

  addDevice() {
    this.dataService.addDevice(this.device).subscribe(() => console.log("Device is added"));
  }
}
