export interface IDevice {
  id: string;
  ipAddress: string;
  port: number;
  name: string;
  deviceStatus: boolean;
  lastActivityDate: Date;
  streamStatus: boolean;
  lastStreamStartDate: Date;
  lastStreamEndDate: Date;
}
