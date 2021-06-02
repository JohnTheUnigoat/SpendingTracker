<script lang="ts">
    import type { TabInfo } from "./tabinfo";

    export let tabs: TabInfo[];

    let currentTab = tabs[0];
</script>

<div class="buttons">
    {#each tabs as tab}
    <button
        on:click={() => currentTab = tab}
        class:selected={currentTab == tab}>
        { tab.name }
    </button>
    {/each}
</div>

<!-- <p>{ currentTab.name }</p> -->

<svelte:component this={currentTab.component} />

<style>
    button {
        box-sizing: border-box;
        border-radius: 0;
        border: 3px solid #333;
        background: #333;
        color: #eee;
        font-weight: bold;
        margin: 5px;
    }

    button.selected {
        background: #eee;
        color: #333;
    }
</style>