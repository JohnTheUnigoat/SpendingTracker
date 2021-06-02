<script lang="ts">
import token from "../stores/tokenStore";

    import user from "../stores/userStore";

    let logOut = () => {
        token.set(null);
        user.set(null);
    }
</script>

{#if $user}
<img src={$user.pictureUrl} alt="avatar">

<p>{$user.firstName} {$user.lastName}</p>

<button on:click={logOut}>
    Sign Out
</button>
{/if}

