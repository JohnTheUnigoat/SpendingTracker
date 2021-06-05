<script lang="ts">
    import { onMount } from 'svelte';
    import api from '../../api';
    import type { Transaction } from '../../models/transaction/Transaction';
    import type { ReportPeriod } from '../../models/wallet/ReportPeriod';
    import { reportPeriods } from '../../models/wallet/ReportPeriod';
    import { getReportPeriod } from '../../models/wallet/ReportPeriod';
    import type { Wallet } from '../../models/wallet/Wallet';
    import { TransactionList } from './TransactionList';

    let wallets: Wallet[] = [];
    let currentWallet: Wallet;
    let currentReportPeriod: ReportPeriod;
    let transactions: Transaction[] = [];

    let fetchTransactions = async () => {
        let res = await api.getTransactions(currentWallet.id, currentReportPeriod.code);
        currentWallet.defaultReportPeriod = currentReportPeriod.code;
        transactions = res.data;
    };

    let walletChange = () => {
        currentReportPeriod = getReportPeriod(currentWallet.defaultReportPeriod);
        fetchTransactions();
    };

    let reportPeriodChange = () => {
        currentWallet.defaultReportPeriod = currentReportPeriod.code;
        fetchTransactions();
        console.log(wallets);
    };

    onMount(async () => {
        let res = await api.getWallets();
        wallets = res.data;

        currentWallet = wallets[0];
        currentReportPeriod = getReportPeriod(wallets[0].defaultReportPeriod) ?? currentReportPeriod;

        fetchTransactions();
    });
</script>

<div class="container">
    <div class="select-container">
        <div class="select">
            <i class="fas fa-wallet"></i>
            <!-- svelte-ignore a11y-no-onchange -->
            <select bind:value={currentWallet} on:change={walletChange}>
                {#each wallets as wallet}
                <option value={wallet}>{wallet.name}</option>
                {/each}
            </select>
        </div>
        
        <div class="select">
            <i class="fas fa-calendar-alt"></i>
            <!-- svelte-ignore a11y-no-onchange -->
            <select bind:value={currentReportPeriod} on:change={reportPeriodChange}>
                {#each reportPeriods as reportPeriodOption}
                <option value={reportPeriodOption}>
                    {reportPeriodOption.text}
                </option>
                {/each}
            </select>
        </div>
    </div>
    
    <TransactionList {transactions} />
</div>

<style>
    .container {
        width: 100%;
        padding: 0.5em;
        background: var(--bg-light);
        border-radius: 0.5em;
        color: var(--white);
    }

    .select-container {
        display: flex;
        margin-bottom: 1.5em;
    }

    .select {
        display: flex;
        align-items: center;
        width: 50%;
        max-width: 17em;
    }

    .select:not(:last-child) {
        margin-right: 0.4em;
    }

    .select .fas {
        font-size: 150%;
        width: 1.2em;
        margin-right: 0.15em;
        text-align: center;
        color: var(--highlight);
    }

    .select select {
        padding: 0.5em;
        color: var(--white);
        background: var(--bg-medium);
        border: none;
        flex: 1 0 auto;
        outline: none;
    }
</style>
