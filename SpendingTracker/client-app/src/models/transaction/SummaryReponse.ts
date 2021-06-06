interface CategoryWalletSummary {
    id: number;
    name: string;
    amount: number;
} 

interface OneWaySummary {
    categories: CategoryWalletSummary[];
    wallets: CategoryWalletSummary[];
}

export interface SummaryResponse {
    totalIncome: number;
    totalExpense: number;
    incomeDetails: OneWaySummary;
    expenseDetails: OneWaySummary;
}