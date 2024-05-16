<template>
    <div class="points">
        <div class="point" v-for="(point, index) in points" :key="point.id">
            <NuxtTextArea :id="`point-${index}`" :placeholder="`Пункт ${index + 1}`" ref="textarea" :value="point.point"/>
            <div class="icon-container">
                <div class="icon icon-trash" @click="remove(point.id)"></div>
            </div>
        </div>
        <div class="points-buttons">
            <NuxtButton @click="create" type="button">Добавить</NuxtButton>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            points: [] as Array<{ id: number, point: string }>
        }),
        props: {
            startPoints: {
                type: Array as PropType<Array<string> | undefined>,
                default: []
            }
        },
        mounted() {
            if (typeof(this.startPoints) !== 'undefined') {
                this.points = this.startPoints.map((e, i) => ({ id: i + 1, point: e }));
            }
        },
        methods: {
            create() {
                this.points.push({
                    id: this.points[this.points.length - 1].id + 1,
                    point: '',
                });
            },
            remove(id: number) {
                if (this.points.length > 1) {
                    this.points = this.points.filter(e => e.id !== id);
                }
            }
        },
    }
</script>

<style scoped>
    .points {
        display: flex;
        flex-direction: column;
        justify-content: center;
        gap: 10px;
    }

    .point {
        display: grid;
        grid-template-columns: auto 5%;
        gap: 10px;
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


    .points-buttons {
        display: flex; 
        justify-content: flex-end;
    }

    @media (max-width: 425px) {
        .point {
            grid-template-columns: auto 10%;
        }
    }
</style>
