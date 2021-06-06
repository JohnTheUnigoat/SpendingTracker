import { writable } from 'svelte/store'
import type { Wallet } from '../models/wallet/Wallet'

const wallets = writable<Wallet[]>([]);

export default wallets;