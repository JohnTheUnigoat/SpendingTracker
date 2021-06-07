<script lang="ts">
    import api from "../../../api";
    import Buttons from "../../components/Buttons.svelte";
    import TwoStateSelector from "../../components/TwoStateSelector.svelte";
    import type { AddTransactionRequest } from "../../../models/api/AddTransaction";
    import CategoryTransactionForm from "./CategoryTransactionForm.svelte";
    import WalletTransactionForm from "./WalletTransactionForm.svelte";

    let readyToSave: boolean;
    let payload: AddTransactionRequest;

    export let onComplete: (success: boolean) => any;

    let isWallet = false;

    const addTransaction = async () => {
        try {
            console.log(payload);
            await api.addTransaction(payload);
            onComplete(true);
        } catch {
            console.log('Something went wrong when trying to save transaction');
        }
    }
</script>

<div class="container">
    <div class="category-wallet-select">
        <TwoStateSelector labels={['Category', 'Wallet']} bind:value={isWallet} />
    </div>

    <hr>

    {#if isWallet}
    <WalletTransactionForm bind:readyToSave={readyToSave} bind:payload={payload} />
    {:else}
    <CategoryTransactionForm bind:readyToSave={readyToSave} bind:payload={payload} />
    {/if}

    <div class="form"></div>

    <div class="buttons">
        <Buttons
            primary={{text: 'Add', action: addTransaction, disabled: readyToSave === false}}
            secondary={{text: 'Cancel', action: () => onComplete(false)}}
        />
    </div>
</div>

<style>
    .container {
        font-size: 1rem;
        width: 20em;
    }

    .category-wallet-select {
        font-size: 1.2rem;
    }

    hr {
        border: 1px solid var(--bg-dark);
    }

    .buttons {
        height: 3em;
    }
</style>
