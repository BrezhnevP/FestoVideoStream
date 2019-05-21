import { Component, OnInit, ViewChild, Input, Inject } from '@angular/core';
import { VgAPI } from 'videogular2/core';
import { VgDASH } from 'videogular2/src/streaming/vg-dash/vg-dash';
import { VgHLS } from 'videogular2/src/streaming/vg-hls/vg-hls';
import { HttpClient } from '@angular/common/http';

export interface IMediaStream {
  type: 'dash';
  source: string;
  label: string;
}

@Component({
  selector: 'app-device-video',
  templateUrl: './device-video.component.html',
  styleUrls: ['./device-video.component.css']
})
export class DeviceVideoComponent implements OnInit {
  @ViewChild(VgDASH) vgDash: VgDASH;
  @ViewChild(VgHLS) vgHls: VgHLS;

  @Input() deviceId: string;
  streamUrl: string;
  currentStream: string;
  api: VgAPI;

  constructor(private http: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.streamUrl = apiUrl + '/stream/';
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;
  }

  ngOnInit() {
    this.http.get(this.streamUrl + this.deviceId + '/dash', { responseType: 'text' }).subscribe(result => {
      this.currentStream = result;
    }, error => console.error(error));
  }
}
