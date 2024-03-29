<script lang="ts">
    import { onMount } from 'svelte';
    import api from '../../api';
    import { getReportPeriod, reportPeriods } from '../../models/wallet/ReportPeriod';
    import type { ReportPeriod } from '../../models/wallet/ReportPeriod';
    import type { Wallet } from '../../models/wallet/Wallet';
    import ShortSummary from './components/ShortSummary.svelte';
    import TwoStateSelector from '../../components/TwoStateSelector.svelte';
    import TransactionList from './components/TransactionList.svelte';
    import Modal from '../../components/Modal.svelte';
    import wallets from '../../stores/walletStore';
    import AddTransaction from './components/AddTransaction.svelte';
    import Summary from './components/Summary.svelte';

    let isSummarySelected = false;

    let currentWallet: Wallet | null = null;
    let currentReportPeriod: ReportPeriod | null = null;

    let isTransactionModalOpen = false;

    const walletChange = () => {
        if (currentWallet) {
            currentReportPeriod = getReportPeriod(currentWallet.defaultReportPeriod);
        }
    };

    const reportPeriodChange = () => {
        if (currentWallet && currentReportPeriod) {
            currentWallet.defaultReportPeriod = currentReportPeriod.code;
        }
    };

    // props for fetching transaction
    $: walletId = currentWallet?.id ?? null;
    $: reportPeriod = currentReportPeriod?.code ?? null;

    let summaryNeedsUpdate = false;
    let transactionsNeedUpdate = false;

    onMount(async () => {
        let res = await api.getWallets();
        $wallets = res.data;

        currentWallet = $wallets[0];
        currentReportPeriod = getReportPeriod($wallets[0].defaultReportPeriod) ?? currentReportPeriod;
    });

    const update = () => {
        summaryNeedsUpdate = true;
        transactionsNeedUpdate = true;
    }

    $: if (walletId && reportPeriod) {
        isSummarySelected;
        update();
    }

    const onTransactionModalDone = () => {
        isTransactionModalOpen = false;
        update();
    }
</script>

<div class="container">
    <div class="select-container">
        <div class="input wallet">
            <i class="fas fa-wallet"></i>
            <!-- svelte-ignore a11y-no-onchange -->
            <select bind:value={currentWallet} on:change={walletChange}>
                {#each $wallets as wallet}
                <option value={wallet}>{wallet.name}</option>
                {/each}
            </select>
        </div>
        
        <div class="input period">
            <i class="fas fa-calendar-alt"></i>
            <!-- svelte-ignore a11y-no-onchange -->
            <select bind:value={currentReportPeriod} on:change={reportPeriodChange}>
                {#each reportPeriods as reportPeriodOption}
                <option value={reportPeriodOption}>
                    {reportPeriodOption.text}
                </option>
                {/each}
            </select>
        </div>

        <div class="summary-toggle">
            <TwoStateSelector labels={["History", "Summary"]} bind:value={isSummarySelected}/>
        </div>
    </div>

    <!-- <div class="time-range">
        <div class="input">
            <label for="from-input">From</label>
            <input type="date" id="from-input">
        </div>

        <div class="input">
            <label for="from-input">From</label>
            <input type="date" id="from-input">
        </div>
    </div> -->

    {#if isSummarySelected}
    <Summary {walletId} {reportPeriod} bind:needUpdate={summaryNeedsUpdate} />
    {:else}
    <ShortSummary {walletId} {reportPeriod} bind:needUpdate={summaryNeedsUpdate} />
    <TransactionList {walletId} {reportPeriod} bind:needUpdate={transactionsNeedUpdate} />
    {/if}

    <div class="add-transaction" on:click={() => isTransactionModalOpen = true}>
        <div class="circle">
            <i class="fas fa-plus"></i>
        </div>
    </div>

    {#if isTransactionModalOpen}
    <Modal>
        <AddTransaction onComplete={onTransactionModalDone} />
    </Modal>
    {/if}
</div>

<style>
    .container {
        width: 100%;
        padding: 1em 2em;
        background: var(--bg-light);
        border-radius: 0.5em;
        color: var(--white);
    }

    .select-container {
        display: flex;
        justify-content: space-between;
        align-items: center;
        /* margin-bottom: 0.7em; */
    }

    .select-container > * {
        margin-bottom: 0.4em;
    }

    .input {
        display: flex;
        align-items: center;
        width: 100%;
    }

    .input.wallet {
        padding-right: 0.2em;
    }

    .input.period {
        padding-left: 0.2em;
    }

    .summary-toggle {
        margin-left: 0.4em;
        width: 50%;
    }

    .input .fas {
        font-size: 150%;
        width: 1.2em;
        margin-right: 0.2em;
        text-align: center;
        color: var(--highlight);
    }

    .input select,
    .input input {
        padding: 0.5em;
        color: var(--white);
        background: var(--bg-medium);
        width: 100%;
        border: none;
        border-radius: 0.5em;
        outline: none;
    }

    .time-range {
        display: flex;
    }

    .input {
        display: flex;
    }

    .add-transaction {
        font-size: 250%;
        position: sticky;
        text-align: end;
        bottom: 0.5em;
        padding: 0.3em 0.15em 0 0;
    }

    .add-transaction .circle {
        width: 1.2em;
        height: 1.2em;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 50%;
        margin-left: auto;
        background: var(--highlight);
        color: var(--bg-medium);
        box-shadow: var(--bg-dark) 0 0.1em 0.3em 0.1em;
    }

    .add-transaction .circle:hover {
        background: var(--highlight-hover);
        color: var(--bg-dark);
    }

    .add-transaction .circle:active {
        background: var(--highlight-active);
    }

    @media screen and (max-width: 700px) {
        .container {
            padding: 0.5em;
        }

        .select-container {
            flex-wrap: wrap;
        }

        .input {
            width: 50%;
        }

        .summary-toggle {
            width: 100%;
            margin: 0;
        }

        .add-transaction .circle {
            width: 1.5em;
            height: 1.5em;
        }
    }

    @media screen and (max-width: 400px) {
        .input .fas {
            font-size: 120%;
        }

        .input select {
            padding: 0.2em;
        }
    }
</style>
