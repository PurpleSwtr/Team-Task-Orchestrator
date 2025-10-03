<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Вход в аккаунт</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="email"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="bg-white p-2 mx-20 border-2 rounded-md border-gray-300 border-b-green-600 outline-blue-600"
          v-model="password"
        >
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center" key="error-message">
            {{ errorMessage }}
          </p>
        </Transition>
        <AppButton @click="tryLogin" :statusLoading="buttonLoading" message="Войти" class="self-center mb-10"/>
        <div class="text-center">
            <p class="inline">У вас нет аккаунта? </p>
            <button 
                @click="$emit('switchToRegister')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Зарегистрироваться
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';
const emit = defineEmits(['switchToRegister']);

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');
const errorMessage = ref('');

const tryLogin = async () => {
    buttonLoading.value = true;
    errorMessage.value = '';
    try {
      await apiClient.post('/Auth/login', {
        email: email.value,
        password: password.value
      });

      window.location.href = '/';

    } catch (error) {
      errorMessage.value = 'Неверный email или пароль.';
      console.error('Ошибка при входе:', error);
    } finally {
      buttonLoading.value = false;
    }
  };
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