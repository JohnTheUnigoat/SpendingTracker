export interface Wallet {
    id: number;
    ownerEmail: string;
    sharedEmails: string[];
    name: string;
    defaultReportPeriod: string;
}