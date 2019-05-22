export interface IDeviceDetails {
  id: string;
  ipAddress: string;
  port: number;
  name: string;
  config: string;
  checkType: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamStatus: boolean;
  lastStreamStartDate: Date;
  lastStreamEndDate: Date;
}
