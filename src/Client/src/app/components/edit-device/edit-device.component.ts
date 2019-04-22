import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceDataService } from '../../services/device.service';
import { IDeviceDetails } from '../../models/idevicedetails.type';
import { tryParse } from 'selenium-webdriver/http';

@Component({
  selector: 'app-edit-device',
  templateUrl: './edit-device.component.html'
})
export class EditDeviceComponent implements OnInit {

  private device: IDeviceDetails;
  editForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder, private router: Router,
    private dataService: DeviceDataService, private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.editForm = this.formBuilder.group({
      id: [''],
      name: ['', Validators.required],
      ipAddress: ['', Validators.required],
      config: [''],
      stasus: ['']
    });
    this.dataService.getDevice(this.route.snapshot.params['id']).subscribe(result => {
      this.editForm.setValue({
        id: result.id,
        name: result.name,
        ipAddress: result.ipAddress,
        config: result.config,
        stasus: result.deviceStatus
      });
    }, error => console.error(error));

  }

  onSubmit() {
    this.dataService.editDevice(this.editForm.value).subscribe(data => {
      this.router.navigate(['devices']);
    });
  }
}
