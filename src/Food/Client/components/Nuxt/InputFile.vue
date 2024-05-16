<template>
    <div
        class="input-image-file"
        @dragover.prevent="isDragging = true"
        @dragleave="isDragging = false"
        @drop.prevent="drop">
        <input type="file" @change="fileChanged($event)" ref="file" accept="image/*" hidden>
        <div v-if="isCompact" :class="['button-add-file', isActiveCompactButton && 'active']" title="Выберите файл" @click="($refs.file as any).click()">
            <div class="icon icon-add-circle-outline-white"></div>
        </div>
        <NuxtImage
            @click="isActiveCompactButton = !isActiveCompactButton"
            :class="['user-image', (isDragging) && 'drag', (!isCompact) && 'compact']"
            :settings="{
                size: imageSize,
                isUser: isUserImage,
                image: image
            }"/>
        <NuxtButton v-if="!isCompact" @click="($refs.file as any).click()" type="button">Выберите файл</NuxtButton>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import { fileImageTypes } from '~/data';

    export default {
        data: () => ({
            image: '',
            isDragging: false,
            isActiveCompactButton: false,
        }),
        props: {
            imageSize: {
                type: Object as PropType<{ x: number, y: number}>,
                default: { x: 3, y: 2 }
            },
            isUserImage: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            isCompact: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            currentImage: {
                type: String as PropType<string>,
                default: ''
            },
        },
        mounted() {
            this.image = this.currentImage;
        },
        methods: {
            onChange(file: File) {
                let params = file.name.split('.');
                if (!fileImageTypes.includes(`.${params[params.length - 1]}`)) return;
                if (file.size > 1048576) return;
                
                let reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => { this.image = `url(${reader.result})` };

                this.isActiveCompactButton = false;
            },
            drop(event: DragEvent) {
                this.onChange(event.dataTransfer?.files[0]!);
                this.isDragging = false;
            },
            fileChanged(event: Event) {
                let file = (event.target as any).files[0] as File;
                this.onChange(file);
            },
        },
        watch: {
            image(nextValue) {
                this.$emit('changeImage', nextValue);
            },
            currentImage(nextValue) {
                this.image = nextValue;
            },
        },
    }
</script>

<style scoped>
    .input-image-file {
        position: relative;
        overflow: hidden;
    }

    .button-add-file {
        --border-size: 40px;
        position: absolute;
        top: calc(var(--border-size) * -2);
        right: calc(var(--border-size) * -2);
        border: var(--border-size) solid transparent;
        border-top: var(--border-size) solid rgba(0, 0, 0, 0.7);
        border-right: var(--border-size) solid rgba(0, 0, 0, 0.7);
        cursor: pointer;
        transition: all .3s ease-in-out;
    }

    .icon {
        width: 30px;
        height: 30px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        background-color: transparent;
        cursor: pointer;
        position: absolute;
        top: -30px;
        left: 0px;
    }

    .active {
        top: 1px;
        right: 1px;
    }

    .user-image {
        margin-bottom: 0;
    }

    .drag {
        border: 2px solid var(--default-color-red);
    }

    .compact {
        margin-bottom: 10px;
    }
</style>
