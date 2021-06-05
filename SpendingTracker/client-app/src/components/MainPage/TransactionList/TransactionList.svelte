<script lang="ts">
    import { getDatePart } from "../../../helpers/dateHelpers";
    import type { Transaction } from "../../../models/transaction/Transaction";
    import { TransactionItem } from './TransactionItem';
    export let transactions: Transaction[];

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

    <br>
    {/each}
</div>

<style>
    .container {
        padding: 0 2em;
    }

    @media screen and (max-width: 500px) {
        .container {
            padding: 0 0.2em;
        }
    }

    .date {
        text-align: center;
        color: var(--white);
        margin-bottom: 0.7em;
    }
</style>
