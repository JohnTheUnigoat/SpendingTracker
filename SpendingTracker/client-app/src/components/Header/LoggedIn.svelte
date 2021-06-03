<script lang="ts">
    import token from '../../stores/tokenStore';
    import user from '../../stores/userStore';

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
        color: #eee;
        align-items: center;
    }

    .container > * {
        vertical-align: middle;
    }

    img {
        border: 1px solid #222;
        border-radius: 1000em;
        height: 100%;
        margin-right: 0.3em;
    }

    p {
        margin-right: 0.4em;
    }

    button {
        color: rgb(202, 151, 10);
        background: none;
        border: none;
        text-decoration: underline;
    }

    button:hover {
        color:rgb(252, 185, 41);
    }
</style>
