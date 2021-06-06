<script lang="ts">
import { component_subscribe } from "svelte/internal";

    import api from "../../api";
    import type { SummaryResponse } from "../../models/transaction/SummaryReponse";

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

            res.sort((a, b) => b.amount - a.amount);

            return res;
        }
    })();

    export let needUpdate = false;

    $: if (needUpdate) {
        fetchSummary().then(() => needUpdate = false);
    }
</script>

{#if summary && incomes && expenses}
<h3>Income:</h3>

{#each incomes as incomeSummary}
<div>{incomeSummary.isWallet ? '(w)' : ''}{incomeSummary.name}: {incomeSummary.amount}</div>
{/each}

<hr>

<div class="total">Total: {summary.totalIncome}</div>

<br>

<h3>Expense:</h3>

{#each expenses as expenseSummary}
<div>{expenseSummary.isWallet ? '(w)' : ''}{expenseSummary.name}: {expenseSummary.amount}</div>
{/each}

<hr>

<div class="total">Total: {summary.totalExpense}</div>
{/if}

