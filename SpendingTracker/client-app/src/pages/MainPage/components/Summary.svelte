<script lang="ts">
    import api from "../../../api";
    import type { SummaryResponse } from "../../../models/transaction/SummaryReponse";

    export let walletId: number | null;
    export let reportPeriod: string | null;

    let summary: SummaryResponse | null = null;

    const fetchSummary = async () => {
        if (walletId && reportPeriod) {
            let res = await api.getSummary(walletId, reportPeriod);
            summary = res.data;
        }
    };

    $: incomes = (() => {
        if (summary) {
            let res = summary.incomeDetails.categories.map(cs => ({...cs, isWallet: false}));
            res = res.concat(summary.incomeDetails.wallets.map(ws => ({...ws, isWallet: true})));

            res.sort((a, b) => b.amount - a.amount);

            return res;
        }
    })();

    $: expenses = (() => {
        if (summary) {
            let res = summary.expenseDetails.categories.map(cs => ({...cs, isWallet: false}));
            res = res.concat(summary.expenseDetails.wallets.map(ws => ({...ws, isWallet: true})));

            res.sort((a, b) => a.amount - b.amount);

            return res;
        }
    })();

    $: total = (() => {
        if (summary) {
            return summary.totalIncome + summary.totalExpense
        } else {
            return null;
        }
    })();

    export let needUpdate = false;

    $: if (needUpdate) {
        fetchSummary().then(() => needUpdate = false);
    }
</script>

{#if summary && incomes && expenses && total !== null}
<div class="container">
    <section class="income">
        <div class="row total">
            <div class="label">Income</div>
            <div>{summary.totalIncome.toFixed(2)}</div>
        </div>

        {#each incomes as incomeSummary}
        <div class="row">
            <div>{incomeSummary.isWallet ? '(w)' : ''}{incomeSummary.name}</div>
            <div class="positive">{incomeSummary.amount.toFixed(2)}</div>
        </div>
        {/each}
    </section>

    <section class="expense">
        <div class="row total">
            <div class="label">Expense</div>
            <div>{summary.totalExpense.toFixed(2)}</div>
        </div>

        {#each expenses as expenseSummary}
        <div class="row">
            <div>{expenseSummary.isWallet ? '(w)' : ''}{expenseSummary.name}</div>
            <div class="negative">{expenseSummary.amount.toFixed(2)}</div>
        </div>
        {/each}
    </section>
</div>

<section class="total">
    <div class="label">Total</div>
    <div class:positive={total > 0} class:negative={total < 0}>{total.toFixed(2)}</div>
</section>
{/if}

<style>
    .container {
        display: flex;
    }

    section {
        width: 100%;
        padding: 1em;
        background: var(--bg-medium);
        border-radius: 0.5em;
        margin-bottom: 1em;
    }

    section.income {
        margin-right: 0.5em;
    }

    section.expense {
        margin-left: 0.5em;
    }

    .row {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 0.5em;
    }

    .row.total {
        font-size: 120%;
        font-weight: bolder;
        margin-bottom: 0.7em;
    }

    .positive {
        color: var(--green);
    }

    .positive::before {
        content: '+';
    }

    .negative {
        color: var(--red);
    }

    section.total {
        display: flex;
        justify-content: space-between;
        font-size: 120%;
        font-weight: bolder;
    }

    @media screen and (max-width: 700px) {
        .container {
            flex-wrap: wrap;
        }

        section.income, section.expense {
            margin-left: 0;
            margin-right: 0;
        }
    }
</style>
