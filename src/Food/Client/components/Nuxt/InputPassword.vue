<template>
    <div class="password">
        <NuxtInputText
            :id="id"
            ref="pass"
            :class="[(isAlternativeStyle) && 'alternative-style']"
            :settings="{
                type: (isVisible) ? `text` : `password`,
                placeholder,
                iconName: (isAlternativeStyle) ? 'icon-key-white' : 'icon-key-black'
            }"/>
        <div
            :class="['icon', (isVisible) ? `icon-eye-off-${(isAlternativeStyle) ? 'white' : 'black'}` : `icon-eye-${(isAlternativeStyle) ? 'white' : 'black'}`]"
            @click="isVisible = !isVisible">
        </div>
    </div>
</template>

<script lang="ts">
import type { PropType } from 'vue';

    export default {
        data: () => ({
            isVisible: false,
        }),
        props: {
            id: {
                type: String as PropType<string>,
                required: true
            },
            placeholder: {
                type: String as PropType<string>,
                default: 'Пароль'
            },
            isAlternativeStyle: {
                type: Boolean as PropType<boolean>,
                default: false
            }
        }
    }
</script>

<style scoped>
    .password {
        position: relative;
    }

    .icon {
        width: 20px;
        height: 20px;
        background-size: cover;  
        background-repeat: no-repeat;
        position: absolute;
        top: 7px;
        right: 12px;
        cursor: pointer;
    }
</style>
