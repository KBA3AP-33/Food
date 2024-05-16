<template>
    <div class="ingredients">
        <div class="ingredient" v-for="(ingredient, index) in ingredients" :key="ingredient.id">
            <NuxtInputText
                :id="`ingredient-name-${index}`"
                ref="ingredient"
                class="ingredient-param"
                :settings="{
                    placeholder: 'Ингредиент',
                    list: 'ingredients',
                    value: (ingredient.ingredientName) ? ingredient.ingredientName  : ingredient.ingredient?.name
                }"/>
            <NuxtInputNumber :id="`ingredient-count-${index}`" class="ingredient-param" placeholder="Кол-во" ref="count" :min="1" :value="`${ingredient.count}`"/>
            <NuxtInputText
                :id="`ingredient-units-${index}`"
                ref="units"
                class="ingredient-param"
                :settings="{
                    placeholder: 'Ед. изм.',
                    list: 'units',
                    value: ingredient.units
                }"/>       
            <div class="icon-container">
                <div class="icon icon-trash" @click="remove(ingredient.id)"></div>
            </div>
        </div>
        <div class="ingredients-buttons">
            <NuxtButton @click="create" type="button">Добавить</NuxtButton>
        </div>
        <slot></slot>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { IngredientCount } from '~/types';

    export default {
        data: () => ({
            ingredients: [{ id: 1, ingredient: null, ingredientName: '', count: 0, units: '' }] as Array<IngredientCount>,
        }),
        props: {
            startIngredients: {
                type: Array as PropType<Array<IngredientCount>>,
                default: []
            },
        },
        mounted() {
            this.ingredients = (this.startIngredients) ? this.startIngredients : this.ingredients;
        },
        methods: {
            create() {
                this.ingredients.push({
                    id: this.ingredients[this.ingredients.length - 1].id + 1,
                    ingredient: null,
                    ingredientName: '',
                    count: 0,
                    units: '' 
                });
            },
            remove(id: number) {
                if (this.ingredients.length > 1) {
                    this.ingredients = this.ingredients.filter(e => e.id !== id);
                }
            }
        },
    }
</script>

<style scoped>
    .ingredients {
        display: flex;
        flex-direction: column;
        justify-content: center;
        gap: 10px;
    }

    .ingredient {
        display: grid;
        grid-template-columns: auto 18% 18% 5%;
        gap: 10px;
    }

    .ingredient-param {
        min-width: 50px;
    }

    .icon-container {
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .icon {
        width: 25px;
        height: 25px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-trash {
        background-image: var(--icon-trash);
    }

    .icon-trash:hover {
        background-image: var(--icon-trash-hover);
    }

    .ingredients-buttons {
        display: flex; 
        justify-content: flex-end;
    }
 
    @media (max-width: 425px) {
        .ingredient {
            grid-template-columns: auto 18% 18% 10%;
        }
    }
</style>
