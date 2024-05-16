<template>
    <form id="filter" v-if="userFilters.length" class="filter-container" @submit.prevent="submit">
        <div class="icon icon-close" @click="$emit('close')"></div>
        <div class="filter">
            <NuxtSpoiler v-for="item in filters" :key="item.id" :header="item.header" :is-visible="item.isVisible">
                <div class="spoiler-list">
                    <NuxtCheckbox
                        v-for="i in item.filter"
                        :item="i"
                        :key="i.id"
                        @click="filterStore.changeUserFilters({ id: item.id, select: i.id })"
                        :value="userFilters.find(e => e.id === item.id)?.selected.includes(i.id)"
                        ref="filterParams"/>
                </div>
            </NuxtSpoiler>
        </div>
        <div class="filter-buttons">
            <NuxtButton type="submit" style="width: 100%;">Показать</NuxtButton>
        </div> 
    </form>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { Filter } from '~/types/page';
    import { filterStore } from "~/store/filter";

    export default {
        data: () => ({
            filterStore: filterStore(),
            filters: [] as Array<Filter>,
            userFilters: [] as Array<{id: number, selected: Array<number>}>,
        }),
        props: {
            isUseFilterByLoad: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            filterOptions: {
                type: Array as PropType<Array<{ key: string, value: string }>>,
                default: []
            },
            redirect: {
                type: String as PropType<string>,
                default: ''
            },
        },
        async mounted() {
            let filters = useCookie('search').value;
            let options = useCookie('options').value;
            this.$emit('result', await this.filterStore.setup((filters) ? filters : '', (options) ? options : null, this.isUseFilterByLoad));
            this.filters = this.filterStore.defaultFilters;
            this.userFilters = this.filterStore.userFilters;
        },
        methods: {
            async submit() {
                this.$emit('result', await this.filterStore.changeOptions([{ key: 'start', value: 0 }]));
                (this.redirect !== '') && this.$router.push(this.redirect);
                window.scrollTo({top: 0, behavior: 'smooth'});
            },
        },
    }
</script>

<style scoped>
    .filter-container {
        width: 100%;
        height: 100%;
        background-color: var(--default-color-white);
        display: flex;
        flex-direction: column;
        align-items: center;
        position: relative;
        z-index: 1000000000;
    }

    .icon {
        width: 25px;
        height: 25px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
        position: absolute;
        top: 4px;
        right: 4px;
        display: none;
    }

    .icon-close {
        background-image: var(--icon-close);
    }

    .filter {
        width: 90%;
    }

    .filter-buttons {
        width: 100%;
        padding: 20px;
        display: flex;
        justify-content: space-around;
        gap: 10px;
    }

    .spoiler-list {
        padding-bottom: 10px;
    }
    
    @media (max-width: 1400px) {
        .icon {
            display: block;
        }
    }
</style>
