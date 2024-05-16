<template>
    <div class="pagination" v-if="pagesCount > 1">
        <NuxtButton
            v-for="page in getPages"
            :key="page"
            @click="changePage(Number(page))"
            :class="[(Number(page) === currentPage) && 'red', 'pagination-buttons']"
            style="transition-duration: 0s;">
            {{ page }}
        </NuxtButton>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        props: {
            pagesCount: {
                type: Number as PropType<number>,
                required: true
            },
            currentPage: {
                type: Number as PropType<number>,
                required: true
            },
        },
        methods: {
            changePage(page: number) {
                if (!isNaN(page)) {
                    this.$emit('changePage', page);
                }
            }
        },
        computed: {
            getPages() {
                if (this.pagesCount < 10) return Array.from({ length: this.pagesCount }, (_, i) => `${i + 1}`);

                let first = 1, second = (this.pagesCount > 1) ? 2 : -1, penultimate = this.pagesCount - 1, last = this.pagesCount;
                let skip = '...';
                return [
                    `${first}`,
                    `${second}`,
                    (this.currentPage - 2 > second) && skip,
                    (this.currentPage > 1) && `${this.currentPage - 1}`,
                    `${this.currentPage}`,
                    (this.currentPage < last) && `${this.currentPage + 1}`,
                    (this.currentPage + 2 < penultimate) && skip,
                    `${penultimate}`,
                    `${last}`
                ].filter((value, index, self) => typeof(value) !=='boolean' && (index === self.findIndex((e) => (e === value))) || value === skip) as Array<string>;
            },
        },
    }
</script>

<style scoped>
    .pagination {
        display: flex;
        flex-wrap: nowrap;
        justify-content: center;
        align-items: center;
        gap: 10px;
        border-top: 1px solid var(--default-color-gray);
    }

    .pagination-buttons {
        font-size: 1rem;
    }

    @media (max-width: 672px) {
        .pagination {
            gap: 5px;
        }

        .pagination-buttons {
            height: 2rem;
            font-size: .75rem;
            padding: 0 11px;
        }
    }

    @media (max-width: 375px) {
        .pagination-buttons {
            height: 1.5rem;
            font-size: .75em;
            padding: 0 8px;
        }
    }
</style>
