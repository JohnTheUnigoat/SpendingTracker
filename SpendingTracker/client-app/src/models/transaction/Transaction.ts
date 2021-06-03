export interface Transaction {
    id: number;
    amount: string;
    targetLabel: string;
    categoryId: number | null;
    otherWalletId: number | null;
    timestamp: Date;
}