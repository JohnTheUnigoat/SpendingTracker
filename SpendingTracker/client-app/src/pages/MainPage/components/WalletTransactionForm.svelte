<script lang="ts">
    import type { AddTransactionRequest } from "../../../models/api/AddTransaction";
    import type { Wallet } from "../../../models/wallet/Wallet";
    import wallets from "../../../stores/walletStore";

    let fromWallet: Wallet | null = null;
    let toWallet: Wallet | null = null;

    let amount: number | null = null;

    $: otherWalletOptions = $wallets.filter(w => w !== fromWallet);

    $: if (amount && amount < 0) amount = -amount;

    $: walletId = toWallet?.id ?? null;
    $: otherWalletId = fromWallet?.id ?? null;

    export let readyToSave = false;
    export let payload: AddTransactionRequest;

    $: readyToSave = walletId !== null && otherWalletId !== null && amount !== null && amount > 0;

    $: if (readyToSave) {
        payload = {
            walletId: walletId as number,
            amount: amount as number,
            otherWalletId: otherWalletId as number
        };
    }
    
</script>

<div class="input">
    <label for="from-wallet-select">From</label>

    <select id="from-wallet-select" bind:value={fromWallet}>
        {#if fromWallet === null}
        <option value={null}> Choose a wallet </option>
        {/if}

        {#each $wallets as wallet}
        <option value={wallet}>{wallet.name}</option>
        {/each}
    </select>
</div>

<div class="input">
    <label for="to-wallet-select">To</label>

    <select id="to-wallet-select" bind:value={toWallet}>
        {#if toWallet === null}
        <option value={null}> Choose a wallet </option>
        {/if}

        {#each otherWalletOptions as wallet}
        <option value={wallet}>{wallet.name}</option>
        {/each}
    </select>
</div>


<div class="input">
    <input type="number" min="0" bind:value={amount} placeholder="Amount">
</div>

<style>
    .input {
        width: 100%;
        display: flex;
        align-items: center;
        margin-bottom: 0.5em;
    }

    label {
        width: 3em;
        color: var(--gray);
    }

    input, select {
        font-size: inherit;
        flex: 1 0 auto;
        padding: 0.8em;
        background: var(--bg-medium);
        color: var(--white);
        border: none;
        border-radius: 0.5em;
    }
</style>
