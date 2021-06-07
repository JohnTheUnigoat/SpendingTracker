<script lang="ts">
    import { createEventDispatcher } from "svelte";
    import token from "../stores/tokenStore";
    import user from "../stores/userStore";
    import LoggedInMenu from "./LoggedInMenu.svelte";

    const dispatchUserSettings = createEventDispatcher<{'settings-click': null}>();

    let logOut = () => {
        token.set(null);
        user.set(null);
    }

    let menuOpen = false;

    let menuButton: HTMLElement;
    let menuDiv: HTMLElement;

    const handleClickOutsideMenu: svelte.JSX.MouseEventHandler<HTMLElement> = (e) => {
        let target = e.target as Node;

        if (menuButton && menuButton.contains(target)) return;

        if (menuDiv && menuDiv.contains(target) == false) {
            menuOpen = false;
        }
    }
</script>

<svelte:body on:click={handleClickOutsideMenu} />

{#if $user}
<div class="container">
    <img src={$user.pictureUrl} alt="avatar">
    
    <p>{$user.firstName} {$user.lastName}</p>
    
    <div class="menu-button" bind:this={menuButton} on:click={() => menuOpen = !menuOpen}>
        <i class="fas fa-bars"></i>
    </div>

    {#if menuOpen}
    <div class="menu" bind:this={menuDiv}>
        <LoggedInMenu buttons={[
            { text: 'User Settings', action: () => dispatchUserSettings('settings-click') },
            { text: 'Sign Out', action: logOut }
        ]}/>
    </div>
    {/if}
</div>
{/if}

<style>
    .container {
        display:  flex;
        height: 100%;
        color: var(--white);
        align-items: center;
        position: relative;
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

    .menu-button {
        font-size: 170%;
        color: var(--highlight);
    }

    .menu-button:hover {
        color: var(--highlight-hover);
    }

    .menu-button:active {
        color: var(--highlight-active);
    }

    .menu {
        position: absolute;
        top: 120%;
        right: 0;
    }
</style>
