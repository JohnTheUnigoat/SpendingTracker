<script lang="ts">
    import type { Wallet } from '../models/Wallet';
    import { walletStore } from '../stores/walletStore';

    let wallet: Wallet;

    $: console.log(wallet);
</script>

<h1>And now Page 2</h1>

<select bind:value={wallet}>
    {#each $walletStore as wallet}
    <option value={ wallet }>{ wallet.name }</option>
    {/each}
</select>

{#each $walletStore as wallet}
<div>{ wallet.name }</div>
{/each}
