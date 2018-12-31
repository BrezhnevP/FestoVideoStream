import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceDetailsComponent } from './components/device-details/device-details.component';
import { AddDeviceComponent } from './components/add-device/add-device.component';

import { DeviceDataService } from './services/device.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    DeviceListComponent,
    DeviceDetailsComponent,
    AddDeviceComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'devices', component: DeviceListComponent },
      { path: 'devices/:id', component: DeviceDetailsComponent },
      { path: 'devices/add', component: AddDeviceComponent }
    ])
  ],
  providers: [DeviceDataService],
  bootstrap: [AppComponent]
})
export class AppModule { }
