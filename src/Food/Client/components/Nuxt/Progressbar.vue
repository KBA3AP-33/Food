<template>
    <div class="progress-container">
        <div class="progress active"></div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            intervalId: {} as NodeJS.Timeout,
        }),
        props: {
            time: {
                type: Number as PropType<number>,
                default: 10
            }
        },
        mounted() {
            let time = this.time * 1000;
            this.intervalId = setInterval(() => this.$emit('changeProgress'), time);
        },
        beforeUnmount() {
            clearInterval(this.intervalId);
        },
        computed: {
            getTime() {
                return `${this.time}s`;
            }
        }
    }
</script>

<style scoped>
    .progress,
    .progress-container {
        width: 100%;
        height: 3px;
        position: relative;
        overflow: hidden;
    }

    .progress {
        background-color: var(--default-color-red);
        position: absolute;
        top: 0;  
    }

    .progress.active {
        animation: move v-bind(getTime) linear infinite;
    }

    @keyframes move {
        from {
            left: -100%;
        }
        to {
            left: 0;
        }
    }
</style>
