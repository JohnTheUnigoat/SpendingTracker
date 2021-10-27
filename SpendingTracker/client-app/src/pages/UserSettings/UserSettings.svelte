<script lang="ts">
    import categories from "../../stores/categoryStore";
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

    <hr>

    <h3>Categories</h3>

    {#if $categories}
    <div class="categories">
        <div>
            <h4>Income</h4>
            <ul>
                {#each $categories.income as category}
                <li>
                    {category.name}
                    <i class="fas fa-trash-alt"></i>
                </li>
                {/each}
                <li>
                    <a href="$">
                        <i class="fas fa-plus"></i>
                        Add category
                    </a>
                </li>
            </ul>
        </div>

        <div>
            <h4>Expense</h4>
            <ul>
                {#each $categories.expense as category}
                <li>
                    {category.name}
                    <i class="fas fa-trash-alt"></i>
                </li>
                {/each}
                <li>
                    <a href="$">
                        <i class="fas fa-plus"></i>
                        Add category
                    </a>
                </li>
            </ul>
        </div>
    </div>
    {/if}
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

    .wallet-settings:not(:last-child) {
        margin-right: 0.25em;
    }
    
    .wallet-settings:not(:first-child) {
        margin-left: 0.25em;
    }

    .categories {
        display: flex;
        justify-content: space-between;
    }

    .categories > * {
        width: 48%;
        padding: 0.5em;
        border-radius: 0.5em;
        background: var(--bg-medium);
        padding: 0.7em 10%;
        /* text-align: center; */
    }

    .categories h4 {
        margin: 0 0 0.5em 0;
    }

    ul {
        list-style: none;
        margin: 0;
        padding: 0;
        color: var(--gray);
    }

    ul .fas, a {
        color: var(--highlight);
    }

li {
    margin: 0.25em 0;
}

    li:last-child {
        margin-top: 0.4em;
    }
</style>