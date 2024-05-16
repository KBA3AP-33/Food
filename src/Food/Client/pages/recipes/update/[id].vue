<template>
    <RecipeEditor :recipe="recipeEdit"/>
</template>

<script lang="ts" setup>
    import { authenticationStore } from "~/store/authentication";
    import { filterStore } from "~/store/filter";
    import type { Recipe, RecipeEdit } from "~/types";

    definePageMeta({
        validate: ({ params }) => /^\d+$/.test(params.id as string),
    })

    const filter = filterStore();
    const recipe = await filter.getRecipe(Number(useRoute().params.id), false, false) as Recipe;
    const recipeEdit = {
        id: recipe.id,
        created: recipe.created,
        name: recipe.name,
        description: recipe.description,
        image: recipe.image,
        author: recipe.author,
        time: recipe.time,
        personCount: recipe.personCount,
        rating: recipe.rating,
        ingredients: recipe.ingredients,
        plan: recipe.plan,
        statusId: recipe.status.id,
        timeCategoryId: recipe.timeCategory.id,
        categoryId: recipe.category.id,
        kitchenId: recipe.kitchen.id,
    } as RecipeEdit;
</script>

<script lang="ts">
    export default {
        data: () => ({
            authentication: authenticationStore(),
        }),
        emits: ['changeModalComponent', 'changeVisibleLoader'],
        mounted() {
            this.$emit('changeVisibleLoader', false);
        },
        watch: {
            'authentication.user': function(nextValue) {
                if (nextValue === null) {
                    this.$router.push('../');
                }
            }
        }
    }
</script>
