<template>
    <RecipeEditor :recipe="currentRecipeCreate"/>
</template>

<script setup lang="ts">
    import { authenticationStore } from '~/store/authentication';
    import type { Author, RecipeEdit, User } from '~/types';

    useHead({
        titleTemplate: 'Создание',
    })
</script>

<script lang="ts">
    export default {
        data: () => ({
            authentication: authenticationStore(),
            currentRecipeCreate: {
                id: -1,
                name: '',
                description: '',
                created: new Date().toString(),
                time: 10,
                personCount: 1,
                rating: 0,
                author: {},
                ingredients: [{ id: 1, ingredient: null, ingredientName: '', count: 0, units: '' }],
                timeCategoryId: 1,
                categoryId: 1,
                kitchenId: 1,
                plan: ['']
            } as RecipeEdit,
        }),
        emits: ['changeVisibleLoader', 'changeModalComponent'],
        async mounted() {
            let user = (useCookie('user').value as User | null)!;
            this.currentRecipeCreate.author = { id: user.userId, userName: user.userName } as Author
            this.$emit('changeVisibleLoader', false);
        },
        watch: {
            'authentication.user': function(nextValue) {
                if (nextValue === null) {
                    this.$router.push('../');
                }
            }
        },
    }
</script>

<style scoped>
    .create-page {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        position: relative;
    }

    .create-page-content {
        width: var(--content-width);
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        position: relative;
    }

    .recipe-loader {
        width: var(--content-width); 
        height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .editor {
        padding: 10px;
        border-right: 1px solid var(--default-color-gray);
    }

    .recipe-edit {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        grid-template-rows: repeat(7, 1fr);
        gap: 10px;
    }

    .header {
        padding-bottom: 10px;
        margin-bottom: 10px;
        border-bottom: 1px solid var(--default-color-gray);
    }

    .header,
    .recipe-name {
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .recipe-image,
    .recipe-params {
        grid-row-start: 3;
        grid-row-end: 8;
    }

    .recipe-params {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        grid-auto-rows: min-content;
        gap: 10px;
    }

    .recipe-params * {
        min-width: 50px;
    }


    .recipe-description {
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .recipe-ingredients {
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .ingredient-header {
        font-size: 1.3em;
        margin-bottom: 10px;
    }

    .recipe-plan {
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .error {
        color: var(--default-error-color);
        grid-column-start: 1;
        grid-column-end: 3;
    }

    .ps {
        color: var(--default-date-color);
    }

    .preview {
        padding: 10px;
    }
</style>
