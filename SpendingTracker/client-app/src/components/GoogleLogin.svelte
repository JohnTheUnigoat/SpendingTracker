<svelte:head>
    <script src="https://accounts.google.com/gsi/client" async defer></script>
</svelte:head>

<script lang="ts">
    import api from '../api';
    import type { GoogleResponse } from '../models/auth/GoogleResponse';
    import token from '../stores/tokenStore';
    import user from '../stores/userStore';

    (window as any).onSignIn = async (response: GoogleResponse) => {
        var res = await api.signIn(response.credential);
        token.set(res.data.accessToken);
        user.set(res.data.user);
    }
</script>

<div id="g_id_onload"
     data-client_id="761268948043-glqrvs2am2unak1o90ksu94koegslgq6.apps.googleusercontent.com"
     data-context="signin"
     data-ux_mode="popup"
     data-callback="onSignIn"
     data-auto_prompt="false">
</div>

<div class="g_id_signin"
     data-type="standard"
     data-shape="rectangular"
     data-theme="filled_black"
     data-size="large"
     data-logo_alignment="left">
</div>
