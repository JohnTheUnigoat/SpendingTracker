import http from "./api"
import App from "./App.svelte";

const app = new App({
	target: document.body,
	props: {
		name: 'big brain boi'
	}
});

export default app;