import axios, { AxiosInstance, AxiosTransformer } from "axios";
import type { AuthResponse } from "./models/auth/AuthResponse";
import type { User } from "./models/auth/User";
import { ShortSummary } from "./models/transaction/ShortSummary";
import type { Transaction } from "./models/transaction/Transaction";
import type { Wallet } from "./models/wallet/Wallet";
import token from "./stores/tokenStore";

class Api {
    private accessToken: string | null = null;

    http: AxiosInstance;

    constructor() {
        token.subscribe(value => {
            this.accessToken = value;
        });

        this.http = axios.create({
            baseURL: '/api',
        });

        this.http.interceptors.request.use(config => {
            if(this.accessToken) {
                config.headers = {
                    ...config.headers,
                    'Authorization': `Bearer ${this.accessToken}`  
                }
            }

            return config;
        });
    }

    signIn(googleToken: string) {
        return this.http.post<AuthResponse>('/auth/google-sign-in', `"${googleToken}"`, {
            headers: {
                "Content-Type": "application/json"
            }
        });
    }

    getUser() {
        return this.http.get<User>('/auth/user');
    }

    getWallets() {
        return this.http.get<Wallet[]>('/wallets');
    }

    getTransactions(walletId: number, reportPeriod: string, from?: Date, to?: Date) {
        return this.http.get<Transaction[]>(`/wallets/${walletId}/transactions`, {
            params: {
                reportPeriod,
                from,
                to
            },
            transformResponse: [
                ...this.http.defaults.transformResponse as AxiosTransformer[],
                (transactions: {timestamp: string}[]) => {
                    return transactions.map(t => ({...t, timestamp: new Date(t.timestamp)}));
                },
            ]
        });
    }

    getShortSummary(walletId: number, reportPeriod: string, from?: Date, to?: Date){
        return this.http.get<ShortSummary>(`/wallets/${walletId}/transactions/summary_short`, {
            params: {
                reportPeriod,
                from,
                to
            }
        });
    }
}

const api = new Api();

export default api;