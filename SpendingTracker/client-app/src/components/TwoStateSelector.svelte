<script lang="ts">
    export let labels: [string, string];
    export let value = false;
</script>

<div class="two-state-select">
    <span
        on:click={() => value = false}
        class:selected={value == false}
    >
        {labels[0]}
    </span>

    <span
        on:click={() => value = true}
        class:selected={value == true}
    >
        {labels[1]}
    </span>
</div>

<style>
    .two-state-select {
        display: flex;
        font-size: 90%;
    }

    span {
        width: 100%;
        text-align: center;
        background: var(--bg-medium);
        color: var(--white);
        padding: 0.3em;
        border: 0.15em solid var(--highlight);
    }

    span:first-child {
        border-right: none;
        border-radius: 0.3em 0 0 0.3em;
    }

    span:last-child {
        border-left: none;
        border-radius: 0 0.3em 0.3em 0;
    }

    .selected {
        background: var(--highlight);
        color: var(--bg-dark);
    }
</style>