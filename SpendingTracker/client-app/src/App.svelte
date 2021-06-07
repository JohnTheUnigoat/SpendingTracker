<script lang="ts">
    import { onMount } from 'svelte';
    import api from './api';
    import token from './stores/tokenStore';
    import user from './stores/userStore';
    import MainPage from './pages/MainPage.svelte';
    import Header from './components/Header.svelte';
    import GoogleLogin from './components/GoogleLogin.svelte';
    import categories from './stores/categoryStore';

    onMount(async () => {
        if($token) {
            const [userRes, categoriesRes] = await Promise.all([api.getUser(), api.getCategories()]);

            user.set(userRes.data);
            categories.set(categoriesRes.data);
        }
    });
</script>

<div class="vertical-container">
    <Header />

    <div class="main">
        {#if $token}
        <MainPage />
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
