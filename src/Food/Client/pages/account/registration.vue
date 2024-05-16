<template>
    <main class="page">
        <section class="login-page">
            <form class="login-form" @submit.prevent="registration">
                <h1 class="header">Регистрация</h1>
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
                <NuxtInputPassword id="passwordRepeat" placeholder="Подтверждение пароля" ref="passwordRepeat" :is-alternative-style="true"/>
                <div v-if="error" class="error">
                    <p>{{ error }}</p>
                </div>
                <div class="login-buttons">
                    <NuxtButton type="button" @click="$router.replace('/account/login')">Войти</NuxtButton>
                    <NuxtButton type="submit" class="green">Зарегестрироваться</NuxtButton>
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
        titleTemplate: 'Регистрация'
    });
</script>

<script lang="ts">
    import { authenticationStore } from "~/store/authentication";

    
    export default {
        data: () => ({
            authentication: authenticationStore(),
            error: '',
        }),
        methods: {
            async registration() {
                let login: string = (this.$refs.login as any).currentValue;
                let password: string = (this.$refs.password as any).$refs.pass.currentValue;
                let passwordRepeat: string = (this.$refs.passwordRepeat as any).$refs.pass.currentValue;

                if (password !== passwordRepeat) {
                    this.error = 'Проверьте введенные данные';
                    return;                    
                }
                this.error = '';
                await this.authentication.registration({ login, password, passwordRepeat })
                    .then(async () => { await navigateTo('../account/login', { replace: true }) })
                    .catch(_ => this.error = 'Проверьте введенные данные');
            }
        }
    }
</script>

<style scoped src="~/assets/login.css">
    
</style>
