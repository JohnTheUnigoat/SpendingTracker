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
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={currentWallet} on:change={walletChange}>
        {#each wallets as wallet}
        <option value={wallet}>{wallet.name}</option>
        {/each}
    </select>
    
    <!-- svelte-ignore a11y-no-onchange -->
    <select bind:value={currentReportPeriod} on:change={reportPeriodChange}>
        {#each reportPeriods as reportPeriodOption}
        <option value={reportPeriodOption}>
            {reportPeriodOption.text}
        </option>
        {/each}
    </select>
    
    <TransactionList {transactions} />
</div>

<style>
    .container {
        width: 100%;
        padding: 0.5em;
        background: #555;
        border-radius: 0.5em;
        color: #eee
    }
</style>
