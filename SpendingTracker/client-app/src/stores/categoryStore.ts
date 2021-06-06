import { writable } from "svelte/store";
import type { Categories } from "../models/category/Categories";

const categories = writable<Categories | null>(null);

export default categories;