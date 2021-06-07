<script lang="ts">
    export let buttons: {
        text: string,
        action: () => any
    }[] = [];
</script>

<div class="container">
    {#each buttons as button}
    <button on:click={button.action}>{button.text}</button>
    {/each}
</div>

<style>
    .container {
        padding: 0.5em;
        border-radius: 0.5em;
        background: var(--bg-light);
        box-shadow: var(--shadow) 0 0.3em 0.9em;
    }

    button {
        display: block;
        margin: 0.5em 0;
        text-align: end;
        color: var(--highlight);
        background: none;
        border: none;
    }

    button:hover {
        color: var(--highlight-hover);
        text-decoration: underline;
    }

    button:active {
        color: var(--highlight-active);
        text-decoration: underline;
    }
</style>