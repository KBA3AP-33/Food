<template>
    <div class="recipe-face">
        <NuxtImage
            :settings="{
                size: { x: 16, y: 9 },
                image: face.image,
                isIncrease: true
            }"
            @changeModalComponent="$emit('changeModalComponent', $event)"/>
        <div class="recipe-info no-print">
            <div>
                <CatalogItemAuthor class="author" v-if="face.author" :author="face.author" :created="face.created" @changeModalComponent="$emit('changeModalComponent', $event)"/>
            </div>
            <div v-if="!isReadOnly">
                <NuxtRatingForm :rating="face.rating" :current-rating="face.rate" @changeRating="$emit('changeRating', $event)"/>
            </div>
        </div>
        
        <div v-if="!isReadOnly" class="recipe-buttons no-print">
            <NuxtButton
                @click="$emit('remove')"
                v-if="face.author.id === authentication.user?.userId || authentication.user?.roles.includes('Admin')"
                title="Удалить">
                <div class="icon icon-trash"></div>
            </NuxtButton>
            <NuxtButton
                @click="$emit('transitionPage')"
                v-if="face.author.id === authentication.user?.userId || authentication.user?.roles.includes('Admin')"
                title="Редактировать">
                <div class="icon icon-edit"></div>
            </NuxtButton>
            <NuxtButton class="filter" @click="$emit('changeVisibleFilter')" title="Фильтр">
                <div class="icon icon-filter"></div>
            </NuxtButton>
            <NuxtButton @click="$emit('print')" title="Печатать">
                <div class="icon icon-print"></div>
            </NuxtButton>
            <NuxtButton @click="$emit('changeFavourite')">
                <div :class="['icon', 'icon-heart', (isFavourite) && 'active']"></div>
            </NuxtButton>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { RecipeFace } from '~/types/page';
    import { authenticationStore } from "~/store/authentication";

    export default {
        data: () => ({
            authentication: authenticationStore(),
            isFavourite: false,
            isRating: false,
        }),
        props: {
            isReadOnly: {
                type: Boolean as PropType<boolean>,
                default: false
            },
            face: {
                type: Object as PropType<RecipeFace>,
                default: { image: null, author: null, created: null, rating: 0, rate: 0, favourite: false }
            },
        },
        mounted() {
            this.isFavourite = this.face.favourite;
        },
        watch: {
            face: {
                handler(nextValue) {
                    this.isFavourite = nextValue.favourite;
                },
                deep: true,
            },
        }
    }
</script>

<style scoped>
    .recipe-face {
        padding: 15px 0;
    }

    .recipe.print div:nth-child(2).recipe-face .no-print {
        display: none;
    }

    .recipe-info {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .author {
        padding: 10px 0px;
        font-weight: 700;
    }

    .recipe-buttons {
        display: flex;
        justify-content: flex-end;
        align-items: center;
        gap: 5px;
    }

    .recipe-buttons * {
        max-height: 40px;
        padding: 0 9px;
    }

    .filter {
        display: none;
    }

    .icon {
        width: 20px;
        height: 20px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-trash {
        background-image: var(--icon-trash);
    }

    .icon-edit {
        background-image: var(--icon-edit);
    }

    .icon-filter {
        background-image: var(--icon-filter);
    }

    .icon-print {
        background-image: var(--icon-print);
    }

    .icon-heart {
        background-image: var(--icon-heart);
    }

    .icon-heart.active {
        background-image: var(--icon-heart-active);
    }

    @media (max-width: 1400px) {
        .filter {
            display: inline-block;
        }
    }

    @media (max-width: 425px) {
        .recipe-info {
            flex-direction: column;
            align-items: flex-start;
        }
    }
</style>
