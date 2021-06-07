<script lang="ts">
    import token from '../stores/tokenStore';
    import LoggedIn from './LoggedIn.svelte';
</script>

<div class="container">
    <div class="logo">
        Spending Tracker
    </div>
    {#if $token}
    <div>
        <LoggedIn />
    </div>
    {/if}
</div>

<style>
    .container {
        background: var(--bg-light);
        padding: 1em;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .container > * {
        height: 3em;
    }

    .logo {
        color: var(--highlight);
        font-family: Verdana;
        font-size: 1.1em;
        max-width: 5em;
        display: flex;
        align-items: center;
    }

    @media screen and (max-width: 400px) {
        .container {
            padding: 0.5em;
        }
    }
</style>