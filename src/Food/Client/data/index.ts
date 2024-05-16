import images from "~/data/index.json";
import type { Ribbon } from "~/types";
import type { AuthPage, SearchSortOption, Slide, UserControl } from "~/types/page";

export const limit = 30;

export const ribbons = [] as Array<Ribbon>;

export const authPages = [
    { name: 'recipes-create', roles: ['Guest'] },
    { name: 'recipes-update-id', roles: ['Guest'] },
    { name: 'users-id', roles: ['Guest'] },
] as Array<AuthPage>;

export const defaultControls = [
    { icon: 'fast-food', name: 'my-recipes', point: 'Мои рецепты', roles: ['Guest'] },
    { icon: 'heart', name: 'my-favourite-recipes', point: 'Мне нравится', roles: ['Guest'] },
    { icon: 'hammer', name: 'controls', point: 'Управление', roles: ['Admin'] },
    { icon: 'key', name: 'user-settings', point: 'Изменить пароль', roles: ['Guest'] },
] as Array<UserControl>;

export const slides = [
    {
        image: images[0],
        header: {
            value: 'Если вас не зовут Google, перестаньте вести себя так, будто вы все знаете.',
            style: {
                fontWeight: '600',
                fontSize: '40px',
                color: '#fff',
                top: '0px',
                left: '10px'
            }
        }
    },
    {
        image: images[1],
        header: {
            value: 'Если ночью есть нельзя, то зачем лампочка в холодильнике?',
            style: {
                fontWeight: '600',
                fontSize: '40px',
                color: '#fff',
                bottom: '0px',
                left: '10px'
            }
        }
    },
    {
        image: images[0],
        header: {
            value: 'Здесь могла быть ваша реклама, но мне лень ее делать.',
            style: {
                fontWeight: '600',
                fontSize: '40px',
                color: '#fff',
                bottom: '0px',
                right: '10px'
            }
        }
    },
    {
        image: images[1],
        header: {
            value: 'Может показаться, что я ничего не делаю. Но в голове я очень занят.',
            style: {
                fontWeight: '600',
                fontSize: '40px',
                color: '#fff',
                top: '0px',
                left: '10px'
            }
        }
    },

] as Array<Slide>;

export const sortOptins = [
    {
        id: 1,
        name: 'Популярные'
    },
    {
        id: 2,
        name: 'Новые'
    },
    {
        id: 3,
        name: 'Старые'
    },
] as Array<SearchSortOption>;

export const fileImageTypes = ['.png', '.jpeg'];
