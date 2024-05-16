<template>
    <Title>{{ (recipe.id > 0) ? "Изменение" : "Создание" }}</Title>
    <div>
        <NuxtProgressbar @changeProgress="update" class="editor-progress"/>
        <div class="create-page-content">
            <div class="editor">
                <form class="recipe-edit" @submit.prevent.stop="save" novalidate>
                    <h2 class="header">{{ (recipe.id > 0) ? "Изменение" : "Создание" }}</h2>
                    <div class="recipe-name">
                        <NuxtInputText id="recipe-name" ref="name" :settings="{ placeholder: 'Название*', minlength: 5, maxlength: 100 }"/>
                    </div>
                    <div class="recipe-hidden-info">
                        <div class="recipe-image">
                            <NuxtInputFile ref="image"/>
                        </div>
                        <div class="recipe-params">
                            <div class="recipe-params-numbers">
                                <NuxtInputNumber id="recipe-time" ref="time" :settings="{ placeholder: 'Время (мин.)', max: 1440, min: 10 }"/>
                                <NuxtInputNumber id="recipe-count" ref="personCount" :settings="{ placeholder: 'Кол-во персон', max: 10, min: 1 }"/>
                            </div>
                            <NuxtDefaultSelect id="timeCategory" :data="timeCategories" ref="timeCategory"/>
                            <NuxtDefaultSelect id="category" :data="categories" ref="category"/>
                            <NuxtDefaultSelect id="kitchen" :data="kitchens" ref="kitchen"/>
                        </div>  
                    </div>
                    <div class="recipe-description">
                        <NuxtTextArea id="description" placeholder="Добавить описание" ref="description"/>
                    </div>
                    <div class="recipe-ingredients">
                        <h3 class="ingredient-header">Ингредиенты*</h3>
                        <RecipeCreateIngredient ref="ingredients" :start-ingredients="recipe?.ingredients">
                            <datalist id="ingredients">
                                <option v-for="ingredient in ingredients" :key="ingredient.id" :value="ingredient.name"/>
                            </datalist>
                            <datalist id="units">
                                <option value="кг."/>
                                <option value="шт."/>
                                <option value="зубчик"/>
                                <option value="веточка"/>
                                <option value="л."/>
                                <option value="мл."/>
                                <option value="ст. л."/>
                                <option value="ч. л."/>
                                <option value="горсть"/>
                                <option value="г."/>
                            </datalist>
                        </RecipeCreateIngredient>
                    </div>
                    <div class="recipe-plan">
                        <h3 class="ingredient-header">Рецепт*</h3>
                        <RecipeCreatePlan ref="plan" :start-points="recipe?.plan"/>
                    </div>
                    <p v-if="errors.length" class="error">Ошибка: {{ errors.join(', ') }}</p>
                    <div class="recipe-submit">
                        <p class="ps">Рецепт будет опубликован после прохождения модерации.</p>
                        <NuxtButton class="red" type="submit">Опубликовать</NuxtButton>
                    </div>
                </form>
            </div>
            <div class="preview">
                <h2 class="header">Предпросмотр</h2>
                <Recipe :current-recipe="currentRecipe" :is-read-only="true"/>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { filterStore } from '~/store/filter';
    import type { PropType } from 'vue';
    import type { Category, Filters, Recipe, RecipeEdit } from '~/types';
    import type { DefaultSelectItemProp } from '~/types/page';


    export default {
        data: () => ({
            filter: filterStore(),
            ingredients: [] as Array<Category>,
            categories: [] as Array<DefaultSelectItemProp>,
            kitchens: [] as Array<DefaultSelectItemProp>,
            timeCategories: [] as Array<DefaultSelectItemProp>,
            statuses: [] as Array<DefaultSelectItemProp>,
            errors: [] as Array<string>,
            currentRecipe: {} as Recipe,
        }),
        props: {
            recipe: {
                type: Object as PropType<RecipeEdit>,
                required: true
            },
        },
        async mounted() {
            let filters = await this.filter.getFilter() as Filters;

            this.ingredients = filters.ingredients;
            this.categories = filters.categories.map(e => ({ text: e.name, value: e.id }));
            this.kitchens = filters.kitchens.map(e => ({ text: e.name, value: e.id }));
            this.timeCategories = filters.timeCategories.map(e => ({ text: e.name, value: e.id }));
            this.statuses = filters.statuses.map(e => ({ text: e.name, value: e.id }));
            this.setup();
            this.update();
        },
        methods: {
            setup() {
                if (this.recipe !== null) {
                    (this.$refs.name as any).currentValue = this.recipe.name;
                    (this.$refs.image as any).image = this.recipe.image;
                    (this.$refs.time as any).currentValue = this.recipe.time;
                    (this.$refs.personCount as any).currentValue = this.recipe.personCount;
                    (this.$refs.description as any).text = this.recipe.description;
                    (this.$refs.category as any).selected = this.recipe.categoryId;
                    (this.$refs.kitchen as any).selected = this.recipe.kitchenId;
                    (this.$refs.timeCategory as any).selected = this.recipe.timeCategoryId;
                    this.recipe.plan.forEach((e: string, i: number) => (this.$refs.plan as any).$refs.textarea[i].text = e);
                    this.recipe.ingredients.forEach((e, i) => {
                        (this.$refs.ingredients as any).$refs.ingredient[i].currentValue = (e.ingredient) ? e.ingredient.name : e.ingredientName;
                        (this.$refs.ingredients as any).$refs.count[i].currentValue = e.count;
                        (this.$refs.ingredients as any).$refs.units[i].currentValue = e.units;
                    });
                }
            },
            update() {
                let name: string = (this.$refs.name as any).currentValue;
                let image: string = (this.$refs.image as any).image;
                let time: string = (this.$refs.time as any).currentValue;
                let personCount: string = (this.$refs.personCount as any).currentValue;
                let description: string = (this.$refs.description as any).text;
                let timeCategory = (this.$refs.timeCategory as any).selected;
                let kitchen = (this.$refs.kitchen as any).selected;
                let category = (this.$refs.category as any).selected;
                let ingredients = [
                    (this.$refs.ingredients as any).$refs.ingredient.map((e: any) => e.currentValue),
                    (this.$refs.ingredients as any).$refs.count.map((e: any) => e.currentValue),
                    (this.$refs.ingredients as any).$refs.units.map((e: any) => e.currentValue),
                ];
                let plan = (this.$refs.plan as any).$refs.textarea.map((e: any, i: number) => ({ id: i + 1, description: e.text }));
                ingredients = ingredients[0].map((e: any, i: number) => ({ id: i + 1, name: e, count: ingredients[1][i], units: ingredients[2][i] }));

                this.currentRecipe = {
                    id: this.recipe!.id,
                    created: this.recipe.created,
                    name, 
                    description,
                    image,
                    author: this.recipe.author,
                    time: Number(time),
                    personCount: Number(personCount),
                    rating: this.recipe!.rating,
                    ingredients: (ingredients.length === 1 && (ingredients[0].name == '' || ingredients[0].count === '' || ingredients[0].units === ''))
                        ? [{ id: 1, ingredientName: 'Не заполнено', ingredient: null, count: 0, units: 'кг.' }]
                        : ingredients.filter((e: any) => e.name !== '' && e.count !== 0 && e.units !== '')
                            .map((e, i) => ({ id: i + 1, ingredientName: e.name, ingredient: null, count: e.count, units: e.units })),
                    plan: ((plan.length === 1 && plan[0].description === '') ? [{ id: 1, description: 'Не заполнено'}] : plan.filter((e: any) => e.description !== ''))
                        .map((e: any) => e.description),
                    status: { id: 1, name: '' },
                    timeCategory: { id: timeCategory, name: '' },
                    kitchen: { id: kitchen, name: '' },
                    category: { id: category, name: '' },
                    userRatings: []
                };
            },
            async save() {
                this.update();
                this.errors = [];
                if (this.currentRecipe.name === '' || this.currentRecipe.name === 'Не заполнено') this.errors.push('Заполните название');
                if (this.currentRecipe.ingredients.length === 1 && (this.currentRecipe.ingredients[0].ingredientName == '' || this.currentRecipe.ingredients[0].count === 0 || this.currentRecipe.ingredients[0].units === '')) this.errors.push('Добавьте ингредиенты');
                if (this.currentRecipe.plan.length === 1 && (this.currentRecipe.plan[0] === '' || this.currentRecipe.plan[0] === 'Не заполнено')) this.errors.push('Добавьте хотя бы 1 пункт рецепта');
                if (this.errors.length) return;

                let recipe = {
                    id: this.currentRecipe.id,
                    name: this.currentRecipe.name,
                    description: this.currentRecipe.description,
                    image: this.currentRecipe.image,
                    author: this.currentRecipe.author,
                    time: this.currentRecipe.time,
                    personCount: this.currentRecipe.personCount,
                    categoryId: this.currentRecipe.category.id,
                    timeCategoryId: this.currentRecipe.timeCategory.id,
                    kitchenId: this.currentRecipe.kitchen.id,
                    statusId: this.currentRecipe.status.id,
                    ingredients: this.currentRecipe.ingredients
                        .map(e => ({ name: e.ingredientName, count: e.count, units: e.units })),
                    plan: this.currentRecipe.plan,
                };

                let url = (recipe.id > 0) ? `api/catalog/${recipe.id}` : 'api/catalog';
                let result = await this.$authHost(url, (recipe.id > 0) ? 'PUT' : 'POST', {}, recipe) as number;
                if (result > 0) {
                    return this.$router.go(-1);
                }
                this.errors.push('Произошла ошибка! Повторите позже.');
            },
        },
    }
