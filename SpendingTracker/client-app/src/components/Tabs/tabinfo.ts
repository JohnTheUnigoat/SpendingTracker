import type { SvelteComponent } from "svelte";

export interface TabInfo {
    name: string;
    component: typeof SvelteComponent;
}