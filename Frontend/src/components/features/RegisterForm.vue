<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Регистрация</h1>
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
        <AppButton @click="tryRegister" :statusLoading="buttonLoading" message="Зарегистрироваться"
        class="self-center mb-10"/>
        <div class="text-center">
            <p class="inline">У вас уже есть аккаунт? </p>
            <button 
                @click="$emit('switchToLogin')"
                class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700"
            >
                Войти
            </button>
        </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>

<script setup lang="ts">
import { ref, inject } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';

const emit = defineEmits(['switchToLogin']);
const router = useRouter();
const auth = inject('auth') as { setLoggedIn: () => void };

const buttonLoading = ref(false);
const email = ref('');
const password = ref('');
const errorMessage = ref('')

const tryRegister = async () => {
    buttonLoading.value = true; 
    errorMessage.value = '';

    try {
      await apiClient.post('/Auth/register', {
        email: email.value,
        password: password.value
      });

      await apiClient.post('/Auth/login', {
        email: email.value,
        password: password.value
      });
      
      if (auth) {
        auth.setLoggedIn();
      }

      await router.push('/'); 

    } catch (error: any) {
      if (error.response && error.response.data && Array.isArray(error.response.data)) {
        errorMessage.value = error.response.data.map((e: any) => e.description).join(' ');
      } else {
        errorMessage.value = 'Ошибка при регистрации. Возможно, такой email уже занят.';
      }
      console.error('Ошибка при регистрации:', error);
    } finally {
      buttonLoading.value = false; 
    }
  };
</script>


<style scoped>
.fade-enter-active, .fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>