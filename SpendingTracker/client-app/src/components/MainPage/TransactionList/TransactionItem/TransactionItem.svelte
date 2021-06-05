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
        align-items: center;
        width: 100%;
        padding: 1em;
        color: var(--gray);
    }

    .container:not(:last-child) {
        border-bottom: 3px solid var(--bg-light);
    }

    .amount {
        width: 4.8em;
        text-align: start;
        color: var(--red);
        padding: 0.2em;
        background: var(--red-bg);
        border: 2px solid var(--red-border);
        border-radius: 0.3em;
        text-align: center;
    }

    .amount.positive {
        color: var(--green);
        background: var(--green-bg);
        border-color: var(--green-border);
    }

    .amount.positive::before {
        content: "+";
    }

    .label {
        flex: 1 0 auto;
        margin: 0 1em;
        color: var(--white);
    }
</style>