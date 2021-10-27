<script lang="ts">
    import Buttons from "../../../components/Buttons.svelte";
    import type { Wallet } from "../../../models/wallet/Wallet";

    export let wallet: Wallet | null = null;
</script>

{#if wallet}
<input bind:value={wallet.name}>

<div class="buttons">
    <Buttons
        primary={{text: 'Save', action: () => {}}}
        secondary={{text: 'Cancel', action: () => {}}}
    />
</div>
{/if}

<style>
    input {
        padding: 0.5em;
        width: 100%;
        border: none;
        border-radius: 0.5em;
        background-color: var(--bg-dark);
        color: var(--white);
        margin-bottom: 0.5em;
    }
</style>