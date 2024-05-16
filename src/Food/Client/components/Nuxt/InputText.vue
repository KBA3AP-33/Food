<template>
    <div class="input-container">
        <div v-if="settings.iconName">
            <div :class="['icon', settings.iconName]"></div>
        </div>
        <div class="input">
            <input
                :id="id"
                :placeholder="settings.placeholder"
                :type="settings.type"
                :list="settings.list"
                :disabled="settings.isDisabled"
                v-model="currentValue"
                :minlength="settings.minlength"
                :maxlength="settings.maxlength" 
                required>
            <span class="focus-border"></span>
        </div>
    </div>
</template>

<script lang="ts">
    import type { PropType } from 'vue';
    import type { InputTextProps } from '~/types/page';

    export default {
        data: () => ({
            currentValue: '',
        }),
        props: {
            id: {
                type: String as PropType<string>,
                required: true
            },
            settings: {
                type: Object as PropType<InputTextProps>,
                default: {
                    type: 'text',
                    placeholder: '',
                    iconName: '',
                    list: '',
                    value: '',
                    isDisabled: false,
                    minlength: 5,
                    maxlength: 20
                } as InputTextProps
            },
        },
        mounted() {
            this.reset();
        },
        methods: {
            reset() {
                if (this.settings.value) {
                    this.currentValue = this.settings.value;
                }
            }
        }
    }
</script>

<style scoped>
    .input-container {
        min-width: 300px;
        display: flex;
        align-items: center;
        gap: 7px;
    }

    .input {
        width: 100%;
        height: 2rem;
        position: relative;
    }

    input {
        width: 100%;
        height: 100%;
        background-color: var(--default-color-white);
        font-size: .95rem;
        font-weight: 400;
        padding: 0 5px 0 10px;
        border-radius: 3px;
    }

    .icon {
        width: 25px;
        height: 25px;
        background-size: cover;  
        background-repeat: no-repeat;
        pointer-events: none;
        opacity: .8;
    }

    .icon-person-white {
        background-image: var(--icon-person-white);
    }

    .icon-key-white {
        background-image: var(--icon-key-white);
    }

    .icon-key-black {
        background-image: var(--icon-key-black);
    }

    .focus-border {
        display: none;
    }

    .alternative-style.input-container {
        gap: 10px;
    }

    .alternative-style input:focus{
        outline: none;
    }

    .alternative-style input {
        color: var(--default-color-gray);
        background-color: transparent;
        border: 0;
        border-bottom: 1px solid var(--default-color-lightgray);
        border-radius: 0;
    }

    .alternative-style input::placeholder {
        color: var(--default-color-lightgray);
    }

    .alternative-style .focus-border {
        display: initial;
    }

    .alternative-style input ~ .focus-border {
        width: 0;
        height: 2px;
        background-color: var(--default-color-green);
        position: absolute;
        bottom: 0;
        left: 50%;
        transition: 0.4s;
    }

    .alternative-style input:focus ~ .focus-border {
        width: 100%;
        transition: 0.4s;
        left: 0;
    }

    @media (max-width: 768px) {
        .input-container {
            min-width: 290px;
        }
    }
</style>
