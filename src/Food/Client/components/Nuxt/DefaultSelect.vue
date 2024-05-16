<template>
    <div class="select-container">
        <select class="select" :id="id" v-model="selected">
            <option v-for="item in data" :key="item.value" :value="item.value">{{ item.text }}</option>
        </select>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { DefaultSelectItemProp } from '~/types/page';

    export default {
        data: () => ({
            selected: 1,
        }),
        props: {
            id: {
                type: String as PropType<string>,
                required: true
            },
            data: {
                type: Array as PropType<Array<DefaultSelectItemProp>>,
                required: true
            },
            value: {
                type: Number as PropType<number>,
                default: 1
            },
        },
        mounted() {
            this.selected = (this.value) ? this.value : this.data[0]?.value;
        },
    }
</script>

<style scoped>
    .select-container {
        width: 100%;
        height: 2rem;
    }

    .select {
        width: 100%;
        height: 100%;
        font-weight: 400;
        font-size: .95rem;
        background-color: var(--default-color-white);
        border: 2px solid var(--default-color-black);
        border-radius: 3px;
        padding: 0 20px;
    }

    @media (max-width: 576px) {
        .select {
            padding: 0 10px;
        }
    }
</style>
