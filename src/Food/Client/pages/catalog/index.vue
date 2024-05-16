<template>
    <div class="page-container">
        <div :class="['page-left-container', isActiveFilter && 'active']">
            <CatalogFilter
                @close="isActiveFilter = false"
                @result="changeData"
                :is-use-filter-by-load="true"/>
        </div>
        <div class="page-center-container">
            <Catalog
                :is-search="true"
                :search-value="filter.options.search"
                :select-value="filter.options.sort"
                :catalog-items="catalogItems"
                :favourites="authentication.user?.favourites"
                @changeVisibleFilter="isActiveFilter = !isActiveFilter"
                @changeFavourite="authentication.changeFavourites($event)"
                @changePage="async (start) => changeData(await filter.changeOptions([{ key: 'start', value: start }], true) as Catalog)"
                @changeSort="async (sort) => changeData(await filter.changeOptions([{ key: 'sort', value: sort }, { key: 'start', value: 0 }]) as Catalog)"
                @changeSearch="async (search) => changeData(await filter.changeOptions([{ key: 'search', value: search }, { key: 'start', value: 0 }]) as Catalog)"/>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { authenticationStore } from "~/store/authentication";
    import { filterStore } from "~/store/filter";
    import type { Catalog } from "~/types";

    useHead({
        titleTemplate: 'EASY-COOK'
    });

    const filter = filterStore();
    const search = useCookie('search');
    const options = useCookie('options');

    let catalogItems = ref((typeof(search.value) !== 'undefined')
        ? ((typeof(options.value) !== 'undefined')
            ? await filter.getRecipes(`${search.value}&${options.value}`) as Catalog
            : await filter.getRecipes(`${search.value}`) as Catalog)
        : await filter.getRecipes('') as Catalog);

    function changeData(recipes: Catalog) {
        catalogItems.value = recipes;
    }  
</script>

<script lang="ts">
    export default {
        data: () => ({
            authentication: authenticationStore(),
            isActiveFilter: false,
        }),
        async mounted() {
            this.$emit('changeVisibleLoader', false);
        },
    }
</script>
