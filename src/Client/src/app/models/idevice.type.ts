export interface IDevice {
  id: string;
  ipAddress: string;
  name: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamStatus: boolean;
  lastStreamStartDate: Date;
  lastStreamEndDate: Date;
}
