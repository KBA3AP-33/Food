<template>
    <div class="catalog-item">
        <NuxtImage class="catalog-item-image" :settings="{ size: { x: 3, y: 2 }, image: item.image }"/>
        <div class="title">{{ item.name }}</div>
        <CatalogItemAuthor :author="item.author" :created="item.created"/>
        <CatalogItemRating :rating="item.rating" :is-like="isFavourite" @changeFavourite="$emit('changeFavourite')"/>
        <div v-if="getRibbon" class="ribbon">{{ getRibbon }}</div>
    </div>
</template>

<script lang="ts">
    import { ribbons } from '~/data';
    import type { PropType } from 'vue';
    import type { CatalogRecipe } from '~/types';

    export default {
        props: {
            isFavourite: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            item: {
                type: Object as PropType<CatalogRecipe>,
                required: true
            },
        },
        computed: {
            getRibbon() {
                let ribbon = ribbons.find(e => e.recipeId === this.item.id);
                if (ribbon) return ribbon.ribbons[ribbon.ribbons.length - 1];
                if (this.item.status && this.item.status.id > 1) return `${this.item.status?.name}`;            
                if (new Date(this.item.created) >= new Date(new Date().valueOf() - (1000 * 24 * 3600 * 30))) return 'Новое';              
                if (this.item.rating >= 4.5) return 'Популярное';
                return '';
            },
        },
    }
</script>

<style scoped>
    .catalog-item {
        width: 17.5rem;
        height: 28rem;
        background-color: var(--default-color-white);
        box-shadow: 0 0.07em 0.2em rgba(0,0,0,0.25);
        border: 1px solid var(--default-color-red);
        border-radius: 5px;
        display: grid;
        grid-template-rows: 185px auto 10% 10%;
        flex-shrink: 0;
        position: relative;
        overflow: hidden;
        cursor: pointer;
    }

    .catalog-item:hover {
        box-shadow: 0 0.07em 1em rgba(0,0,0,0.25);
    }

    .title {
        color: var(--default-color-black);
        line-height: 1.25rem;
        font-size: 1rem;
        font-weight: 700;
        padding: 10px 20px 0 20px;
        overflow: hidden;
    }

    .catalog-item-image {
        border: none;
        border-bottom: 1px solid var(--default-color-red);
    }

    .ribbon {
        width: 200px;
        padding: 3px;
        font-size: 1rem;
        position: absolute;
        text-align: center;
        color: var(--default-color-white);
        background-color: var(--default-color-red);
        bottom: 40px;
	    right: -50px;
        box-shadow: 0 10px 10px rgba(0,0,0,0.25);
        transform: rotate(-45deg);
    }
</style>
