<template>
    <Title>Страница пользователя {{ authentication.user?.userName }}</Title>

    <div class="page-container page-user">
        <div :class="['page-left-container', (isActiveUserPanel) && 'active']">
            <div class="user-name">{{ authentication.user?.userName }}</div>
            <NuxtInputFile
                :image-size="{ x: 1, y: 1 }"
                :current-image="userImage"
                :isUserImage="true"
                :is-compact="true"
                style="width: 100%;"
                @changeImage="changeUser($event)"/>
            <div style="width: 100%;">
                <ul class="user-controls">
                    <li
                        :class="['control-item', (control.name === currentControl) && 'active']"
                        v-for="control in getControls"
                        :key="control.point"
                        @click="changeControl(control)">
                        <div :class="['icon', `icon-${control.icon}`, (control.name === currentControl) && 'active']"></div>
                        {{ control.point }}
                    </li>
                </ul>
            </div>
            <div class="button-close">
                <NuxtSpoiler header="" class="close-arrow" :is-visible="isActiveUserPanel" @click="isActiveUserPanel = !isActiveUserPanel"/>
            </div>
        </div>
        <div class="page-center-container" ref="page">
            <Catalog
                v-if="getControlByName('my-recipes')?.name === currentControl || getControlByName('my-favourite-recipes')?.name === currentControl"
                :header="getControlByName('my-recipes')?.name === currentControl ? 'Мои рецепты' : 'Мне нравится'"
                :is-search="false"
                :is-authorize="true"
                :catalog-items="catalogItems"
                :favourites="authentication.user?.favourites"/>

            <div v-if="getControlByName('controls')?.name === currentControl && authentication.user?.roles.includes('Admin')" class="admin-catalog">
                <AdminCatalog @changeModalComponent="$emit('changeModalComponent', $event)"/>
            </div>
            <div v-if="getControlByName('user-settings')?.name === currentControl">
                <div style="width: fit-content;">
                    <form @submit.prevent="changePassword" class="password-form">
                        <h2 class="password-form-header">Изменить пароль</h2>
                        <NuxtInputPassword id="oldPassword" placeholder="Старый пароль" ref="oldPassword"/>
                        <NuxtInputPassword id="newPassword" placeholder="Новый пароль" ref="newPassword"/>
                        <p class="password-form-error" v-show="changePasswordError">{{ changePasswordError }}</p>
                        <div class="password-form-buttons">
                            <NuxtButton type="submit">Изменить</NuxtButton>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { defaultControls } from '~/data';
    import { filterStore } from "~/store/filter";
    import { authenticationStore } from "~/store/authentication";
    import type { Catalog } from '~/types';
    import type { UserControl } from '~/types/page';


    definePageMeta({
        validate: ({ params }) => /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(params.id as string),
    })
</script>

<script lang="ts">
    export default {
        data: () => ({
            authentication: authenticationStore(),
            filter: filterStore(),
            controls: defaultControls,
            currentControl: defaultControls?.[-1]?.name,
            isActiveUserPanel: false,
            catalogItems: {} as Catalog,
            userImage: '',
            changePasswordError: '',
        }),
        emits: ['changeVisibleLoader', 'changeModalComponent'],
        async mounted() {
            this.userImage = (await this.$authHost(`api/users/${this.authentication.user?.userId}`) as any).image;
            let control = useCookie('control').value as UserControl | undefined;
            await this.changeControl((control) ? control : this.controls[0]);
            this.$emit('changeVisibleLoader', false);
        },
        beforeUnmount() {
            useCookie('control').value = null;
        },
        methods: {
            getControlByName(name: string) {
                return this.controls.find(e => e.name === name);
            },
            async changeControl(control: UserControl) {
                if (control.name !== this.currentControl) {
                    this.currentControl = control.name;
                    await this.activeControl(control);
                    useCookie('control').value = JSON.stringify(control);
                }
            },
            async activeControl(control: UserControl) {
                switch (control.name) {
                    case 'my-recipes':
                        await this.getMyCatalogItems();
                        break;
                    case 'my-favourite-recipes':
                        await this.getMyLikesCatalogItems();
                        break;
                    case 'user-settings':
                        this.changePasswordError = '';
                        break;
                }
            },
            async changeUser(image: string) {
                this.userImage = (await this.authentication.getUser(image) as any)?.image;
            },
            async getMyCatalogItems() {
                this.catalogItems = await this.filter.getRecipesAuthorize(`author=${this.authentication.user?.userName}&statuses=1&statuses=2&statuses=3`) as Catalog;
            },
            async getMyLikesCatalogItems() {
                let favourites = this.authentication.user?.favourites;
                let recipes = await this.filter.getRecipesById((favourites) ? favourites : []);
                this.catalogItems = { start: 0, limit: 0, recipes, count: 0 } as Catalog;
            },
            async changePassword() {
                let password: string = (this.$refs.oldPassword as any).$refs.pass.currentValue;
                let nextPassword: string = (this.$refs.newPassword as any).$refs.pass.currentValue;

                let result = await this.authentication.changePassword(password, nextPassword);
                this.changePasswordError = (result) ? 'Изменение пароля выполнено успешно' : 'При выполнении операции произошла ошибка';
            }
        },
        computed: {
            getControls() {
                return defaultControls.filter(e => e.roles.some(r => this.authentication.user?.roles.includes(r)))
            }
        },
        watch: {
            'authentication.user': function(nextValue) {
                if (nextValue === null) {
                    this.$router.push('../');
                }
            }
        }
    }
