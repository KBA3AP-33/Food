<template>
    <table class="catalog" ref="catalog">
        <tbody>
            <TransitionGroup name="list" v-if="catalogItems.recipes?.length">
                <tr v-for="item in catalogItems.recipes" :key="item.id">
                    <td>
                        <NuxtImage
                        class="item-image"
                        :settings="{ image: item.image, size: { x: 1, y: 1 }, isIncrease: true }"
                        @changeModalComponent="$emit('changeModalComponent', $event)"/>
                    </td>
                    <td><div class="user-name">{{ item.name }}</div></td>
                    <td class="item-info date">{{ new Date(item.created).toLocaleDateString() }}</td>
                    <td class="item-info user">{{ item.author.userName }}</td>
                    <td class="item-info status">{{ item.status?.name }}</td>
                    <td>
                        <div>
                            <div class="icon icon-checkmark-circle" title="Одобрить" @click="changeRecipe(item, true)"></div>
                        </div>
                    </td>
                    <td>
                        <div>
                            <div
                                title="Отклонить"
                                class="icon icon-close-circle"
                                @click="changeRecipe(item, false)">
                            </div>
                        </div>
                    </td>
                    <td>
                        <a :href="`/recipes/${item.id}?authorize=true&admin=true`" target="_blank">
                            <div class="icon icon-link" title="Перейти"></div>
                        </a>
                    </td>
                </tr>
            </TransitionGroup>
            <tr colspan="8" v-else>
                <p class="empty-list">Список пуст!</p>
            </tr>
        </tbody>
    </table>
    <NuxtPagination :pages-count="getPageCount" :current-page="getCurrentPage" @changePage="$emit('changePage', $event)" class="page-pagination"/>
</template>

<script lang="ts">
    import { limit } from '~/data';
    import { filterStore } from "~/store/filter";
    import type { Catalog, CatalogRecipe } from '~/types';

    export default {
        data: () => ({
            selectRecipeId: 0,
            filter: filterStore(),
            catalogItems: {} as Catalog,
        }),
        async mounted() {
            this.catalogItems = await this.$authHost(`api/catalog/authorize?statuses=2&statuses=4`) as Catalog;
        },
        emits: ['changeModalComponent', 'changePage', 'changeMessage'],
        methods: {
            async changeRecipe(recipe: CatalogRecipe, isApprove: boolean) {
                (isApprove)
                    ? (recipe.status?.id === 2)
                        ? await this.approveRecipe(recipe.id, 1)
                        : await this.rejectRecipe(recipe.id)
                    : (recipe.status?.id === 2)
                        ? await this.approveRecipe(recipe.id, 3)
                        : await this.approveRecipe(recipe.id, 1)
            },
            async approveRecipe(id: number, statusId: number) {
                let result = await this.$authHost(`api/catalog/status/${id}`, 'PUT', {}, { statusId });
                this.catalogItems = { ...this.catalogItems, recipes: this.catalogItems.recipes.filter(e => e.id !== result) }
            },
            async rejectRecipe(id: number) {
                let result = await this.$authHost(`api/catalog/${id}`, 'DELETE');
                this.catalogItems = { ...this.catalogItems, recipes: this.catalogItems.recipes.filter(e => e.id !== result) }
            },
        },
        computed: {
            getPageCount() {
                return Math.ceil(this.catalogItems.count / limit);
            },
            getCurrentPage() {
                return (this.catalogItems.start / limit) + 1;
            },
        }
    }
</script>

<style scoped>
    .catalog {
        width: 100%;
        border-collapse: collapse; 
    }

    .catalog tr:nth-child(odd){
        background: var(--default-color-white);
    }

    .catalog tr:nth-child(even){
        background: var(--scrollbar-background);
    }

    .catalog tr td:first-child {
        border-left: none;
    }
    .catalog tr td:last-child {
        border-right: none;
    }

    .catalog td {
        border: 1px solid var(--default-color-lightgray);
        padding: 5px;
    }

    .catalog tr:first-child td {
        border-top: none;
    }

    .catalog td:nth-child(1) {
        width: 75px;
        height: 75px;
        aspect-ratio: 1 / 1;
    }

    .catalog td:nth-child(2) {
        font-weight: 600;
    }

    .catalog td:nth-child(2) * {
        -webkit-line-clamp: 3;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        overflow: hidden;
    }

    .catalog td:nth-child(6),
    .catalog td:nth-child(7),
    .catalog td:nth-child(8) {
        width: 5%;
    }

    .catalog td:nth-child(6) *,
    .catalog td:nth-child(7) *,
    .catalog td:nth-child(8) * {
        display: flex;
        justify-content: center;
    }

    .item-info {
        width: 9%;
        text-align: center;
    }

    .item-image {
        background-size: 25%;
        border-radius: 50%;
    }

    .comment,
    .status,
    .user {
        width: 15%;
    }

    .icon {
        width: 25px;
        height: 25px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-checkmark-circle {
        background-image: var(--icon-checkmark-circle);
    }

    .icon-close-circle {
        background-image: var(--icon-close-circle);  
    }

    .icon-link {
        background-image: var(--icon-link);
    }

    .icon-checkmark-circle:hover {
        background-image: var(--icon-checkmark-circle-hover);
    }

    .icon-close-circle:hover {
        background-image: var(--icon-close-circle-hover);  
    }

    .icon-link:hover {
        background-image: var(--icon-link-hover);
    }

    .page-pagination {
        border-top: none;
        padding: 10px 0;
    }

    .empty-list {
        font-size: 1rem;
        text-align: center;
        background-color: var(--default-color-white);
        padding: 10px 0;
    }

    .list-enter-active,
    .list-leave-active {
        transition: all 0.5s ease;
    }

    .list-enter-from,
    .list-leave-to {
        opacity: 0;
        transform: translateX(30px);
    }

    @media (max-width: 672px) {
        .catalog {
            font-size: 10px;
        }

        .icon {
            width: 15px;
            height: 15px;
        }

        .catalog td:nth-child(1) {
            width: 50px;
            height: 50px;
        }
    }

    @media (max-width: 425px) {
        .catalog {
            font-size: 8px;
        }

        .catalog td:nth-child(1) {
            width: 40px;
            height: 40px;
        }

        .user-name {
            font-size: 10px;
        }

        .date,
        .user {
            display: none;
        }
    }
</style>
