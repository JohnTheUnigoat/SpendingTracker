<script lang="ts">
import api from "../../api";
import Buttons from "../../components/Buttons.svelte";
import TwoStateSelector from "../../components/TwoStateSelector.svelte";
import type { Categories } from "../../models/category/Categories";

import type { Category } from "../../models/category/Category";

    import type { Wallet } from "../../models/wallet/Wallet";
import categories from "../../stores/categoryStore";
import user from "../../stores/userStore";
    import wallets from "../../stores/walletStore";

    export let onComplete: (success: boolean) => any;

    let selectedWallet: Wallet | null = null;

    let isExpense = false;
    let amount = 0;

    let categoriesOptions: Category[] = [];
    let selectedCategory: Category | null = null;

    const updateCategoriesOptions = async () => {
        let ownWallet = $user?.email === selectedWallet?.ownerEmail;

        let walletSpecificCategories: Categories | null = null;

        if (ownWallet === false) {
            walletSpecificCategories = (await api.getWalletCategories(<number>selectedWallet?.id)).data
        }

        // if specific wallet categories exist, use them, otherwise use default user categories
        let applicableCategories = walletSpecificCategories ?? $categories as Categories;

        categoriesOptions = isExpense ? applicableCategories.expense : applicableCategories.income;
        selectedCategory = null;
    }

    $: if (selectedWallet) {
        isExpense;
        updateCategoriesOptions();
    }

    $: if (amount < 0) amount = -amount;

    $: walletId = selectedWallet?.id ?? null;
    $: categoryId = selectedCategory?.id ?? null;
    $: finalAmount = isExpense ? -amount : amount;

    $: readyToSave = walletId !== null && categoryId !== null && finalAmount != 0;

    const addTransaction = async () => {
        if(readyToSave) {
            try {
                await api.addTransaction(<number>walletId, finalAmount, <number>categoryId);
                onComplete(true);
            } catch {
                console.log('Something went wrong when trying to save transaction');
            }
        }
    }
</script>

<div class="container">
    <div class="input">
        <select bind:value={selectedWallet}>
            {#if selectedWallet === null}
            <option value={null}> Choose a wallet </option>
            {/if}
    
            {#each $wallets as wallet}
            <option value={wallet}>{wallet.name}</option>
            {/each}
        </select>
    </div>

    <div class="income-expense-select">
        <TwoStateSelector labels={['Income', 'Expense']} bind:value={isExpense} />
    </div>

    <div class="input">
        <i class={isExpense ? 'fas fa-minus' :'fas fa-plus'}></i>
        <input type="number" min="0" bind:value={amount} placeholder="Amount">
    </div>

    <div class="input">
        <select bind:value={selectedCategory} disabled={selectedWallet === null}>
            {#if selectedCategory === null}
            <option value={null}> Choose a category </option>
            {/if}

            {#each categoriesOptions as category}
            <option value={category}>{category.name}</option>
            {/each}
        </select>
    </div>

    <div class="buttons">
        <Buttons
            primary={{text: 'Add', action: addTransaction, disabled: readyToSave === false}}
            secondary={{text: 'Cancel', action: () => onComplete(false)}}
        />
    </div>
</div>

<style>
    .container {
        background: var(--bg-light);
        padding: 1em;
        border-radius: 0.5em;
        border: 0.2em solid var(--bg-dark);
    }

    .input {
        width: 100%;
        display: flex;
        align-items: center;
        margin-bottom: 0.5em;
    }

    .income-expense-select {
        margin-bottom: 0.5em;
        
    }

    .fas {
        width: 2em;
        font-size: 120%;
        color: var(--highlight);
        text-align: center;
    }

    input, select {
        flex: 1 0 auto;
        padding: 0.5em;
        background: var(--bg-medium);
        color: var(--white);
        border: none;
        border-radius: 0.5em;
    }
</style>