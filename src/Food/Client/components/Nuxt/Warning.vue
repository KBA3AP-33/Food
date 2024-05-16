<template>
    <div class="warning-container">
        <h3 class="warning-header">Предупреждение!!!</h3>
        <p>
            Весь контент представленный на сайте выдуман и создан исключительно для демонстрации работы сайта, 
            не пытайтесь повторить готовить по рецептам (это может быть опасно для жизни)!
        </p>
    </div>
</template>

<style scoped>
    .warning-container {
        max-width: 300px;
        background-color: #fccbd3;
        border-radius: 10px;
        padding: 10px;
        pointer-events: none;
        animation: disappearance 2s ease-in-out forwards;
        animation-delay: 5s;
    }

    .warning-header {
        margin-bottom: 10px;
    }

    @keyframes disappearance {
        from {
            opacity: 1;
        }
        to {
            opacity: 0;
            display: none;
        }
    }
</style>
