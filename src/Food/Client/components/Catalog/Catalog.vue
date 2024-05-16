<template>
    <div :class="[(isSearch) ? 'search-catalog-container' : 'catalog-container']">
        <div>
            <h1 class="header">{{ header }}</h1>
            <div class="search-panel" v-if="isSearch">
                <NuxtSearch
                    class="search-line"
                    :count="catalogItems.count"
                    :search="searchValue"
                    :select="selectValue"
                    @changeSort="$emit('changeSort', $event)"
                    @changeSearch="$emit('changeSearch', $event)"/>
                <div class="search-panel-buttons">
                    <NuxtButton class="red" :class="'filter button-icon'" @click="$emit('changeVisibleFilter')">
                        <div class="icon icon-filter" style="pointer-events: none;"></div>
                        <span class="button-name">Фильтр</span>
                    </NuxtButton>
                    <NuxtButton class="red" :class="'button-icon'" @click="$router.push('recipes/create')">
                        <div class="icon icon-add" style="pointer-events: none;"></div>
                        <span class="button-name">Добавить рецепт</span>
                    </NuxtButton>
                </div>
            </div>
        </div>
        <div class="catalog" v-if="catalogItems.recipes?.length">
            <CatalogItem
                v-for="item in catalogItems.recipes"
                :key="item.id"
                :item="item"
                :is-favourite="favourites.includes(item.id)"
                @changeFavourite="$emit('changeFavourite', item.id)"
                @click="$router.push((isAuthorize && item.status?.id !== 1) ? `../recipes/${item.id}?authorize=true` : `../recipes/${item.id}`)"/>
        </div>
        <div class="empty-catalog" v-else>Список пуст!</div>
        <NuxtPagination :pages-count="getPageCount" :current-page="getCurrentPage" @changePage="changePage" class="page-pagination"/>
    </div>
</template>

<script lang="ts">
    import { limit } from '~/data';
    import type { PropType } from 'vue';
    import type { Catalog } from '~/types';
   
    export default {
        props: {
            isSearch: {
                type: Boolean as PropType<boolean>,
                required: true
            },
            catalogItems: {
                type: Object as PropType<Catalog>,
                required: true
            },
            header: {
                type: String as PropType<string>,
                default: 'Каталог рецептов'
            },
            searchValue: {
                type: String as PropType<string>,
                default: ''
            },
            selectValue: {
                type: Number as PropType<number>,
                default: 0
            },
            favourites: {
                type: Array as PropType<Array<number>>,
                default: []
            },
            isAuthorize: {
                type: Boolean as PropType<boolean>,
                default: false
            }
        },
        methods: {
            changePage(page: number) {
                this.$emit('changePage', (page - 1) * limit);
            },
        },
        computed: {
            getPageCount() {
                return Math.ceil(this.catalogItems.count / limit);
            },
            getCurrentPage() {
                return (this.catalogItems.start / limit) + 1;
            },
        },
    }
</script>

<style scoped>
    .search-catalog-container,
    .catalog-container {
        display: grid;
        grid-template-rows: 120px auto 50px;
        gap: 30px;
    }

    .catalog-container {
        grid-template-rows: 50px auto 50px;
    }

    .empty-catalog {
        font-size: 1rem;
        text-align: center;
    }

    .catalog {
        width: 100%;
        display: grid;
        grid-template-columns: repeat(3, 1fr);
        justify-items: center;
        row-gap: 20px;
        padding: 0 10px; 
    }

    .header {
        text-align: center;
        font-size: 2.2rem;
    }

    .search-line {
        width: 80%;
    }

    .search-panel {
        display: flex;
        align-items: flex-start;
        gap: 10px;
        border-bottom: 1px solid var(--default-color-gray);
        padding: 15px 23px;
    }

    .search-panel-buttons {
        display: flex;
        gap: 10px;
    }

    .icon {
        width: 17px;
        height: 17px;
        background-size: cover;  
        background-repeat: no-repeat;
    }

    .icon-add {
        background-image: var(--icon-plus);
    }
    
    .icon-filter {
        background-image: var(--icon-filter-white);
    }

    .button-icon {
        display: flex;
        align-items: center;
        gap: 7px;
    }

    .filter {
        display: none;
    }

    @media (max-width: 1400px) {
        .filter {
            display: flex;
        }
    }

    @media (max-width: 992px) {
        .catalog {
            grid-template-columns: repeat(2, 1fr);
            padding: 0 15px;
        }

        .search-line {
            width: 85%;
        }

        .button-icon {
            padding: 0 11px;
        }

        .button-name {
            display: none;
        }
    }

    @media (max-width: 768px) {
        .header {
            font-size: 1.75rem;
        }

        .search-line {
            width: 80%;
        }
    }

    @media (max-width: 672px) {
        .catalog {
            grid-template-columns: 1fr;
            padding: 0 15px;
        }
    }

    @media (max-width: 576px) {
        .search-panel {
            padding-bottom: 30px;
        }
    }

    @media (max-width: 425px) {
        .header {
            font-size: 1.3rem;
        }

        .search-catalog-container {
            grid-template-rows: 80px auto 50px;
            gap: 15px;
        }
        
        .search-line {
            width: 100%;
        }

        .search-panel {
            padding: 10px 15px;
            display: grid;
            grid-template-columns: 1fr 100px;
            gap: 10px;
        }

        .search-panel-buttons {
            width: 100%;
            justify-content: center;
        }
    }
</style>
