<script lang="ts">
    import type { Transaction } from '../../../../models/transaction/Transaction';
    import { getAmPmTime } from '../../../../helpers/dateHelpers';

    export let transaction: Transaction;
</script>

<div class="container">
    <div class="amount" class:positive={transaction.amount > 0}>
        {transaction.amount}
    </div>

    <div class="label">
        {#if transaction.categoryId === null}
        <i class="fas fa-wallet"></i>
        {/if}
        {transaction.targetLabel}
    </div>

    <div class="time">
        {getAmPmTime(transaction.timestamp)}
    </div>
</div>

<style>
    .container {
        display: flex;
        width: 100%;
        padding: 1em;

        border-radius: 0.5em;
        margin: 0.4em 0;
        background: var(--bg-medium);
        color: var(--gray);
    }
/* 
    .container:not(:last-child) {
        border-bottom: 1px solid var(--bg-dark);
    } */

    .amount {
        width: 3.5em;
        text-align: start;
        color: #d33737;
    }

    .amount.positive {
        color: #5abd5f;
    }

    .amount.positive::before {
        content: "+";
    }

    .label {
        flex: 1 0 auto;
        margin: 0 1em;
    }
</style>