<script lang="ts">
    import { onDestroy, onMount } from 'svelte';

    onMount(() => {
        document.body.classList.add('modal-open');
    });

    onDestroy(() => {
        document.body.classList.remove('modal-open');
    });
</script>

<div class="bg"></div>

<div class="content">
    <slot></slot>
</div>

<style>
    .bg {
        position: fixed;
        inset: 0;
        background-color: var(--bg-dark-transparent);
    }

    .content {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }
</style>