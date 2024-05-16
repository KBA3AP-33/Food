<template>
    <div class="checkbox-container" @click="changeValue">
        <input type="checkbox" :name="item.name" :id="item.name" v-model="currentValue" hidden>
        <label :for="item.name" class="header" @click.stop>{{ item.name }}</label>
        <div class="checkbox"></div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { CheckboxItemProp } from '~/types/page';

    export default {
        data() {
            return {
                currentValue: false,
            };
        },
        props: {
            item: {
                type: Object as PropType<CheckboxItemProp>,
                required: true,
            },
            value: {
                type: Boolean as PropType<boolean>,
                default: false,
            },
        },
        mounted() {
            this.currentValue = this.value;
        },
        methods: {
            changeValue() {
                this.currentValue = !this.currentValue;
            }
        },
    }
</script>

<style scoped>
    .checkbox-container {
        display: flex;
        flex-direction: row-reverse;
        justify-content: flex-end;
        align-items: center;
        gap: 10px;
        flex-shrink: 0;
        padding: 5px 15px;
    }

    .checkbox {
        width: 16px;
        height: 16px;
        border: 1px solid var(--default-color-gray);
        border-radius: 2px;
        flex-shrink: 0;
        position: relative;
        cursor: pointer;
        transition: all .3s;
    }

    input[type='checkbox']:checked ~ .checkbox {
        background-color: var(--default-color-red);
    }

    input[type='checkbox']:checked ~ .checkbox::after {
        content: '';
        width: 8px;
        height: 4px;
        position: absolute;
        top: 3px;
        left: 2px;
        border: 2px solid var(--default-color-white);
        border-top: none;
        border-right: none;
        background: transparent;
        transform: rotate(-45deg);
    }

    .header {
        font-size: .95rem;
        cursor: pointer;
    }

    .checkbox:hover,
    .header:hover ~ .checkbox {
        border-color: var(--default-color-black);
    }
</style>
