<script lang="ts">
import { text } from "svelte/internal";

    interface ButtonConfig {
        text: string;
        action: () => any;
        disabled?: boolean;
    }

    export let primary: ButtonConfig; 
    export let secondary: ButtonConfig;

    $: console.log(primary.disabled);
    $: console.log(secondary.disabled);
</script>

<div class="container">
    <button on:click={secondary.action} disabled={secondary.disabled ?? false}>
        {secondary.text}
    </button>
    
    <button on:click={primary.action} disabled={primary.disabled ?? false}>
        {primary.text}
    </button>
</div>

<style>
    .container {
        display: flex;
    }

    .container button {
        width: 100%;
        padding: 0.3em;
        border: none;
        border-radius: 0.5em;
    }

    .container button:first-child {
        margin-right: 0.2em;
        background: var(--bg-medium);
        color: var(--white);
    }

    .container button:first-child:hover {
        background: var(--bg-dark);
    }

    .container button:first-child:active {
        background: black;
    }

    .container button:last-child {
        margin-left: 0.2em;
        background: var(--highlight);
        color: var(--bg-dark);
    }

    .container button:last-child:hover {
        background: var(--highlight-hover);
    }

    .container button:last-child:active {
        background: var(--highlight-active);
    }

    .container button:disabled {
        background: var(--bg-medium-hover) !important;
        color: var(--gray) !important;
    }
</style>