</script>

<style scoped>
    .page-container {
        display: grid;
        grid-template-columns: 300px auto !important;
    }

    .page-left-container {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 10px;
        padding: 10px;
        position: relative;
        z-index: 1000000000000;
        --left-container: 300px;
    }

    .page-center-container {
        min-height: calc(100vh - 110px);
        position: relative;
    }

    .user-name {
        font-size: 1.5em;
        font-weight: 900;
        padding: 0 5px;
    }

    .user-controls {
        list-style: none;
    }

    .control-item {
        display: flex;
        align-items: center;
        gap: 10px;
        font-size: 1.2em;
        padding: 10px 0;
        border-bottom: 1px solid var(--default-color-gray);
        cursor: pointer;
    }

    .control-item * {
        transition: all .3s;
    }

    .control-item.active,
    .control-item:hover .icon {
        color: var(--default-color-red);
    }

    .catalog-loader {
        width: 100%;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .icon {
        width: 25px;
        height: 25px;
        transition: background-image .3s;
        background-size: cover;  
        background-repeat: no-repeat;
        cursor: pointer;
    }

    .icon-fast-food {
        background-image: var(--icon-fast-food);
    }

    .icon-fast-food:hover {
        background-image: var(--icon-fast-food-hover);
    }

    .icon-fast-food.active {
        background-image: var(--icon-fast-food-active);
    }

    .icon-heart {
        background-image: var(--icon-heart);
    }

    .icon-heart:hover {
        background-image: var(--icon-heart-hover);
    }

    .icon-heart.active {
        background-image: var(--icon-heart-active);
    }

    .icon-hammer {
        background-image: var(--icon-hammer);
    }

    .icon-hammer:hover {
        background-image: var(--icon-hammer-hover);
    }

    .icon-hammer.active {
        background-image: var(--icon-hammer-active);
    }

    .icon-key {
        background-image: var(--icon-key-black);
    }

    .icon-key:hover {
        background-image: var(--icon-key-hover);
    }

    .icon-key.active {
        background-image: var(--icon-key-active);
    }

    .button-close {
        width: 65px;
        height: 75px;
        border-left: 1px solid var(--default-color-white);
        background: linear-gradient(105deg, var(--default-color-white) 0%, var(--default-color-gray) 100%);
        clip-path: polygon(0 0, 40% 0%, 40% 70%, 0 100%);
        position: absolute;
        top: -1px;
        left: 100%;
        z-index: 100000;
        display: none;
    }

    .close-arrow {
        border: none;
        rotate: -90deg;
        position: absolute;
        top: 25px;
        left: -10px;
    }

    .admin-catalog {
        display: grid;
        grid-template-rows: auto 60px;
    }

    .password-form {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: flex-start;
        gap: 10px;
        padding: 10px;
    }

    .password-form-header {
        width: 100%;
        text-align: center;
    }

    .password-form-error {
        color: var(--default-error-color);
    }

    .password-form-buttons {
        width: 100%;
        display: flex;
        justify-content: flex-end;
    }

    @media (max-width: 1400px) {
        .button-close {
            display: block;
        }

        .page-container {
            grid-template-columns: 0 1fr !important;
        }
    }

    @media (max-width: 992px) {
        .page-container.page-user {
            grid-template-columns: 0 1fr !important;
        }
    }
</style>
