<template>
    <div class="recipe-description">
        <div class="icons">
            <div class="icon">
                <div class="icon-recipe icon-clock"></div>
                <div class="icons-info">
                    <div>Приготовление</div>
                    <div><b>{{ getTime }}</b></div>
                </div>
            </div>
            <div class="icon">
                <div class="icon-recipe icon-person"></div>
                <div class="icons-info">
                    <div>Рецепт на:</div>
                    <div><b>{{ getPersonCount }}</b></div>
                </div>
            </div>
        </div>
        <div v-if="description.description">
            <h2>Описание</h2>
            <p class="description">{{ description.description }}</p>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { RecipeDescription } from '~/types/page';

    export default {
        props: {
            description: {
                type: Object as PropType<RecipeDescription>,
                default: { time: 10, personCount: 1, description: '' }
            }
        },
        methods: {
            getCorrectWriting(num: number, result: { 1: string, 2: string, 5: string }) {
                let second = num % 10;
                let first = (num % 100 - second) / 10;

                if (second === 1 && first !== 1) return result?.[1];
                else if (second > 1 && second < 5 && first !== 1) return result?.[2];
                else return result?.[5];
            },
        },
        computed: {
            getTime() {
                let hours = Math.floor(this.description.time / 60);
                let minutes = this.description.time % 60;
                let hh = { 1: 'час', 2: 'часа', 5: 'часов' };
                let mm = { 1: 'минута', 2: 'минуты', 5: 'минут' };

                if (hours < 1) return `${minutes} ${this.getCorrectWriting(minutes, mm)}`;
                if (minutes < 1) return `${hours} ${this.getCorrectWriting(hours, hh)}`;
                return `${hours} ${this.getCorrectWriting(hours, hh)} ${minutes} ${this.getCorrectWriting(minutes, mm)}`;
            },
            getPersonCount() {
                return `${this.description.personCount} ${this.getCorrectWriting(this.description.personCount, { 1: 'персону', 2: 'персоны', 5: 'персон' })}`;
            },
        }
    }
</script>

<style scoped>
    .recipe-description {
        font-size: 1.1rem;
    }

    .description-icon {
        display: inline-block;
        font-size: 30px;
        color: var(--default-color-red);
    }

    .icons {
        display: flex;
        align-items: center;
        flex-wrap: nowrap;
        gap: 30px;
        margin-bottom: 20px;
    }

    .icon-recipe {
        width: 40px;
        height: 40px;
        background-size: cover;  
        background-repeat: no-repeat;
    }

    .icon {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .icon-clock {
        background-image: var(--icon-clock);
    }

    .icon-person {
        background-image: var(--icon-person-red);
    }

    .icons-info {
        font-size: .9rem;
        display: flex;
        flex-direction: column;
        justify-content: space-around;
    }

    .description {
        padding: 15px 0;
    }
</style>
