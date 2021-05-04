<script lang="ts">
    import TodoItem from './TodoItem.svelte';
    import type { Todo } from '../models/Todo';

    let todos: Todo[] = [
        {
            id: 1,
            text: 'Buy milk',
            isCompleted: false
        },
        {
            id: 2,
            text: 'Buy bread',
            isCompleted: false
        },
        {
            id: 3,
            text: 'Do the dishes',
            isCompleted: false
        },
    ];

    const toggle = (index: number) => {
        todos[index].isCompleted = !todos[index].isCompleted;
    }
</script>

<div class="container">
    {#each todos as todo, i}
        <TodoItem todo={todo} onChange={() => toggle(i)}></TodoItem>
    {/each}

    <ul>
        {#each todos as todo}
            <li>{todo.isCompleted ? '✓' : '✕'} {todo.text}</li>
        {/each}
    </ul>
</div>

<style>
    .container {
        width: 40%;
        min-width: 20em;
        margin: auto;
    }

    li {
        list-style: none;
        width: 100%;
        text-align: left;
    }
</style>
