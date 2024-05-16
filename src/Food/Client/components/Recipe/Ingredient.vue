<template>
    <div class="ingredient-container">
        <div class="ingredient-name">{{ (ingredient.ingredient !== null) ? ingredient.ingredient?.name : ingredient.ingredientName }}</div>
        <div class="points-line"></div>
        <div class="ingredient-count">{{ ingredient.count }} {{ ingredient.units }}</div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { IngredientCount } from '~/types';

    export default {
        props: {
            ingredient: {
                type: Object as PropType<IngredientCount>,
                required: true
            },
        },
    }
</script>

<style scoped>
    .ingredient-container {
        width: 100%;
        font-size: .95rem;
        padding: 5px 10px;
        display: flex;
        align-items: flex-end;
        gap: 10px;
    }

    .ingredient-name {
        max-width: 60%;
        text-wrap: nowrap;
    }

    .points-line {
        width: 100%;
        border-bottom: 1px dotted var(--default-color-black);
    }

    .ingredient-count {
        font-weight: 900;
        text-wrap: nowrap;
    }

    @media (max-width: 375px) {
        .ingredient-name {
            text-wrap: wrap;
        } 
    }
</style>
