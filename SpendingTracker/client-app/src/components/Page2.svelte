<script lang="ts">
    import type { ReportPeriodString } from '../models/wallet/ReportPeriodString';
    import type { Wallet } from '../models/wallet/Wallet';
    import { walletStore } from '../stores/walletStore';

    let wallet: Wallet;

    let reportPeriodString: ReportPeriodString;

    $: console.log(wallet);
</script>

<h1>And now Page 2</h1>

<select bind:value={wallet}>
    {#each $walletStore as wallet}
    <option value={ wallet }>{ wallet.name }</option>
    {/each}
</select>

// TODO: report period select