<script lang="ts">
    import api from '../../api';
    import { getDatePart } from '../../helpers/dateHelpers';
    import type { Transaction } from '../../models/transaction/Transaction';
    import TransactionItem from './TransactionItem.svelte';

    export let walletId: number;
    export let reportPeriod: string;

    let transactions: Transaction[] = [];

    const fetchTransactions = async () => {
        let res = await api.getTransactions(walletId, reportPeriod);
        transactions = res.data;
    };

    export let needUpdate = false;

    $: if (needUpdate) {
        fetchTransactions().then(() => needUpdate = false);
    }

    $: groupedTransactions = (() => {
        let dates = [...new Set(transactions.map(t => t.timestamp.toLocaleDateString()))].map(ds => new Date(ds));

        return dates.map(d => ({
            date: d,
            transactions: transactions.filter(t => new Date(t.timestamp.toLocaleDateString()).getTime() == d.getTime())
        }));
    })();
</script>

<div class="container">
    {#each groupedTransactions as group}
    <div class="date">
        {#if group.date.getTime() === getDatePart(new Date()).getTime()}
        Today
        {:else}
        {group.date.toLocaleDateString()}
        {/if}
    </div>

    <div class="transactions">
        {#each group.transactions as transaction}
        <TransactionItem {transaction}/>
        {/each}
    </div>
    {/each}
</div>

<style>
    .date {
        text-align: center;
        color: var(--gray);
        margin: 0.7em 0;
    }

    .transactions {
        background: var(--bg-medium);
        border-radius: 0.5em;
    }
</style>
