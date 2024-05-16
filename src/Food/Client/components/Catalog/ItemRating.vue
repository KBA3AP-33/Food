<template>
    <div class="rating">
        <div class="rate">
            <div class="icon star"></div>
            <span class="number">{{ rate }}</span>
        </div>
        <div :class="['icon', 'heart', (isLike) && 'active']" @click.stop="$emit('changeFavourite')"></div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            rate: ''
        }),
        props: {
            isLike: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            rating: {
                type: Number as PropType<number>,
                default: 0
            },
        },
        mounted() {
            this.rate = (this.rating === null ||this.rating === 0) ? 'Нет данных' : `${this.rating.toLocaleString('en-US', { minimumFractionDigits: 1, maximumFractionDigits: 1 })}`;
        },
    }
</script>

<style scoped>
    .rating {
        display: flex;
        justify-content: space-between;
        align-items: center;
        padding: 0 13px;
    }

    .number {
        font-size: 1.3rem;
    }

    .rate {
        height: 100%;
        display: flex;
        align-items: flex-start;
        gap: 5px;
        padding-top: 10px;
    }

    .icon {
        width: 22px;
        height: 22px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .star {
        background-image: var(--icon-star);
    }

    .heart {
        background-image: var(--icon-heart);
    }

    .heart:hover {
        background-image: var(--icon-heart-hover);
    }

    .heart.active {
        background-image: var(--icon-heart-active);
    }
</style>
