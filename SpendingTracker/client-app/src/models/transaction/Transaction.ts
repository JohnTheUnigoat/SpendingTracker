export interface Transaction {
    id: number;
    amount: number;
    targetLabel: string;
    categoryId: number | null;
    otherWalletId: number | null;
    timestamp: Date;
}