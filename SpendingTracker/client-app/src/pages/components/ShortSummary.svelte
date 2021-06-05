<script lang="ts">
    import api from '../../api';
    import type { ShortSummary } from '../../models/transaction/ShortSummary';

    export let walletId: number;
    export let reportPeriod: string;

    let summary: ShortSummary;

    const fetchSummary = async () => {
        let res = await api.getShortSummary(walletId, reportPeriod);
        summary = res.data;
    };

    $: if (walletId && reportPeriod) {
        fetchSummary();
    }

</script>

{#if summary}
<div class="container">
    <div class="income">{summary.income}</div>
    <div class="expense">{summary.expense}</div>
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
