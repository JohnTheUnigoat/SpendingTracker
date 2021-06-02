<script lang="ts">
	import GoogleLogin from "./components/GoogleLogin.svelte";
	import Tabs from "./components/Tabs/Tabs.svelte";
	import type { TabInfo } from "./components/Tabs/tabinfo";
	import Page1 from "./components/Page1.svelte";
	import Page2 from "./components/Page2.svelte";
	import token from "./stores/tokenStore";
	import LoggedIn from "./components/LoggedIn.svelte";
	import user from "./stores/userStore";
import api from "./api";

	const tabs: TabInfo[] = [
		{
			name: "Page #1",
			component: Page1
		},
		{
			name: "Page #2",
			component: Page2
		}
	]

	if($token) {
		api.getUser().then(res => {
			user.set(res.data);
		});
	}
</script>

{#if $token}
<LoggedIn />
<Tabs tabs={tabs}></Tabs>
{:else}
<GoogleLogin />
{/if}

