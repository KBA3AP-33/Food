<template>
    <div class="instruction">
        <h2 class="instruction-header">Пошаговый рецепт</h2>
        <div class="instruction-points" v-for="(point, index) in points" :key="index">
            <div class="instruction-point-name"># шаг {{ index + 1 }}/{{ points.length }}</div>
            <div class="instruction-point-description">{{ point }}</div>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        props: {
            points: {
                type: Array as PropType<Array<string>>,
                default: []
            },
        },
    }
</script>

<style scoped>
    .instruction-header {
        font-size: 1.5rem;
        margin-bottom: 1.2rem;
    }

    .instruction-points:not(:last-child) {
        margin-bottom: 15px;
    }

    .instruction-point-name {
        font-size: 1.25rem;
        font-weight: 700;
        margin-bottom: 10px;
    }

    .instruction-point-description {
        font-size: .95rem;
        line-height: 1.2;
    }
</style>
