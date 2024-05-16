<template>
    <main class="error-page">
        <section class="error-page-container">
            <div class="error-code">{{ error?.statusCode }}</div>
            <p class="error-message">{{ error?.message }}</p>
            <NuxtButton @click="clearError({ redirect: '/' })">Вернуться на главную</NuxtButton>
        </section>
    </main>
</template>

<script lang="ts" setup>
    import type { NuxtError } from '#app';
    import images from "~/data/index.json";

    useHead({
        titleTemplate: 'Error',
    });
    
    defineProps({
        error: Object as () => NuxtError
    });

    const background = images[0];
</script>

<style scoped>
    .error-page {
        min-height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .error-page-container {
        width: 60%;
        width: clamp(310px, 1000px, 80%);
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 20px;
        padding: 20px;
        background-color: var(--default-error-page-background-color);
        box-shadow: 0 14px 28px rgba(0,0,0,0.25), 0 10px 10px rgba(0,0,0,0.22);
    }

    .error-code {
        font-size: clamp(10rem, 25vw, 20rem);
        font-weight: bold;
        background-image: v-bind(background);
        background-repeat: no-repeat;
        background-position: center;
        background-size: cover;
        background-clip: text;
        -webkit-text-fill-color: transparent;
    }

    .error-message {
        font-size: 1rem;
        color: var(--default-date-color);
    }
</style>
