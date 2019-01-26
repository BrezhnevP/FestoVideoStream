import { Component, Inject } from '@angular/core';
import { DeviceDataService } from '../../services/device.service';
import { IDeviceDetails } from '../../models/idevicedetails.type';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-device-details',
  templateUrl: './device-details.component.html'
})
export class DeviceDetailsComponent {
  public device: IDeviceDetails;

  constructor(private dataService: DeviceDataService, private route: ActivatedRoute) {
    this.dataService.getDevice(this.route.snapshot.params['id']).subscribe(result => {
      this.device = result;
    }, error => console.error(error));
  }
}
