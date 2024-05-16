<template>
    <main>
        <Recipe class="print" :current-recipe="recipe"/>
        <span style="display: none;">{{ isLoad }}</span>
    </main>
</template>

<script setup lang="ts">
    import type { Recipe } from '~/types';
    import { filterStore } from "~/store/filter";

    definePageMeta({
        layout: "empty",
        validate: ({ params }) => /^\d+$/.test(params.id as string),
    });

    const recipe = ref(await filterStore().getRecipe(Number(useRoute().params.id), false, false) as Recipe);
</script>

<script lang="ts">
    export default {
        data: () => ({
            isLoad: false,
        }),
        mounted() {
            this.isLoad = true;
        },
        updated() {
            this.$nextTick(() => {
                window.print();
            });
        },
    }
</script>

<style scoped>
    * {
        -webkit-print-color-adjust: exact !important;
        color-adjust: exact !important;
        print-color-adjust: exact !important;
    }

    main {
        width: 100%;
        display: flex;
        justify-content: center;
    }

    .print {
        width: 800px;
        pointer-events: none;
        user-select: none;
    }
</style>
