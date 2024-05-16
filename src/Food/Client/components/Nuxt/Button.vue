<template>
    <button class="button">
        <slot></slot>
    </button>
</template>

<style scoped>
    .button {
        height: 2.5rem;
        font-size: .75rem;
        font-weight: 600;
        padding: 0 1rem;
        white-space: nowrap;
        letter-spacing: 1px;
        text-transform: uppercase;
        color: var(--default-button-color);
        background-color: var(--default-color-white);
        border: 1px solid var(--default-color-lightgray);
        border-radius: 2.5rem;
        transition: border-color 0.3s ease, background-color 0.3s ease;
        cursor: pointer;
    }

    .red {
        background-color: var(--default-color-red);
        border-color: var(--default-color-red);
        color: var(--default-color-white);
    }

    .green {
        background-color: var(--default-color-green);
        border-color: var(--default-color-green);
        color: var(--default-color-white);
    }

    .button:hover {
        border-color: var(--default-color-black);
    }

    .button:disabled {
        background-color: var(--default-color-disabled);
        color: var(--default-background-disabled);
        pointer-events: none;
    }

    .red:hover {
        background-color: var(--default-color-lightred);
        border-color: var(--default-color-lightgray);
    }

    .green:hover {
        background-color: var(--default-color-lightgreen);
        border-color: var(--default-color-lightgray);
    }

    @media (max-width: 425px) {
        .button {
            height: 2rem;
            font-size: .75rem;
        }
    }
</style>
