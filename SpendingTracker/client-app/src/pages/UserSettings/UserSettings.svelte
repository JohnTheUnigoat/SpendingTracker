<script lang="ts">
    import wallets from "../../stores/walletStore";
    import WalletSettings from "./components/WalletSettings.svelte";
</script>

<div class="container">
    <h2>Settings</h2>

    <hr>

    <h3>Wallets</h3>

    <div class="wallets-settings">
        {#each $wallets as wallet}
        <div class="wallet-settings">
            <WalletSettings {wallet} />
        </div>
        {/each}
    </div>
</div>

<style>
    .container {
        width: 100%;
        border-radius: 0.5em;
        padding: 1em;
        background: var(--bg-light);
        color: var(--white);
    }

    @media screen and (max-width: 700px) {
        .container {
            padding: 0.5em;
        }
    }

    hr {
        margin-bottom: 1.5em;
        border-color: var(--bg-dark);
    }

    h2, h3 {
        text-align: center;
        margin: 0 0 1em 0;
    }

    h3 {
        color: var(--gray);
    }

    .wallets-settings {
        display: flex;
    }

    .wallet-settings {
        width: 100%;
        padding: 0.5em;
        border-radius: 0.5em;
        background: var(--bg-medium);
    }
</style>