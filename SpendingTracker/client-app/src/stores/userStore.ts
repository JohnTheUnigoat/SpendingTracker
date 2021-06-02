import { writable } from "svelte/store";
import type { User } from "../models/auth/User";

const user = writable<User | null>(null);

export default user;