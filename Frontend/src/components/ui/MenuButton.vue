<template>
    <RouterLink 
        :to="props.route_path" 
        class="flex items-center p-2 rounded-lg hover:bg-gray-900 transition-colors hover:text-yellow-600 :text-yellow-500"
    >
        <component :is="iconMap[props.icon]" class="w-6 h-6" />
        <span class="ml-3">{{props.message}}</span>
    </RouterLink>    
</template>

<script setup lang="ts">
import { RouterLink } from 'vue-router';

import { ref, type Component } from 'vue'; 
import HomeIcon from '@/components/icons/HomeIcon.vue';

import LoginIcon from '@/components/icons/LoginIcon.vue';
import LogoutIcon from '@/components/icons/LogoutIcon.vue';
import SettingsIcon from '../icons/SettingsIcon.vue';

import AboutIcon from '@/components/icons/AboutIcon.vue';
import GeneratorIcon from '../icons/GeneratorIcon.vue';

import ProjectsIcon from '../icons/ProjectsIcon.vue';
import TeamsIcon from '../icons/TeamsIcon.vue';
import UsersIcon from '../icons/UsersIcon.vue';

// const AppColor = ref("#fbbf24")

const iconMap: { [key: string]: Component } = {
  home: HomeIcon,
  login: LoginIcon,
  about: AboutIcon,
  generator: GeneratorIcon,
  settings: SettingsIcon,
  logout: LogoutIcon,
  projects: ProjectsIcon,
  teams: TeamsIcon,
  users: UsersIcon,

};

const props = defineProps<{
    icon: string
    message: string
    route_path: string
    }>()
</script>

<style>
.router-link-exact-active {
  background-color: #4A5568;
  font-weight: bold;
}
.router-link-exact-active span {
  color: #f8c146; 
}

.router-link-exact-active svg {
  color: #f8c146; 
}
</style>