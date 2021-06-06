export interface AddTransactionRequest {
    walletId: number;
    amount: number;
    categoryId?: number;
    otherWalletId?: number;
}