</script>

<style scoped>
    .create-page-content {
        width: var(--content-width);
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        position: relative;
    }

    .editor {
        padding: 10px;
        border-right: 1px solid var(--default-color-gray);
    }

    .editor-progress {
        position: sticky;
        top: 0;
        left: 0;
        z-index: 100000000000000000;
    }

    .recipe-edit {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
    
    .header {
        padding-bottom: 10px;
        margin-bottom: 10px;
        border-bottom: 1px solid var(--default-color-gray);
    }

    .recipe-hidden-info {
        display: flex;
        gap: 10px;
    }

    .recipe-image,
    .recipe-params {
        width: 50%;
    }

    .recipe-params {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }

    .recipe-params * {
        min-width: 50px;
    }

    .recipe-params-numbers {
        display: flex;
        gap: 10px;
    }

    .recipe-submit {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        gap: 10px;
    }

    .error {
        color: var(--default-error-color);
    }

    .ps {
        color: var(--default-date-color);
    }

    .preview {
        padding: 10px;
    }

    @media (max-width: 992px) {
        .create-page-content {
            grid-template-columns: repeat(1, 1fr);
            grid-template-rows: repeat(2, 1fr);
        }

        .editor {
            border: none;
            border-bottom: 1px solid var(--default-color-gray);
        }
    }

    @media (max-width: 576px) {
        .recipe-hidden-info {
            flex-direction: column;
        }

        .recipe-image,
        .recipe-params {
            width: 100%;
        }
    }
</style>
