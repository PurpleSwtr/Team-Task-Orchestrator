import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'
import HomePage from '../views/HomePage.vue'
import AboutPage from '../views/AboutPage.vue' // 👈 1. Импортируем новую страницу

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout, // 👈 2. Устанавливаем MainLayout как обертку
      children: [ // 👈 3. Все дочерние маршруты будут отображаться внутри MainLayout
        {
          path: '', // Пустой путь для главной страницы ('/')
          name: 'home',
          component: HomePage,
        },
        {
          path: 'about', // Путь для страницы "О проекте" ('/about')
          name: 'about',  
          component: AboutPage,
        },
      ],
    },
  ],
})

export default router