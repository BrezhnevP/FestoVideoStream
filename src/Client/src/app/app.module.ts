import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './components/app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { DeviceVideoComponent } from './components/device-video/device-video.component';
import { AddDeviceComponent } from './components/add-device/add-device.component';
import { EditDeviceComponent } from './components/edit-device/edit-device.component';

import { DeviceDataService } from './services/device.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    DeviceListComponent,
    DeviceDetailsComponent,
    AddDeviceComponent,
    DeviceVideoComponent,
    EditDeviceComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,

    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'devices', component: DeviceListComponent },
      { path: 'device/:id', component: DeviceDetailsComponent },
      { path: 'devices/add', component: AddDeviceComponent, pathMatch: 'full' },
      { path: 'devices/edit/:id', component: EditDeviceComponent },
    ])
  ],
  providers: [DeviceDataService],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
})
export class AppModule { }
