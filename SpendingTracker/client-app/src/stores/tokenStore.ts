import { writable } from "svelte/store";

const {subscribe, set} = writable<string| null>(null);

const localStorageKey = 'auth_token';

const token = {
    subscribe,
    set: (val: string | null) => {
        if (val === null) {
            localStorage.removeItem(localStorageKey);
        } else {
            localStorage.setItem(localStorageKey, val);
        }

        set(val);
    }
};

let existingToken = localStorage.getItem(localStorageKey);

if (existingToken !== null) {
    token.set(existingToken);
} 

export default token;