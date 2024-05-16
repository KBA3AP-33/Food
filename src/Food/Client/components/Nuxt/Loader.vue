<template>
    <div v-show="isVisible" class="loader"></div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        props: {
            isVisible: {
                type: Boolean as PropType<boolean>,
                default: false
            }
        }
    }
</script>

<style scoped>
    .loader {
        width: 50px;
        aspect-ratio: 1 / 1;
        position: absolute;
        top: calc(50% - 25px);
        left: calc(50% - 25px);
        border: 4px solid transparent;
        border-top-color: var(--default-color-red);
        border-right-color: var(--default-color-red);
        border-radius: 50%;
        animation: rotation 1.2s infinite linear;
        z-index: 100000000000;
    }

    @keyframes rotation {
        from {
            transform: rotate(0deg);
        }
        to {
            transform: rotate(360deg);
        }
    }
</style>
