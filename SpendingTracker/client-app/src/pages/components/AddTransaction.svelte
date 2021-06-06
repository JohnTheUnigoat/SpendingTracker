<script lang="ts">
    import api from "../../api";
    import Buttons from "../../components/Buttons.svelte";
    import TwoStateSelector from "../../components/TwoStateSelector.svelte";
    import type { AddTransactionRequest } from "../../models/api/AddTransaction";
    import AddCategoryTransaction from "./AddCategoryTransaction.svelte";

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
    <div>here will be wallet thing</div>
    {:else}
    <AddCategoryTransaction bind:readyToSave={readyToSave} bind:payload={payload} />
    {/if}

    <div class="form"></div>

    <div class="buttons">
        <Buttons
            primary={{text: 'Add', action: addTransaction}}
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
