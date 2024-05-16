import type { RouteLocationNormalized } from "vue-router";
import { authPages } from "~/data";


export default (to: RouteLocationNormalized, from: RouteLocationNormalized) => {
    const page = authPages.find(e => e.name === to.name);
    if (typeof(page) !== 'undefined') {
        const user = useCookie('user');
        const login = '/account/login';

        if (typeof(user.value) === 'undefined' || user.value === null) { 
            return navigateTo(`${login}?message=Авторизуйтесь`);
        } else {
            if (!page.roles.some(e => (user.value as any).roles.includes(e))) {
                return navigateTo(`${login}?message=Не достаточно прав`);
            }
        }
    }
}
