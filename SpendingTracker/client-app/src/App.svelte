<script lang="ts">
    import token from "./stores/tokenStore";
    import user from "./stores/userStore";
    import api from "./api";
    import { Header } from "./components/Header";
    import { MainPage } from "./components/MainPage";
    import { onMount } from "svelte";
    import GoogleLogin from "./components/GoogleLogin.svelte";

    onMount(async () => {
        if($token) {
            let res = await api.getUser();
            user.set(res.data);
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

    @media screen and (max-width: 600px) {
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
