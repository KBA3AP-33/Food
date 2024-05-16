<template>
    <div class="author-container">
        <div>
            <NuxtImage
                class="author-image"
                :settings="{
                    size: { x: 1, y: 1 },
                    isUser: true,
                    isIncrease: true,
                    image: author.image
                }"
                @changeModalComponent="$emit('changeModalComponent', $event)"/>
        </div>
        <div class="food-info">
            <div class="author-name">{{ author.userName }}</div>
            <div class="food-date">{{ new Date(created).toLocaleDateString() }}</div>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { Author } from '~/types';

    export default {
        props: {
            author: {
                type: Object as PropType<Author>,
                required: true
            },
            created: {
                type: String as PropType<string>,
                required: true
            },
        },
    }
</script>

<style scoped>
    .author-container {
        width: 100%;
        height: 100%;
        padding: 5px 20px 0 20px;
        display: flex;
        gap: 10px;
    }

    .author-container:hover .author-name {
        text-decoration: underline;
    }

    .author-image {
        width: 40px;
        border-radius: 50%;
    }

    .food-info {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
    }

    .author-name {
        max-width: 190px;
        overflow: hidden;
        white-space: nowrap;
        cursor: pointer;
    }

    .food-date {
        font-size: .9rem;
        font-weight: normal;
        color: var(--default-date-color);
    }
</style>
