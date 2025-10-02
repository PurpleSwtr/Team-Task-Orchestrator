<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
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
        <AppButton @click="tryLogin" message="Войти" class="mx-40 mb-10"></AppButton>
        <div class="text-center">
        <p class="inline">У вас нет аккаунта? </p>
        <button class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700">Зарегистрироваться</button>
      </div>
        <div class="pb-15"></div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue'

import AppButton from '@/components/ui/AppButton.vue';
import axios from 'axios';

const password = ref('')
const email = ref('')


const tryLogin = async () => {
  try {
    console.log('click');
    const url = `http://localhost:8080/api/Auth/login`;
    console.log(url, {
      email: email.value,
      password: password.value
    });
    // Правильно передаем данные в теле запроса
    const response = await axios.post(url, {
      email: email.value,
      password: password.value
    });
    console.log(response)
  } catch (error) {
    // Обрабатываем различные типы ошибок
    console.error('Ошибка при входе:', error);
    
  }
};
  
</script>

<style>

</style>