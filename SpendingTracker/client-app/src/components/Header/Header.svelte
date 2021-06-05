<script lang="ts">
    import token from '../../stores/tokenStore';
    import LoggedIn from './LoggedIn.svelte';

</script>

<div class="container">
    <div class="logo">
        Spending Tracker
    </div>
    {#if $token}
    <LoggedIn />
    {/if}
</div>

<style>
    .container {
        background: var(--bg-light);
        padding: 1em;
        display: flex;
        justify-content: space-between;
        height: 4.6em;
    }

    .logo {
        color: var(--highlight);
        font-family: Verdana;
        font-size: 1.1em;
        max-width: 5em;
    }
</style>