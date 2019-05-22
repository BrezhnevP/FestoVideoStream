import { Component } from '@angular/core';
import { IDevice } from '../../models/idevice.type';
import { DeviceDataService } from '../../services/device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.css']
})
export class DeviceListComponent {
  public devices: IDevice[];

  constructor(private dataService: DeviceDataService) {
    this.refreshList();
  }

  getDevices() {
    this.dataService.getDevices().subscribe(result => {
      this.devices = result;
    }, error => console.error(error));
  }

  deleteDevice(device: IDevice) {
    if (confirm('Are you sure?')) {
      this.dataService.deleteDevice(device).subscribe(() => {
        console.log('Device is deleted');
        this.refreshList();
      });
    }
  }

  refreshList() {
    this.devices = null;
    this.getDevices();
  }

  getpagenumb(numb: number) {
    return Math.ceil(numb);
  }

  sortbyname(call: string) {
    if (call !== 'nametop') {
      this.devices = this.devices.sort((a, b) => a.name < b.name ? -1 : a.name > b.name ? 1 : 0);
      call = 'nametop';
      return call;
    } else {
      this.devices = this.devices.sort((a, b) => a.name > b.name ? -1 : a.name < b.name ? 1 : 0);
      call = '';
      return call;
    }
  }
  sortbyip(call: string) {
    if (call !== 'iptop') {
      this.devices = this.devices.sort((a, b) => a.ipAddress < b.ipAddress ? -1 : a.ipAddress > b.ipAddress ? 1 : 0);
      call = 'iptop';
      return call;
    } else {
      this.devices = this.devices.sort((a, b) => a.ipAddress > b.ipAddress ? -1 : a.ipAddress < b.ipAddress ? 1 : 0);
      call = '';
      return call;
    }
  }
  sortbyonline(call: string) {
    if (call !== 'onlinetop') {
      this.devices = this.devices.sort((a, b) => a.streamStatus > b.streamStatus ? -1 : a.streamStatus < b.streamStatus ? 1 : 0);
      call = 'onlinetop';
      return call;
    } else {
      this.devices = this.devices.sort((a, b) => a.streamStatus < b.streamStatus ? -1 : a.streamStatus > b.streamStatus ? 1 : 0);
      call = '';
      return call;
    }
  }

  sortbyactivity(call: string) {
    if (call !== 'activetop') {
      this.devices = this.devices.sort((a, b) => a.deviceStatus > b.deviceStatus ? -1 : a.deviceStatus < b.deviceStatus ? 1 : 0);
      call = 'activetop';
      return call;
    } else {
      this.devices = this.devices.sort((a, b) => a.deviceStatus < b.deviceStatus ? -1 : a.deviceStatus > b.deviceStatus ? 1 : 0);
      call = '';
      return call;
    }
  }

  sortbyid(call: string) {
    if (call !== 'idtop') {
      this.devices = this.devices.sort((a, b) => a.id < b.id ? -1 : a.id > b.id ? 1 : 0);
      call = 'idtop';
      return call;
    } else {
      this.devices = this.devices.sort((a, b) => a.id > b.id ? -1 : a.id < b.id ? 1 : 0);
      call = '';
      return call;
    }
  }
}
