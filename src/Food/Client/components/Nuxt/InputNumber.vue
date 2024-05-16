<template>
    <div class="input-container">
        <input
            :id="id"
            :placeholder="settings.placeholder"
            :min="settings.min"
            :max="settings.max"
            type="number"
            v-model="currentValue"
            required>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { InputNumberProps } from '~/types/page';

    export default {
        data: () => ({
            currentValue: '',
        }),
        props: {
            id: {
                type: String as PropType<string>,
                required: true
            },
            settings: {
                type: Object as PropType<InputNumberProps>,
                default: { placeholder: '', min: 0, max: 1000, value: '' } as InputNumberProps
            }
        },
        mounted() {
            this.currentValue = `${this.settings.value}`;
        },
    }
</script>

<style scoped>
    .input-container {
        width: 100%;
        min-width: 300px;
        height: 2rem;
        display: flex;
        align-items: center;
        gap: 5px;
    }

    input {
        width: 100%;
        height: 100%;
        background-color: var(--default-color-white);
        font-size: .95rem;
        font-weight: 400;
        padding: 0 5px 0 10px;
        border-radius: 3px;
    }

    @media (max-width: 768px) {
        .input-container {
            min-width: 290px;
        }
    }
</style>
