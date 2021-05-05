<svelte:head>
    <script src="https://apis.google.com/js/platform.js" async defer></script>
</svelte:head>

<script lang="ts">
    (window as any).onSignIn = (thing: any) => {
        console.log(thing.getAuthResponse());
    }
</script>

<div class="g-signin2" data-onsuccess="onSignIn"></div>

<style>
    .g-signin2 {
        display: flex;
        justify-content: center;
        margin-bottom: 1em;
    }
</style>
