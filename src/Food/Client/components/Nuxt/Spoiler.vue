<template>
    <div :class="['spoiler', (isOpen) && 'open']">
        <div class="spoiler-header" @click="isOpen = !isOpen">{{ header }}</div>
        <div class="content">
            <slot></slot>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            isOpen: false,  
        }),
        props: {
            header: {
                type: String as PropType<string>,
                required: true
            },
            isVisible: {
                type: Boolean as PropType<boolean>,
                default: false
            }
        },
        mounted() {
            this.isOpen = this.isVisible;
        },
    }
</script>

<style scoped>
    .spoiler {
        padding: 5px;
        border-bottom: 1px solid var(--default-color-gray);
    }

    .spoiler-header {
        color: var(--default-color-black);
        font-size: 1.2em;
        font-weight: 700;
        margin: 5px 0;
        padding-left: 30px;
        position: relative;
        cursor: pointer;
    }

    .spoiler-header::before,
    .spoiler-header::after {
        content: '';
        display: inline-block;
        width: 10px;
        border: 2px solid var(--default-color-black);
        position: absolute;
        top: calc(50% - 1px);
        left: 0;
        transition: all .3s;
    }

    .spoiler-header::before {
        border-top-left-radius: 10px;
        border-bottom-left-radius: 10px;
        transform: rotate(40deg);
        left: 4px;
    }
    
    .spoiler-header::after {
        border-top-right-radius: 10px;
        border-bottom-right-radius: 10px;
        transform: rotate(-40deg);
        left: 12px;
    }

    .spoiler.open .spoiler-header::before {
        border-color: var(--default-color-red);
        transform: rotate(-40deg);
    }

    .spoiler.open .spoiler-header::after {
        border-color: var(--default-color-red);
        transform: rotate(40deg);
    }

    .content {
        max-height: 0px;
        overflow: auto;
        transition: max-height .3s linear;
    }
    
    .spoiler.open .content {
        max-height: 250px;
    }
</style>
