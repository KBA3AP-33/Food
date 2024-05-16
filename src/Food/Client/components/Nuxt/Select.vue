<template>
    <div class="select-container">
        <p class="select" @click="isOpen = !isOpen">{{ options[select].name }}</p>
        <div :class="['options', (isOpen) && 'open']">
            <ul>
                <li
                    v-for="(option, index) in options"
                    :key="option.id"
                    @click="changeSelect(index)"
                    :style="{ display: (index === select) ? 'none' : '' }">
                    {{ option.name }}
                </li>
            </ul>
            <div class="triangle"></div>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    

    export default {
        data: () => ({
            select: 0,
            isOpen: false
        }),
        props: {
            options: {
                type: Array as PropType<Array<{ id: number, name: string }>>,
                required: true
            },
            selectValue: {
                type: Number as PropType<number>,
                default: 0
            }
        },
        mounted() {
            this.select = this.selectValue;
        },
        methods: {
            changeSelect(nextValue: number) {
                this.select = nextValue;
                this.isOpen = false;
                this.$emit('changeSelect', nextValue);
            },
        },
        watch: {
            selectValue(nextValue) {
                this.select = nextValue;  
            },
        },
    }
</script>

<style scoped>
    ul {
        list-style: none;
    }

    .select-container {
        position: relative;
    }

    .options {
        width: fit-content;
        background-color: var(--default-color-white);
        box-shadow: 0 0.07em 0.3em rgba(0,0,0,0.3);
        position: absolute;
        top: 100%;
        right: 0;
        display: none;
    }
    
    .select {
        padding: 0px 25px 0px 10px;
        cursor: pointer;
    }

    li {
        font-weight: normal;
        padding: 5px 30px;
        cursor: pointer;
    }

    li:hover {
        color: var(--default-color-red);
    }

    .select::before,
    .select::after {
        content: '';
        display: inline-block;
        width: 6px;
        border: 1px solid var(--default-color-black);
        position: absolute;
        top: calc(50% - 1px);
        right: 0;
        transition: all .3s;
    }

    .select::before {
        transform: rotate(-40deg);
        right: 7px;
    }
    
    .select::after {
        transform: rotate(40deg);
        right: 12px;
    }

    .select-container:has(.options.open) .select::before {
        border-color: var(--default-color-red);
        transform: rotate(40deg);
    }

    .select-container:has(.options.open) .select::after {
        border-color: var(--default-color-red);
        transform: rotate(-40deg);
    }

    .options.open {
        display: block;
        z-index: 1000000;
    }

    @media (max-width: 768px) {
        .select::before,
        .select::after {
            width: 5px;
            top: calc(50% - 2px);
        }
    }
</style>
