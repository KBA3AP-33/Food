<template>
    <Transition name="fade">
        <div class="modal-menu" v-show="isVisible" @click="$emit('changeVisibleModal', false)">
            <div class="modal-content" @click.stop>
                <slot></slot>
            </div>
        </div>
    </Transition>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            top: '0px',
            left: '0px',
        }),
        props: {
            isVisible: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            position: {
                type: Object as PropType<{ x: number, y: number }>,
                default: { x: 0, y: 0 }
            },
        },
        watch: {
            position: {
                handler(newValue) {
                    this.top = `${newValue.y}px`;
                    this.left = `${newValue.x}px`;
                },
                deep: true,
            },
        },
    }
</script>

<style scoped>
    .modal-menu {
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        background: rgba(0,0,0,0.5);
        z-index: 10000000000000;
    }

    .modal-content {
        width: 40%;
        position: absolute;
        top: 50%;
        left: 50%;
        background-color: var(--default-color-white);
        padding: 20px 25px;
        transform: translate(-50%, -50%);
    }

    .fade-enter-active,
    .fade-leave-active {
        transition: opacity .3s ease-in-out;
    }

    .fade-enter-from,
    .fade-leave-to {
        opacity: 0;
    }

    .fade-enter-active .modal-content,
    .fade-leave-active .modal-content {
        animation: appearance .3s ease-in-out;
    }

    .fade-enter-from .modal-content,
    .fade-leave-to .modal-content {
        animation: appearance .3s ease-in-out reverse;
    }

    @keyframes appearance {
        15% {
            width: 0;
            top: v-bind(top);
            left: v-bind(left);
        }
    }

    @media (max-width: 1400px) {
        .modal-content {
            width: 60%;
        }
    }

    @media (max-width: 992px) {
        .modal-content {
            width: 80%;
        }
    }

    @media (max-width: 768px) {
        .modal-content {
            width: 100%;
        }
    }
</style>
