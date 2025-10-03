<template>
  <div class="min-h-screen flex items-center justify-center pb-50">
    
  </div>
</template>

<script setup lang="ts">
  import {ref} from 'vue'
  import { RouterLink } from 'vue-router';
  import AppButton from '@/components/ui/AppButton.vue';
  import axios from 'axios';

  const password = ref('')
  const email = ref('')
  const buttonLoading = ref(false)

  const tryLogin = async () => {
    try {
      buttonLoading.value = !buttonLoading.value
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