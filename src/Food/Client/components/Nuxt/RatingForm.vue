<template>
    <div class="rating">
        <div :class="['rating-container', (isReadOnly) && 'readonly']" @mouseleave="hoverRating = 0">
            <div
                :class="['icon', 'icon-star', (currentRating >= rate) && 'active']"
                v-for="rate in maxRating"
                :key="rate"
                @click="$emit('changeRating', rate)"
                @mouseenter="hoverRating = rate">
            </div>
            <div class="rating-border" :style="{ left: `${(hoverRating * (100 / maxRating)) - 100}%` }"></div>
        </div>
        <p class="rating-number">{{ getRating }}</p>
    </div>
</template>

<script lang="ts">
import type { PropType } from 'vue';

    export default {
        data: () => ({
            maxRating: 5,
            hoverRating: 0,
            isReadOnly: false,
        }),
        props: {
            rating: {
                type: Number as PropType<number>,
                default: 0
            },
            isReadOnlyRating: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            currentRating: {
                type: Number as PropType<number>,
                default: 0
            }
        },
        mounted() {
            this.isReadOnly = (this.isReadOnlyRating) ? true : (this.currentRating > 0);
        },
        computed: {
            getRating() {
                return this.rating.toLocaleString('en-US', { minimumFractionDigits: 1, maximumFractionDigits: 1 });
            },
        },
        watch: {
            currentRating(nextValue) {
                this.isReadOnly = nextValue > 0;
            },
        },
    }
</script>

<style scoped>
    .rating {
        display: flex;
        align-items: center;
        gap: 5px;
    }

    .rating-container {
        height: 25px;
        display: flex;
        flex-wrap: nowrap;
        gap: 5px;
        position: relative;
        overflow: hidden;
    }

    .rating-number {
        font-size: 1.5em;
    }

    .icon {
        width: 22px;
        height: 22px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-star {
        background-image: var(--icon-star);
    }

    .icon-star:hover {
        background-image: var(--icon-star-hover);
    }

    .icon-star.active {
        background-image: var(--icon-star-active);
    }

    .rating-border {
        width: 100%;
        height: 2px;
        background-color: var(--default-color-red);
        position: absolute;
        bottom: 0;
        left: -100%;
        transition: left .3s;
    }

    .readonly {
        pointer-events: none;
    }
</style>
