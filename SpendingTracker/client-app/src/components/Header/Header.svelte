<script lang="ts">
    import token from '../../stores/tokenStore';
    import LoggedIn from './LoggedIn.svelte';

</script>

<div class="container">
    <div class="logo">
        SpendingTracker
    </div>
    {#if $token}
    <LoggedIn />
    {/if}
</div>

<style>
    .container {
        background: #333;
        padding: 1em;
        display: flex;
        justify-content: space-between;
        height: 2.5em;
    }

    .logo {
        color: #eee;
        font-family: Verdana;
        font-size: 2em;
    }
</style>