<template>
    <main class="page">
        <section class="login-page">
            <form class="login-form" @submit.prevent="login">
                <h1 class="header">Вход</h1>
                <NuxtInputText
                    id="login"
                    ref="login"
                    class="alternative-style"
                    :settings="{
                        type: 'text',
                        placeholder: 'Логин',
                        iconName: 'icon-person-white',
                        minlength: 5,
                        maxlength: 20,
                    }"/>
                <NuxtInputPassword id="password" ref="password" :is-alternative-style="true"/>
                <div class="error" v-if="error">
                    <p>{{ error }}</p>
                </div>
                <div class="login-buttons">
                    <NuxtButton type="button" @click="$router.replace('/account/registration')">Регистрация</NuxtButton>
                    <NuxtButton type="submit" class="green">Войти</NuxtButton>
                </div>
            </form>
            <NuxtSlider style="height: 100%; position: absolute; top: 0; z-index: -1; background-color: #131e22;"/>
        </section>
    </main>
</template>

<script setup lang="ts">
    definePageMeta({
        layout: "empty",
    });
    useHead({
        titleTemplate: 'Войти'
    });
</script>

<script lang="ts">
    import { authenticationStore } from "~/store/authentication";

    export default {
        data: () => ({
            error: '',
        }),
        mounted() {
            let message = this.$route.query.message;
            if (message) {
                this.error = message.toString();
            }
        },
        methods: {
            async login() {
                let login: string = (this.$refs.login as any).currentValue;
                let password: string = (this.$refs.password as any).$refs.pass.currentValue;
                let result = await authenticationStore().login({ login, password });
                if (!result) this.error = 'Неверный логин или пароль.';
            },
        }
    }
</script>

<style scoped src="~/assets/login.css">

</style>
