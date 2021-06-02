import axios, { AxiosInstance } from "axios";
import type { AuthResponse } from "./models/auth/AuthResponse";
import type { Wallet } from "./models/Wallet";
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

    getWallets() {
        return this.http.get<Wallet[]>('/wallets');
    }
}

const api = new Api();

export default api;