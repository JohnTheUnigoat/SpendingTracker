<script lang="ts">
	import GoogleLogin from "./components/GoogleLogin.svelte";
	import Tabs from "./components/Tabs/Tabs.svelte";
	import type { TabInfo } from "./components/Tabs/tabinfo";
	import Page1 from "./components/Page1.svelte";
	import Page2 from "./components/Page2.svelte";
	import token from "./stores/tokenStore";

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
</script>

<GoogleLogin />

{#if $token}
<Tabs tabs={tabs}></Tabs>
{/if}
