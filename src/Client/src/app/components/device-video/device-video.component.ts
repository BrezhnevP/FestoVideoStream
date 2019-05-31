import { Component, Input, Inject, AfterViewInit, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

declare var videojs: any;

@Component({
  selector: 'app-device-video',
  templateUrl: './device-video.component.html',
  styleUrls: ['./device-video.component.css']
})
export class DeviceVideoComponent implements OnInit, AfterViewInit, OnDestroy {

  @Input() deviceId: string;
  streamUrl: string;
  source: string;
  private player: any;

  constructor(private http: HttpClient, @Inject('API_URL') apiUrl: string) {
    this.streamUrl = apiUrl + '/stream/';
  }

  ngOnInit(): void {
    this.http.get(this.streamUrl + this.deviceId + '/hls', { responseType: 'text' }).subscribe(result => {
      this.source = result;
    }, error => console.error(error));
    if (!this.source) {
      this.source = 'http://ctpo.sensorika.info:8080/hls/11111111-1111-1111-1111-111111111111.m3u8';
    }
  }

  ngAfterViewInit(): void {
    this.player = videojs('my_video_1');
    this.player.src({
      'type': 'application/x-mpegURL',
      'src': this.source
    });
    this.player.reloadSourceOnError();
  }

  ngOnDestroy() {
    if (this.player) {
      this.player.dispose();
    }
  }
}
