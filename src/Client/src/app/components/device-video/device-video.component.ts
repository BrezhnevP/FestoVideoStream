import { Component, OnInit, ViewChild } from '@angular/core';
import { VgAPI } from 'videogular2/core';
import { VgDASH } from 'videogular2/src/streaming/vg-dash/vg-dash';
import { VgHLS } from 'videogular2/src/streaming/vg-hls/vg-hls';

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

  currentStream: IMediaStream;
  api: VgAPI;

  streams: IMediaStream =
    {
      type: 'dash',
      label: 'DASH: Live Streaming',
      source: 'http://192.168.0.29:8080/dash/myapp.mpd'
    };

  constructor() {
  }

  onPlayerReady(api: VgAPI) {
    this.api = api;
  }

  ngOnInit() {
    this.currentStream = this.streams;
  }
}
