<script lang="ts">
    import type { Transaction } from "../../../models/transaction/Transaction";
    import { TransactionItem } from './TransactionItem';
    export let transactions: Transaction[];
</script>

{#each transactions as transaction}
<TransactionItem {transaction}/>
{/each}
