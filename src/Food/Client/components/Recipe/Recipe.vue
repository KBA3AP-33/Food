<template>
    <div class="recipe">
        <h1 class="recipe-name">{{ currentRecipe.name }}</h1>
        <RecipeFace
            @print="$emit('getPrinter')"
            @remove="$emit('remove')"
            @changeFavourite="$emit('changeFavourite')"
            @changeRating="$emit('changeRating', $event)"
            @changeVisibleFilter="$emit('changeVisibleFilter')"
            @transitionPage="$router.push(`../recipes/update/${currentRecipe.id}`)"
            @changeModalComponent="$emit('changeModalComponent', $event)"
            :face="{
                image: currentRecipe.image,
                author: currentRecipe.author,
                created: currentRecipe.created,
                rating: currentRecipe.rating,
                favourite: favourite,
                rate,
            }"
            :is-read-only="isReadOnly"/>
        <RecipeDescription :description="{ time: currentRecipe.time, personCount: currentRecipe.personCount, description: currentRecipe.description }"/>
        <RecipeIngredientList :ingredients="currentRecipe?.ingredients"/>
        <RecipeInstruction :points="currentRecipe.plan"/>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { Recipe } from '~/types';

    export default {
        props: {
            currentRecipe: {
                type: Object as PropType<Recipe>,
                required: true
            },
            isReadOnly: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            favourite: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            rate: {
                type: Number as PropType<number>,
                default: 0
            },
        },
    }
</script>

<style scoped>
    .recipe div {
        width: 100%;
        border-bottom: 1px solid var(--default-color-gray);
        padding: 15px 0;
    }

    .recipe.print div:nth-child(2) {
        border: none;
    }

    .recipe-name {
        font-size: 1.5rem;
    }

    .recipe div:last-child {
        border: none;
        padding-bottom: 0;
    }

    @media (max-width: 992px) {
        .recipe-name {
            text-align: center;
        }
    }
</style>
