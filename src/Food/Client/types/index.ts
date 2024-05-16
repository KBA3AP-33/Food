export interface Catalog {
    start: number,
    limit: number,
    recipes: Array<CatalogRecipe>,
    count: number,
}

export interface CatalogRecipe {
    id: number,
    created: string,
    name: string,
    image?: string,
    rating: number,
    author: Author,
    status?: {
        id: number,
        name: string
    },
}

export interface Author {
    id: string,
    userName: string,
    image: string,
}

export interface Recipe {
    id: number,
    created: string,
    name: string,
    description?: string,
    image?: string,
    author: Author,
    time: number,
    personCount: number,
    rating: number,
    ingredients: Array<IngredientCount>,
    plan: Array<string>,
    status: Status,
    timeCategory: Category,
    category: Category,
    kitchen: Category,
    userRatings: Array<{ userId: string, rate: number }>,
}

export interface RecipeEdit {
    id: number,
    created: string,
    name: string,
    description?: string,
    image?: string,
    author: Author,
    time: number,
    personCount: number,
    rating: number,
    ingredients: Array<IngredientCount>,
    plan: Array<string>,
    statusId: number,
    message: string,
    timeCategoryId: number,
    categoryId: number,
    kitchenId: number,
}

export interface IngredientCount {
    id: number,
    ingredientName?: string,
    ingredient: Ingredient | null,
    count: number,
    units: string,
}

export interface Ingredient {
    id: number,
    name: string, 
}

export interface Status {
    id: number,
    name: string,
}

export interface User {
    userId: string,
    userName: string,
    accessToken: string,
    refrechToken: string,
    time: string,
    roles: Array<string>,
    favourites: Array<number>,
}

export interface FilterParams {
    time: number,
    start: number,
    limit: number,
    search: string,
    author: string,
    category: number,
    timeCategory: number,
    kitchen: number,
    sort: number,
}

export interface Filters {
    categories: Array<Category>,
    ingredients: Array<Ingredient>,
    timeCategories: Array<Category>,
    kitchens: Array<Category>,
    statuses: Array<Category>,
}

export interface Category {
    id: number,
    name: string,
}

export interface Ribbon {
    recipeId: number,
    ribbons: Array<string>,
}
