<script lang="ts">
    import token from "../stores/tokenStore";
    import user from "../stores/userStore";

    let logOut = () => {
        token.set(null);
        user.set(null);
    }
</script>

{#if $user}
<div class="container">
    <img src={$user.pictureUrl} alt="avatar">
    
    <p>{$user.firstName} {$user.lastName}</p>
    
    <button on:click={logOut}>
        Sign Out
    </button>
</div>
{/if}

<style>
    .container {
        display:  flex;
        height: 100%;
        color: var(--white);
        align-items: center;
    }

    .container > * {
        vertical-align: middle;
    }

    img {
        border: 1px solid var(--bg-dark);
        border-radius: 1000em;
        height: 100%;
        margin-right: 0.3em;
    }

    p {
        margin-right: 0.4em;
    }

    button {
        color: var(--highlight);
        background: none;
        border: none;
        text-decoration: underline;
    }

    button:hover {
        color: var(--highlight-hover);
    }
</style>
