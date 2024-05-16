import type { Author } from ".";

export interface AuthPage {
    name: string,
    roles: Array<string>,
}

export interface Filter {
    id: number,
    name: string,
    header: string,
    filter: Array<FilterItem>,
    isVisible: boolean,
}

export interface FilterItem {
    id: number,
    name: string,
}

export interface UserControl {
    icon?: string,
    name: string,
    point: string,
    roles: Array<string>,
}

export interface SliderProps {
    isVisibleArrows: boolean,
    isVisiblePoints: boolean,
    slides: Array<Slide>,
}

export interface Slide {
    image: string,
    header?: SlideParam,
}

export interface SlideParam {
    value: string,
    style: {
        fontWeight?: string,
        fontSize?: string,
        color?: string,
        top?: string,
        left?: string,
        right?: string,
        bottom?: string,
    }
}

export interface InputTextProps {
    type?: string,
    placeholder?: string,
    iconName?: string,
    list?: string,
    value?: string,
    isDisabled?: boolean,
    minlength?: number,
    maxlength?: number
}

export interface CheckboxItemProp {
    id: number,
    name: string,
}

export interface DefaultSelectItemProp {
    text: string,
    value: number,
}

export interface ImageProps {
    size?: { x: number, y: number},
    isUser?: boolean,
    image?: string,
    isIncrease?: boolean,
}

export interface InputNumberProps {
    placeholder?: string,
    min?: number,
    max?: number,
    value?: string,
}

export interface SearchSortOption {
    id: number,
    name: string,
}

export interface RecipeFace {
    image?: string,
    author: Author,
    created: string,
    rating: number,
    rate: number,
    favourite: boolean,
}

export interface RecipeDescription {
    time: number,
    personCount: number,
    description?: string,
}
