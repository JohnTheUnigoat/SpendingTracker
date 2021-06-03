<script lang="ts">
    import type { Transaction } from '../../../../models/transaction/Transaction';

    export let transaction: Transaction;
</script>

<div>
    <div>{transaction.timestamp.toLocaleDateString()}</div>
    {transaction.amount} {transaction.targetLabel}
</div>

<br>