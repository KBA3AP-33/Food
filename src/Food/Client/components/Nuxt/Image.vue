<template>
    <div
        :class="['image', `image-${settings.size?.x}-${settings.size?.y}`, (settings.image) ? 'img' : (settings.isUser) && 'image-user']"
        @click="settings.isIncrease && settings.image && $emit('changeModalComponent', { event: $event, modalComponent: ($refs.image as HTMLElement).innerHTML })">
        <div ref="image" style="display: none;">
            <div class="image image-4-3" :style="{ backgroundImage: settings.image, backgroundSize: 'cover' }"></div>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from "vue";
    import type { ImageProps } from "~/types/page";

    export default {
        data: () => ({
            image: ''
        }),
        props: {
            settings: {
                type: Object as PropType<ImageProps>,
                default: { size: { x: 1, y: 1 }, isUser: false, image: '', isIncrease: false } as ImageProps
            },
        },
        mounted() {
            this.image = `${this.settings.image}`;  
        },
        watch: {
            settings: {
                handler(nextValue) {
                    this.image = nextValue.image;
                },
                deep: true,
            },
        },
    }
</script>

<style scoped>
    .image {
        width: 100%;
        background-image: var(--icon-images);
        background-size: contain;
        background-repeat: no-repeat;
        background-size: 15% 15%;
        background-position: center;
        background-color: var(--default-image-background-color);
        border: 1px solid var(--default-color-red);
        border-radius: 3px;
        
    }

    .image-user {
        background-image: var(--icon-person-red);
        background-size: 60% 60%;
    }

    .img {
        background-image: v-bind(image);
        background-size: cover !important;
        cursor: pointer;
    }
</style>
