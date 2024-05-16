<template>
    <div class="textarea-container">
        <textarea
            :id="id"
            :placeholder="placeholder"
            v-model="text">
        </textarea>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';

    export default {
        data: () => ({
            text: '',
        }),
        props: {
            id: {
                type: String as PropType<string>,
                required: true
            },
            placeholder: {
                type: String as PropType<string>,
                default: ''
            },
            value: {
                type: String as PropType<string>,
                default: ''
            },
        },
        mounted() {
            this.text = this.value;
        },
    }
</script>

<style scoped>
    .textarea-container {
        width: 100%;
    }

    textarea {
        resize: none;
        width: 100%;
        min-height: 150px;
        font-size: .95rem;
        font-weight: 400;
        padding: 5px 10px;
        border: 2px solid;
        border-radius: 3px;
        background-color: var(--default-color-white);
    }
</style>
