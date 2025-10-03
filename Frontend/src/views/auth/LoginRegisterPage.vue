// views/auth/LoginRegisterPage.vue

<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    <Transition name="fade" mode="out-in">
      <component 
        :is="activeComponent" 
        @switch-to-register="activeComponentName = 'RegisterForm'"
        @switch-to-login="activeComponentName = 'LoginForm'"
      />
    </Transition>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, shallowRef } from 'vue';
import LoginForm from '@/components/features/LoginForm.vue';
import RegisterForm from '@/components/features/RegisterForm.vue';

const components = {
  LoginForm,
  RegisterForm
};

const activeComponentName = ref<'LoginForm' | 'RegisterForm'>('LoginForm');

const activeComponent = computed(() => components[activeComponentName.value]);
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>