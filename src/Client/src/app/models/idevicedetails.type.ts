export interface IDeviceDetails {
  id: string;
  ipAddress: string;
  name: string;
  config: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamingStatus: boolean;
  lastStreamingDate: Date;
}
