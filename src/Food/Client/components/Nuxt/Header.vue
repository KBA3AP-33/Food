<template>
    <header class="triple-table header">
        <nav class="navbar-container" style="width: var(--content-width);">
            <div class="nav-buttons">
                <NuxtLink class="link main-link" to="/" exact>EASY-COOK</NuxtLink>
            </div>
            <div class="nav-content">
                <ul class="list-link">
                    <li><NuxtLink class="link" to="/account/login" exact v-if="!authentication.user">Войти</NuxtLink></li>
                    <li v-if="authentication.user">
                        <div class="link user">
                            <span @click="isUserPanel = !isUserPanel"><b>{{ authentication.user?.userName }}</b></span>
                            <div class="user-panel">
                                <ul :class="['user-control', (isUserPanel) && 'active']">
                                    <li class="user-control-item" @click="getUserPage">
                                        <div class="icon icon-person"></div>
                                        Профиль
                                    </li>
                                    <li class="user-control-item" @click="logout">
                                        <div class="icon icon-logout"></div>
                                        Выход
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
</template>

<script lang="ts">
    import { authenticationStore } from "~/store/authentication";
    import type { User } from "~/types";


    export default {
        data: () => ({
            authentication: authenticationStore(),
            isUserPanel: false,
        }),
        mounted() {
            let user = useCookie('user').value as User | undefined;
            this.authentication.setup((typeof(user) !== 'undefined') ? user : null);
        },
        methods: {
            getUserPage() {
                this.isUserPanel = false;
                this.$router.push(`../users/${this.authentication.user?.userId}`);
            },
            async logout() {
                this.isUserPanel = false;
                await this.authentication.logout();
            },
        },
    }
</script>

<style scoped>
    .header {
        width: 100%;
        display: flex;
        justify-content: center; 
    }

    .navbar-container {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0 12px;
        border-left: 1px solid var(--default-color-gray);
        border-right: 1px solid var(--default-color-gray);
    }

    .nav-buttons {
        display: flex;
        justify-content: space-between;
    }

    .nav-content {
        display: flex;
        justify-content: flex-end;
    }
    
    .list-link {
        margin: 0;
        padding: 0;
        list-style: none;
        display: flex;
        align-items: center;
        gap: 7px;
    } 

    .link {
        font-size: 1.3rem;
        font-weight: 400;
        font-style: normal;
        font-family: "Marck Script", sans-serif;
        letter-spacing: 2px;
        text-wrap: nowrap;
        text-decoration: none;
        color: var(--default-color-black);
        display: block;
        padding: 8px;
    }

    .link:hover {
        color: var(--default-color-red);
        transition: all .3s;
    }

    .main-link {
        font-size: 1.75rem;
        font-family: "Aguafina Script", serif;
        font-weight: 900;
        font-style: normal;
        padding-bottom: 0;
        letter-spacing: 2px;
    }

    .user {
        display: flex;
        align-items: center;
        gap: 5px;
        position: relative;
        cursor: pointer;
    }

    .user-panel {
        width: fit-content;
        box-shadow: 0 0.07em 0.3em rgba(0,0,0,0.3);
        background-color: var(--default-color-white);
        color: #000;
        position: absolute;
        top: calc(100% + 9px);
        right: -12px;
        z-index: 1000000;
    }
    
    .user-control {
        display: none;
        list-style: none;
    }

    .user-control.active {
        display: block;
    }

    .user-control-item {
        font-size: 1.5rem;
        padding: 5px 20px;
        display: flex;
        align-items: center;
        gap: 15px;
    }

    .user-control-item:not(:last-child) {
        border-bottom: 1px solid var(--default-color-gray);
    }

    .user-control-item:hover {
        color: var(--default-color-red);
    }

    .icon {
        width: 25px;
        height: 25px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-person {
        background-image: var(--icon-person-black);
    }

    .icon-logout {
        background-image: var(--icon-logout);
    }
</style>
