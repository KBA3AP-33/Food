<template>
    <Title>{{ (recipe) ? recipe.name : 'Рецепт' }}</Title>
    <div class="page-container">
        <div :class="['page-left-container', isActiveFilter && 'active']">
            <CatalogFilter
                redirect="../catalog"
                @close="isActiveFilter = false"/>
        </div>
        <div class="page-center-container" ref="page">
            <Recipe
                v-if="recipe !== null"
                :current-recipe="recipe"
                :favourite="authentication.user?.favourites.includes(recipe.id)"
                :rate="recipe.userRatings.find(e => e.userId === authentication.user?.userId)?.rate"
                @getPrinter="print(recipe.id)"
                @remove="remove(recipe.id)"
                @changeFavourite="authentication.changeFavourites(recipe.id)"
                @changeRating="async (rate) => changeRating(await authentication.changeRating(recipe.id, rate) as any)"
                @changeModalComponent="$emit('changeModalComponent', $event)"
                @changeVisibleFilter="isActiveFilter = !isActiveFilter"/>
        </div>
        <div class="page-right-container">
            <div class="user-catalog" v-if="otherRecipes.recipes?.length">
                <h2>Другие рецепты этого автора</h2>
                <CatalogItem
                    v-for="item in otherRecipes.recipes"
                    :key="item.id"
                    :item="item"
                    :is-favourite="authentication.user?.favourites.includes(item.id)"
                    @changeFavourite="authentication.changeFavourites(item.id)"
                    @click="$router.push(`../recipes/${item.id}`)"/>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
    import { authenticationStore } from "~/store/authentication";
    import { filterStore } from "~/store/filter";
    import type { Catalog, Recipe, User } from "~/types";

    definePageMeta({
        validate: ({ params }) => /^\d+$/.test(params.id as string),
    })

    const route = useRoute();
    const filter = filterStore();
    const isAuthorize = route.query.authorize === 'true';
    if (isAuthorize) {
        let user = useCookie('user').value as User | undefined;

        if (user) {
            const tokenModel = {
                accessToken: user.accessToken,
                refrechToken: user.refrechToken
            }
            
            let nextTokenModel: { accessToken: string, refrechToken: string, time: string } = 
            await useNuxtApp().$host(`api/account/refrech`, { method: 'POST', body: tokenModel });
            
            useCookie('user', { expires: new Date(nextTokenModel.time) }).value = JSON.stringify({
                ...user,
                accessToken: nextTokenModel.accessToken,
                refrechToken: nextTokenModel.refrechToken,
                time: nextTokenModel.time,
            });
        }
    }
    const recipe = ref(await filter.getRecipe(Number(route.params.id), isAuthorize, route.query.admin === 'true') as Recipe);
    const otherRecipes = await filter.getRecipesByWithoutChangeOptions(`limit=3&sort=1&author=${recipe.value.author?.userName}`) as Catalog;
    
    function changeRating(userRatings: { userId: string, rate: number, result: number }) {
        if (userRatings) {   
            recipe.value.rating = userRatings.result;
            recipe.value.userRatings = [...recipe.value.userRatings, { userId: userRatings.userId, rate: userRatings.rate}];
        }
    }
</script>

<script lang="ts">
    export default {
        data: () => ({
            authentication: authenticationStore(),
            isActiveFilter: false,
        }),
        emits: ['changeVisibleLoader', 'changeModalComponent'],
        async mounted() {
            this.$emit('changeVisibleLoader', false);

        },
        methods: {
            print(id: number) {
                window.open(this.$router.resolve(`/print/${id}`).href, '_blank');
            },
            async remove(id: number) {
                if (!window.confirm('Вы действительно хотите поставить рецепт на удаление?')) return;

                let result = await this.$authHost(`api/catalog/status/${id}`, 'PUT', {}, { statusId: 4 });
                if (Number(result) > 0) {
                    this.$router.go(-1);
                }
            }
        }
    }
</script>

<style scoped>
    .page-center-container {
        padding: 15px;
    }

    .page-right-container {
        padding: 10px;
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 20px;
    }

    .user-catalog {
        display: flex;
        flex-direction: column;
        align-items: center;
        gap: 20px;
    }
</style>
