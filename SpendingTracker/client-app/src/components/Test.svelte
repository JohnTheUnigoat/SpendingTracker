<script lang="ts">
    import type { Test } from "../models/Test";

    let promise: Promise<Test>;

    const fetchTestAsync = async (): Promise<Test> => {
        return await (await fetch("/api/test")).json() as Test;
    };

    promise = fetchTestAsync();
</script>

{#await promise}
    <p>...loading</p>
{:then res} 
    <h2>{res.number}</h2>
    <h3>{res.text}</h3>
{:catch}
    <p>Something went wrong. Sad.</p>
{/await}
