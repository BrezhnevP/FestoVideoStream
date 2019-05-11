export interface IDeviceDetails {
  id: string;
  ipAddress: string;
  name: string;
  config: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamStatus: boolean;
  lastStreamStartDate: Date;
  lastStreamEndDate: Date;
}
