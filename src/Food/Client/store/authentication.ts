import { defineStore } from 'pinia'
import type { User } from '~/types'

export const authenticationStore = defineStore('authentication', {
    state: () => {
        return {
            user: null as User | null,
        }
    },
    actions: {
        setup(user: User | null) {
            this.user = user;
        },
        async login(user: { login: string, password: string }) {
            let result = await useNuxtApp().$host('api/account/login', {
                method: 'POST',
                body: { Login: user.login, Password: user.password }
            });
 
            if (typeof(result) === 'undefined') return false;
            this.user = result as User;
            useCookie('user', { expires: new Date(this.user.time) }).value = JSON.stringify(this.user);
            await navigateTo('../../');
            return true;
        },
        async registration(user: { login: string, password: string, passwordRepeat: string }) {
            return await useNuxtApp().$host('api/account/registration', {
                method: 'POST',
                body: { Login: user.login, Password: user.password, PasswordRepeat: user.passwordRepeat }
            });
        },
        async logout() {
            await useNuxtApp().$host(`api/account/logout/${this.user?.userId}`)
            .then(() => {
                useCookie('user').value = null;                
                this.user = null;
            });
        },
        async getUser(image: string) {
            return await useNuxtApp().$authHost(`api/users/${this.user?.userId}`, 'PUT', {}, { image });
        },
        async changePassword(password: string, nextPassword: string) {
            return typeof(await useNuxtApp()
                .$authHost(`api/account/passwordChange`, 'POST', {}, { id: this.user?.userId, password, nextPassword })) !== 'undefined';
        },
        async changeFavourites(recipeId: number) {
            if (this.user === null) return await navigateTo('/account/login?message=Авторизуйтесь');
            
            let result = await useNuxtApp()
                .$authHost(`api/users/favourite`, 'POST', {}, { userId: this.user?.userId, recipeId });
                
            let nextUser = useCookie('user').value as User | undefined;
            if (nextUser) {
                this.user!.favourites = (this.user!.favourites.includes(Number(result))
                ? this.user!.favourites.filter(e => e !== Number(result))
                : [...this.user!.favourites, Number(result)]).filter(e => e !== null);

                nextUser.favourites = this.user!.favourites;    
            }
            useCookie('user', { expires: new Date(this.user!.time) }).value = JSON.stringify(nextUser);
        },
        async changeRating(recipeId: number, rate: number) {
            if (this.user === null) return await navigateTo('/account/login?message=Авторизуйтесь');

            let result = await useNuxtApp()
                .$authHost(`api/users/rating`, 'POST', {}, { userId: this.user?.userId, recipeId, rate });

            return { userId: this.user!.userId, rate, result: Number(result) };
        },
    },
})
