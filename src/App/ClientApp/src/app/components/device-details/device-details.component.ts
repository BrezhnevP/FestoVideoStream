import { Component, Inject } from '@angular/core';
import { DeviceDataService } from '../../services/device.service';
import { IDevice } from '../../models/idevice.type';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-device-details',
  templateUrl: './device-details.component.html'
})
export class DeviceDetailsComponent {
  public device: IDevice;

  constructor(private dataService: DeviceDataService, private route: ActivatedRoute) {
    this.dataService.getDevice(this.route.snapshot.params["id"]).subscribe(result => {
      this.device = result;
    }, error => console.error(error))
  }
}
