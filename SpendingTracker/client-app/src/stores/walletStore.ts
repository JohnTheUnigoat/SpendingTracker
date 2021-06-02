import { readable } from "svelte/store";
import api from "../api";
import type { Wallet } from "../models/Wallet";

export const walletStore = readable<Wallet[]>([], (set) => {
    api.getWallets().then(res => set(res.data));
});

