import { Component, OnInit, ViewChild, Input, Inject } from '@angular/core';
import { VgAPI } from 'videogular2/core';
import { VgDASH } from 'videogular2/src/streaming/vg-dash/vg-dash';
import { VgHLS } from 'videogular2/src/streaming/vg-hls/vg-hls';
import { HttpClient } from '@angular/common/http';
import * as HLS from 'hls.js';


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

    var video = <HTMLVideoElement>document.getElementById('video');
    if(hls.isSupported()) {
      var hls = new HLS();
      hls.loadSource(this.currentStream);
      hls.attachMedia(video);
      hls.on(hls.Events.MANIFEST_PARSED,function() {
        video.play();
    });
  }
    else if (video.canPlayType('application/vnd.apple.mpegurl')) {
      video.src = 'currentStream';
      video.addEventListener('loadedmetadata',function() {
        video.play();
      });
    }
  }
}
