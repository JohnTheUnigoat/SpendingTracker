<script lang="ts">
    import type { Todo } from '../models/Todo';

    export let todo: Todo;
    export let onChange: () => void;
</script>

<div on:click={onChange}>
    <span class:completed={todo.isCompleted}>{ todo.text }</span>
    <input type="checkbox" checked={todo.isCompleted} readonly>
</div>

<style>
    div {
        display: flex;
        padding: 1em;
        border: 1px solid #333;
        align-items: center;
    }

    span {
        flex-grow: 1;
        text-align: center;
    }

    .completed {
        text-decoration: line-through;
        color: #776
    }
</style>
