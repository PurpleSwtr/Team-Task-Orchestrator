import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'
import HomePage from '../views/HomePage.vue'
import AboutPage from '../views/AboutPage.vue'
import SettingsPage from '@/views/SettingsPage.vue'

import AdminPage from '@/views/AdminPage.vue'
import GeneratorPage from '../views/GeneratorPage.vue'
import StatisticsPage from '@/views/StatisticsPage.vue'

import AccountPage from '@/views/AccountPage.vue'
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
          path: 'account', 
          name: 'account',
          component: AccountPage,
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
          path: 'statictics', 
          name: 'statictics',   
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
        {
          path: 'admin_panel', 
          name: 'admin_panel',  
          component: AdminPage,
        },
      ],
    },
  ],
})

export default router