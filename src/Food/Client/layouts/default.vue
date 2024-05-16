<template>
    <NuxtLoadingIndicator color="#ff5640" :height="5"></NuxtLoadingIndicator>
    <div class="default-page">
        <NuxtHeader/>
        <main class="page">
            <section class="page-content">
                <NuxtLoader :is-visible="isLoader"/>
                <div v-show="!isLoader">
                    <NuxtPage @changeVisibleLoader="changeVisibleLoader" @changeModalComponent="changeModalComponent"/>
                </div>
                <NuxtModal :is-visible="isVisibleModal" :position="position" @changeVisibleModal="isVisibleModal = $event">
                    <div v-html="modalComponent"></div>
                </NuxtModal>
            </section>
            <NuxtButton :class="['button-top', (isVisibleButtonTop) && 'active']" @click="(isVisibleButtonTop) && scrollUp()"></NuxtButton>
            <NuxtWarning class="warning"/>
        </main>
        <NuxtFooter/>
    </div>
</template>

<script lang="ts">
    export default {
        data: () => ({
            throttle: useNuxtApp().$throttle,
            modalComponent: null as any,
            isVisibleButtonTop: false,
            isLoader: true,
            position: { x: 0, y: 0 },
            isVisibleModal: false,
        }),
        mounted() {
            window.addEventListener('scroll', this.throttle(this.handleScroll, 300));
        },
        unmounted() {
            window.removeEventListener('scroll', this.throttle(this.handleScroll, 300));
        },
        methods: {
            handleScroll() {
                this.isVisibleButtonTop = (window.scrollY > 400);
            },
            scrollUp() {
                window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
            },
            changeModalComponent(event: { event: MouseEvent, modalComponent: string }) {
                this.isVisibleModal = true;
                this.modalComponent = event.modalComponent;
                this.position = { x: event.event.clientX, y: event.event.clientY };
            },
            changeVisibleLoader(isVisible: boolean) {
                this.isLoader = isVisible;
            },
        },
    }
</script>

<style>
    .default-page {
        min-height: 100vh;
        display: grid;
        grid-template-rows: 60px auto 150px;
    }

    .page {
        width: 100%;
        display: flex;
        justify-content: center;
    }
    
    .page-left-container {
        background-color: var(--default-color-white);
    }

    .page-content {
        width: var(--content-width);
        border: 1px solid var(--default-color-gray);
        position: relative;
    }

    .page-container {
        width: 100%;
        min-height: calc(100vh - 110px);
        display: grid;
        grid-template-columns: auto;
        position: relative;
        overflow: hidden;
        --left-container: 320px;
        --right-container: 320px;
    }

    .page-container:has(.page-left-container) {
        grid-template-columns: var(--left-container) auto;
    }

    .page-container:has(.page-right-container) {
        grid-template-columns: auto var(--right-container);
    }

    .page-container:has(.page-right-container).page-container:has(.page-left-container) {
        grid-template-columns: var(--left-container) auto var(--right-container);
    }

    .button-top {
        width: 2.5rem !important;
        height: 2.5rem !important;
        background-image: var(--icon-arrow-up);
        background-repeat: no-repeat;
        background-size: 75%;
        background-position: center;
        position: fixed;
        right: 3%;
        bottom: 10%;
        transition: opacity .3s linear !important;
        opacity: 0;
        cursor: default !important;
        z-index: 100000000000;
    }

    .button-top.active {
        opacity: 1;
        cursor: pointer !important;
    }

    .warning {
        position: absolute;
        top: 10px;
        right: 5px;
        z-index: 10000000;
    }

    @media (max-width: 1880px) {
        .warning {
            max-width: 600px !important;
            position: fixed;
            top: auto;
            bottom: 10%;
            right: 50%;
            transform: translateX(50%);
        }
    }

    @media (max-width: 1400px) {
        .page-left-container,
        .page-center-container {
            transition: left 1s;
        }

        .page-left-container {
            width: var(--left-container);
            height: 100%;
            position: absolute;
            top: 0;
            left: calc(var(--left-container) * -1);
            border-left: none;
        }

        .page-container {
            grid-template-columns: 1fr !important;
        }

        .page-container:has(.page-right-container) {
            grid-template-columns: 1fr var(--right-container) !important;
        }

        .page-left-container.active { 
            left: 0;
        }
    }

    @media (max-width: 992px) {
        .page-content .page-container {
            grid-template-columns: 1fr !important;
        }

        .page-right-container {
            display: none !important;
        }
    }

    @media (max-width: 672px) {
        .warning {
            width: 90%;
        }
    }
</style>
