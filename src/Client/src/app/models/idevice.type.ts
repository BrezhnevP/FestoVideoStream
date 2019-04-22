export interface IDevice {
  id: string;
  ipAddress: string;
  name: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamingStatus: boolean;
  lastStreamingDate: Date;
}
