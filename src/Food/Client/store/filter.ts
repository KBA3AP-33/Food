import { defineStore } from 'pinia';
import type { Filters } from '~/types';
import type { Filter } from '~/types/page';


export const filterStore = defineStore('filter', {
    state: () => {
        return {
            lastSearch: '',
            options: { start: 0, search: '', sort: 0 },
            filters: {} as Filters,
            defaultFilters: [] as Array<Filter>,
            userFilters: [] as Array<{ id: number, selected: Array<number> }>,
        }
    },
    actions: {
        async setup(params: string, options: string | null, isUseFilterByLoad = false) {
            await this.getDefaultFilters();
            let filters = this.getSelectedFilters(this.getFilters(params));
            this.userFilters = (filters.length) ? filters : this.defaultFilters.map(e => ({ id: e.id, selected: [] }));
            this.options = (options) ? this.getOptions(options) : this.options;

            if (isUseFilterByLoad) {
                return await this.useFilter();
            }
        },
        async useFilter(isLast = false) {
            let search = (isLast) ? this.lastSearch : this.getSearchParams();
            let options = Object.entries(this.options).filter(e => e[1] !== '').map(e => `${e[0]}=${e[1]}`).join('&');
            useCookie('search').value = search;
            useCookie('options').value = options;
            this.lastSearch = search;

            return await this.getRecipes(`${options}${search !== '' ? `&${search}` : ''}`);
        },
        async getFilter() {
            return (this.filters.categories && this.filters.ingredients && this.filters.kitchens && this.filters.timeCategories)
                ? this.filters
                : await useNuxtApp().$host('api/filters') as Filters;
        },
        async getDefaultFilters() {
            this.filters = await this.getFilter();
            this.defaultFilters = [
                {
                    id: 1,
                    name: 'time',
                    header: 'Быстрые рецепты',
                    filter: [
                        { id: 1, name: 'До 15 минут' },
                        { id: 2, name: 'До 30 минут' },
                        { id: 3, name: 'До 45 минут' },
                        { id: 4, name: 'До 60 минут' },
                    ],
                    isVisible: true,
                },
                {
                    id: 2,
                    name: 'categories',
                    header: 'Категории блюд',
                    filter: this.filters.categories,
                    isVisible: false,
                },
                {
                    id: 3,
                    name: 'ingredients',
                    header: 'Ингредиенты',
                    filter: this.filters.ingredients,
                    isVisible: false,
                },
                {
                    id: 4,
                    name: 'times',
                    header: 'Время приема пищи',
                    filter: this.filters.timeCategories,
                    isVisible: false,
                },
                {
                    id: 5,
                    name: 'kitchens',
                    header: 'Кухня',
                    filter: this.filters.kitchens,
                    isVisible: false,
                }
            ]
        },
        async changeOptions(options: Array<{ key: string, value: number | string }>, isLast = false) {
            options.forEach(e => (this.options[e.key as keyof typeof this.options] as any) = e.value)
            return await this.useFilter(isLast);
        },
        async getRecipesByWithoutChangeOptions(search: string) {
            return await this.getRecipes(`${search}`);
        },
        async getRecipesById(ids: Array<number>) {
            return await useNuxtApp()
                .$host(`api/catalog/recipes`, { method: 'POST', body: JSON.stringify(ids) });
        },
        async getRecipesAuthorize(search: string) {
            return await useNuxtApp()
                .$authHost(`api/catalog/auth?${search}`);
        },
        async getRecipes(search: string) {
            return await useNuxtApp()
                .$host(`api/catalog?${search}`);
        },
        async getRecipe(id: number, isAuthorize: boolean, isAdmin: boolean) {
            return (isAuthorize)
                ? (isAdmin)
                    ? await useNuxtApp().$authHost(`api/catalog/authorize/${id}?statuses=2&statuses=3&statuses=4`)
                    : await useNuxtApp().$authHost(`api/catalog/auth/${id}?statuses=2&statuses=3&statuses=4`)
                : await useNuxtApp().$host(`api/catalog/${id}`);
        },
        getSelectedFilters(params: string) {
            let filters = [] as Array<{id: number, selected: Array<number>}>;

            params?.split('&').forEach(e => {
                let filterParams = e?.split('=');
                let filter = this.defaultFilters.find(e => e.name === filterParams[0]);

                if (typeof(filter) !== 'undefined') {
                    let findFilter = filter.filter.find(e => e.id === Number(filterParams[1]));
                    if (typeof(findFilter) !== 'undefined') {
                        let currentFilter = filters.find(e => e.id === filter?.id);
                        
                        if (currentFilter) {
                            currentFilter.selected = currentFilter.selected.includes(findFilter.id)
                            ? currentFilter.selected.filter(i => i !== findFilter?.id)
                            : [...currentFilter.selected, findFilter.id];
                        } else {
                            filters.push({ id: filter.id, selected: [findFilter.id] });
                        }
                    }
                }
            });
            return filters;
        },
        changeUserFilters(filter: {id: number, select: number}) {
            let userFilter = this.userFilters.find(e => e.id === filter.id);
 
            if (userFilter) {
                userFilter.selected = (userFilter.selected.includes(filter.select)) ? userFilter.selected.filter(i => i !== filter.select) : [...userFilter.selected, filter.select];
            } else {
                this.userFilters.push({ id: filter.id, selected: [filter.select] });
            }
        },
        getFilters(search: string) {
            return search?.split('&').map(e => {
                let items = e.split('=');
                return `${items[0]}=${(items[0] === this.defaultFilters[0].name) ? this.defaultFilters[0].filter.find(x => x.name.includes(items[1]))?.id : items[1] }`;
            }).join('&');
        },
        getOptions(options: string) {
            let result = {} as { start: number, search: string, sort: number };
            options.split('&').forEach(e => {
                let items = e.split('=');
                (result[items[0] as keyof typeof result] as any) = isNaN(Number(items[1])) ? items[1] : Number(items[1]);
            });

            return result;
        },
        getSearchParams() {
            let paramsUrl = [] as Array<{ key: string, value: string }>;

            this.userFilters.forEach(e => {
                if (e.id === 1) {
                    let value = this.defaultFilters[0].filter.find(x => x.id === Math.max(...e.selected))?.name.split(' ')[1];
                    paramsUrl = (value)
                    ? [...paramsUrl, {
                        key: this.defaultFilters[0].name,
                        value: value
                    }]
                    : [...paramsUrl];
                } else {
                    paramsUrl = [...paramsUrl, ...e.selected.map(i => ({ key: this.defaultFilters[e.id - 1].name, value: `${i}` }))];
                }
            });

            return paramsUrl.map(e => `${e.key}=${e.value}`).join('&');
        },
    }
});
