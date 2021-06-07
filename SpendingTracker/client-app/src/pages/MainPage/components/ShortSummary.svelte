<script lang="ts">
    import api from '../../api';
    import type { ShortSummary } from '../../models/transaction/ShortSummary';

    export let walletId: number | null;
    export let reportPeriod: string | null;

    let summary: ShortSummary;

    const fetchSummary = async () => {
        if (walletId && reportPeriod) {
            let res = await api.getShortSummary(walletId, reportPeriod);
            summary = res.data;
        }
    };

    export let needUpdate = false;

    $: if (needUpdate) {
        fetchSummary().then(() => needUpdate = false);
    }
</script>

{#if summary}
<div class="container">
    <div class="income">{summary.income.toFixed(2)}</div>
    <div class="expense">{summary.expense.toFixed(2)}</div>
</div>
{/if}

<style>
    .container {
        width: 100%;
        display: flex;
        border-radius: 0.5em;
        overflow: hidden;
        border: 2px solid var(--bg-medium);
    }

    .container > * {
        width: 50%;
        padding: 0.2em;
        text-align: center;
    }

    .income {
        color: var(--green);
        background: var(--green-bg);
    }

    .income::before {
        content: "+";
    }

    .expense {
        color: var(--red);
        background: var(--red-bg);
    }
</style>
