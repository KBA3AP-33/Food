<template>
    <div
        class="slider"
        :style="{ backgroundImage: currentSlideImage }"
        v-if="settings.slides.length"
        @mousedown="mouseX = $event.offsetX"
        @mouseup="swipe($event.offsetX)"
        @touchstart="mouseX = $event.targetTouches[0].clientX"
        @touchend="swipe($event.changedTouches[0].clientX)">
        <div v-if="settings.isVisibleArrows" class="icon icon-button-arrow-back-circle arrow left" @click="swipeSlide(currentSlide - 1)"></div>
        <div
            v-for="(slide, index) in settings.slides"
            :key="index"
            :class="['slide', (currentSlide === index) && 'active']">
            <p class="slide-header" :style="slide.header?.style">{{ slide.header?.value }}</p>
        </div>
        <div v-if="settings.isVisibleArrows" class="icon icon-button-arrow-forward-circle arrow right" @click="swipeSlide(currentSlide + 1)"></div>
        <div class="point-container" v-if="settings.isVisiblePoints && settings.slides.length > 1 && settings.slides.length < 11">
            <div v-for="(point, index) in settings.slides.length" :key="point" :class="['point', (currentSlide === index) && 'active']" @click="swipeSlide(index)"></div>
        </div>
    </div>
</template>

<script lang="ts">
    import { slides } from '~/data';
    import type { PropType } from 'vue';
    import type { SliderProps } from '~/types/page';

    export default {
        data: () => ({
            mouseX: 0,
            currentSlide: 0,
            currentSlideImage: '',
            intervalId: {} as NodeJS.Timeout,
            index: 0,
        }),
        props: {
            settings: {
                type: Object as PropType<SliderProps>,
                default: { isVisibleArrows: false, isVisiblePoints: false, slides: slides } as SliderProps
            },
        },
        mounted() {
            this.intervalId = setInterval(() => this.swipeSlide(this.currentSlide + 1), 10000);
            this.changeSlideImage();
        },
        beforeUnmount() {
            clearInterval(this.intervalId);
        },
        methods: {
            swipeSlide(nextIndex: number) {
                this.currentSlide = (nextIndex > this.settings.slides.length - 1) ? 0 : (nextIndex < 0) ? this.settings.slides.length - 1 : nextIndex;
                this.changeSlideImage();
            },
            swipe(end: number) {
                if (Math.abs(end - this.mouseX) < 100) return;
                this.swipeSlide((end > this.mouseX) ? this.currentSlide - 1 : this.currentSlide + 1);
            },
            changeSlideImage() {
                this.currentSlideImage = this.settings.slides[this.index]?.image;
                this.index = (this.index === this.settings.slides?.length - 1) ? 0 : this.index + 1;
            },
        },
    }
</script>

<style scoped>
    .slider {
        width: 100%;
        height: 200px;
        background-size: cover;
        background-repeat: no-repeat;   
        background-position: center;
        backface-visibility: hidden;
        transition: all 2s ease-in-out;
        position: relative;
        user-select: none;
    }

    .slide-header {
        transition: opacity 2s ease-in-out;
        position: absolute;
        opacity: 0;
    }

    .slide.active .slide-header {
        opacity: .6;
    }

    .arrow {
        position: absolute;
        top: calc(50% - 2%);
    }

    .left {
        left: 10px;
    }

    .right {
        right: 10px;
    }

    .point-container {
        display: flex;
        flex-wrap: nowrap;
        justify-content: center;
        align-items: center;
        gap: 5px;
        position: absolute;
        bottom: 10%;
        left: 50%;
        transform: translateX(-50%);
    }

    .point {
        width: 10px;
        height: 10px;
        border: 1px solid var(--default-color-red);
        border-radius: 50%;
        cursor: pointer;
        transition: background-color .3s;
    }

    .point.active {
        background-color: var(--default-color-red);
    }

    .point:hover {
        background-color: var(--default-color-orange);
    }

    .icon {
        height: 4%;
        aspect-ratio: 1 / 1;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }
</style>
