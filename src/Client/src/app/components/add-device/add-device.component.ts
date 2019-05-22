import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceDataService } from '../../services/device.service';
import { IDeviceDetails } from '../../models/idevicedetails.type';

@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html'
})
export class AddDeviceComponent implements OnInit {

  addForm: FormGroup;

  constructor(private formBuilder: FormBuilder, private router: Router, private dataService: DeviceDataService) {
  }

  ngOnInit() {
    this.addForm = this.formBuilder.group({
      name: ['', Validators.required],
      ipAddress: ['', Validators.required],
      config: [''],
    });
  }

  onSubmit() {
    this.dataService.addDevice(this.addForm.value).subscribe(data => {
      this.router.navigate(['/devices']);
    });
  }
}
