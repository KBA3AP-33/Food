<template>
    <div class="ingredients">
        <h2 class="ingredient-header">Ингредиенты</h2>
        <RecipeIngredient v-for="ingredient in ingredients" :key="ingredient.id" :ingredient="ingredient"/>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { IngredientCount } from '~/types';

    export default {
        props: {
            ingredients: {
                type: Array as PropType<Array<IngredientCount>>,
                default: []
            },
        },
    }
</script>

<style scoped>
    .ingredient-header {
        font-size: 1.5rem;
        margin-bottom: 1.2rem;
    }
</style>
