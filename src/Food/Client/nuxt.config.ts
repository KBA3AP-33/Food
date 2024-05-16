export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ['@pinia/nuxt', 'nuxt-icons'],
  css: ['~/assets/main.css', '~/assets/base.css'],
  plugins: ['~/plugins/service.ts', '~/plugins/api.ts'],
  ssr: true,
  routeRules: {
    "/": {
        redirect: {
          to: "/catalog",
          statusCode: 301
        },
    },
  },
  app: {
    head: {
      htmlAttrs: {
        lang: 'ru'
      }
    }
  }
})
