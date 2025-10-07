import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'
import HomePage from '../views/HomePage.vue'
import AboutPage from '../views/AboutPage.vue'
import SettingsPage from '@/views/SettingsPage.vue'

// import LoginPage from '@/views/auth/LoginPage.vue'
// import RegisterPage from '@/views/auth/RegisterPage.vue'

import GeneratorPage from '../views/GeneratorPage.vue'
import StatisticsPage from '@/views/StatisticsPage.vue'

import TasksPage from '@/views/TasksPage.vue'
import TeamsPage from '@/views/TeamsPage.vue'
import ProjectsPage from '@/views/ProjectsPage.vue'
import UsersPage from '@/views/UsersPage.vue'
import LoginRegisterPage from '@/views/auth/LoginRegisterPage.vue'

export const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout, 
      children: [ 
        {
          path: '', 
          name: 'home',
          component: HomePage,
        },
        {
          path: 'about',
          name: 'about',  
          component: AboutPage,
        },
        {
          path: 'login_register', 
          name: 'login_register',  
          component: LoginRegisterPage,
        },

        {
          path: 'generator', 
          name: 'generator',  
          component: GeneratorPage,
        },
        {
          path: 'settings', 
          name: 'settings',  
          component: SettingsPage,
        },
        {
          path: 'projects', 
          name: 'projects',  
          component: ProjectsPage,
        },
        {
          path: 'teams', 
          name: 'teams',  
          component: TeamsPage,
        },
        {
          path: 'users', 
          name: 'users',  
          component: UsersPage,
        },
        {
          path: 'statistics', 
          name: 'statistics',  
          component: StatisticsPage,
        },
        {
          path: 'tasks', 
          name: 'tasks',  
          component: TasksPage,
        },
        {
          path: 'logout', 
          name: 'logout',  
          component: MainLayout,
        },
      ],
    },
  ],
})

export default router