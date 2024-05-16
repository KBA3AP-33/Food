<template>
    <footer class="triple-table">
        <div></div>
        <div class="footer">
            <ul class="social-icon" @click.prevent>
                <li class="social-icon-item">
                    <a class="social-icon-link" href="#">
                        <div class="icon facebook"></div>
                    </a>
                </li>
                <li class="social-icon-item">
                    <a class="social-icon-link" href="#">
                        <div class="icon telegram"></div>
                    </a>
                </li>
                <li class="social-icon-item">
                    <a class="social-icon-link" href="#">
                        <div class="icon youtube"></div>
                    </a>
                </li>
                <li class="social-icon-item">
                    <a class="social-icon-link" href="#">
                        <div class="icon twitter"></div>
                    </a>
                </li>
            </ul>
            <ul class="menu" @click.prevent>
                <li class="menu-item">
                    <a class="menu-link" href="#">Home</a>
                </li>
                <li class="menu-item">
                    <a class="menu-link" href="#">About</a>
                </li>
                <li class="menu-item">
                    <a class="menu-link" href="#">Services</a>
                </li>
                <li class="menu-item">
                    <a class="menu-link" href="#">Team</a>
                </li>
                <li class="menu-item">
                    <a class="menu-link" href="#">Contact</a>
                </li>
            </ul>
            <p class="copyright">&copy; 2024 EASY-COOK | All Rights Reserved</p>
        </div>
        <div></div>
    </footer>
</template>

<style scoped>
    .footer {
        border-left: 1px solid var(--default-color-gray);
        border-right: 1px solid var(--default-color-gray);
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
    }

    .social-icon,
    .menu {
        position: relative;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-wrap: wrap;
        margin: 10px 0;
        gap: 20px;
    }

    .social-icon-item,
    .menu-item {
        list-style: none;
    }

    .social-icon-link {
        display: block;
        transition: .3s;
    }

    .social-icon-link:hover {
        transform: translateY(-10px);
    }

    .icon {
        width: 30px;
        height: 30px;
        background-repeat: no-repeat;
        background-size: cover;
        background-position: center;
    }

    .facebook {
        background-image: var(--icon-facebook);
    }

    .telegram {
        background-image: var(--icon-telegram);
    }

    .youtube {
        background-image: var(--icon-youtube);
    }

    .twitter {
        background-image: var(--icon-twitter);
    }

    .menu-link {
        font-size: 1rem;
        font-weight: 500;
        text-decoration: none;
        color: var(--default-color-black);
        transition: .5s;
        opacity: .65;
    }

    .menu-link:hover {
        color: var(--default-color-red);
        opacity: 1;
    }

    .copyright {
        font-family: sans-serif;
        font-size: .8rem;
        font-weight: 300;
        margin: 10px 0;
    }
</style>
