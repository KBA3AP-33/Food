<template>
    <div class="search-container">
        <form class="search-form" @submit.prevent="searchSubmit">
            <div class="icon search-icon" @click="searchSubmit"></div>
            <input type="search" class="search" name="recipe" v-model="searchValue" placeholder="Поиск по названию">
        </form>
        <div class="search-result">
            <div class="result">Найдено: <b>{{ count }} {{ getCorrectWriting(count, { 1: 'рецепт', 2: 'рецепта', 5: 'рецептов' }) }}</b></div>
            <div class="sort">
                Сортировать: <NuxtSelect style="font-weight: bold;" :options="options" :select-value="select" @changeSelect="$emit('changeSort', $event)"/>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import { sortOptins } from '~/data';

    export default {
        data: () => ({
            searchValue: '',
            options: sortOptins,
        }),
        mounted() {
            this.searchValue = this.search;
        },
        props: {
            search: {
                type: String as PropType<string>,
                default: ''
            },
            select: {
                type: Number as PropType<number>,
                default: 0
            },
            count: {
                type: Number as PropType<number>,
                default: 0
            },
        },
        methods: {
            getCorrectWriting(num: number, result: { 1: string, 2: string, 5: string }) {
                let second = num % 10;
                let first = (num % 100 - second) / 10;

                if (second === 1 && first !== 1) return result?.[1];
                else if (second > 1 && second < 5 && first !== 1) return result?.[2];
                else return result?.[5];
            },
            searchSubmit() {
                this.$emit('changeSearch', this.searchValue.trim());
            },
        },
        watch: {
            search(nextValue: string) {
                this.searchValue = nextValue;
            },
        },
    }
</script>

<style scoped>
    .search-result {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
    }

    .sort {
        display: flex;
        justify-content: flex-end;
        flex-wrap: nowrap;
    }

    .search-form {
        height: 100%;
        width: 100%;
        display: flex;
        align-items: center;
        gap: 10px;
        border: 1px solid var(--default-color-gray);
        border-radius: 50px;
        padding: 0 10px;
        overflow: hidden;
        transition: border-color .3s;
        margin-bottom: 3px;
    }

    .search-form:hover {
        border-color: var(--default-color-black);
    }

    .search {
        width: 100%;
        height: 100%;
        font-size: .95rem;
        padding: 10px 0;
        outline: none;
        border: none;
    }

    .icon {
        width: 20px;
        height: 20px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .search-icon {
        background-image: var(--icon-search);
    }

    .search-icon:hover {
        background-image: var(--icon-search-hover);
    }

    .result {
        padding-left: 15px;
    }

    .search-form:has(.search:focus) {
        border-color: var(--default-color-black);
    }

    input:-webkit-autofill,
    input:-webkit-autofill:hover, 
    input:-webkit-autofill:focus,
    textarea:-webkit-autofill,
    textarea:-webkit-autofill:hover,
    textarea:-webkit-autofill:focus,
    select:-webkit-autofill,
    select:-webkit-autofill:hover,
    select:-webkit-autofill:focus {
        -webkit-box-shadow: 0 0 0px 1000px var(--default-color-white) inset;
    }

    @media (max-width: 576px) {
        .search-result * {
            display: none;
        }
    }

    @media (max-width: 425px) {
        .search {
            padding: 7px 0;
        }

        .search-form {
            margin: 0;
        }
    }
</style>
