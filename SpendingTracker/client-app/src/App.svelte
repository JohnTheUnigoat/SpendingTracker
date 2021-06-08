<script lang="ts">
    import { onMount, SvelteComponent } from 'svelte';
    import api from './api';
    import token from './stores/tokenStore';
    import user from './stores/userStore';
    import categories from './stores/categoryStore';
    import Header from './components/Header.svelte';
    import GoogleLogin from './components/GoogleLogin.svelte';
    import MainPage from './pages/MainPage/MainPage.svelte';
    import UserSettings from './pages/UserSettings/UserSettings.svelte';
    import wallets from './stores/walletStore';

    onMount(async () => {
        if($token) {
            const [userRes, categoriesRes, walletsRes] =
                await Promise.all([api.getUser(), api.getCategories(), api.getWallets()]);

            user.set(userRes.data);
            categories.set(categoriesRes.data);
            wallets.set(walletsRes.data);
        }
    });

    let page: typeof SvelteComponent = MainPage;
</script>

<div class="vertical-container">
    <Header on:settings-click={() => page = UserSettings}/>

    <div class="main">
        {#if $token}
        <svelte:component this={page} />
        {:else}
        <GoogleLogin />
        {/if}
    </div>
</div>

<style>
    .vertical-container {
        display: flex;
        flex-direction: column;
        height: 100%;
    }

    .main {
        flex: 1 1 auto;
        width: 80%;
        max-width: 800px;
        margin: auto;
        padding: 0.7em;
        display: flex;
        justify-content: center;
    }

    @media screen and (max-width: 700px) {
        .vertical-container {
            font-size: 90%;
        }

        .main {
            width: 100%;
        }
    }

    @media screen and (max-width: 400px) {
        .vertical-container {
            font-size: 70%;
        }
    }
</style>
