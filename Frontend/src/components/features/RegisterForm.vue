<template>
    <div class="flex flex-col items-center w-full max-w-md bg-gray-100 pt-30 rounded-3xl shadow-2xl shadow-gray-300">
      <h1 class="text-3xl font-bold mb-6 text-gray-700">Регистрация</h1>
      <div class="flex flex-col gap-4 w-full">
        <AppInput 
        i_type="email" 
        i_placeholder="Email" 
        v-model="email"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Фамилия" 
        v-model="secondName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Имя" 
        v-model="firstName"/>
        <AppInput 
        i_type="text" 
        i_placeholder="Отчество" 
        v-model="lastName"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Пароль" 
        v-model="password"/>
        <AppInput 
        i_type="password" 
        i_placeholder="Подтверждение пароля" 
        v-model="password_check"/>
        <div class="flex justify-center space-x-8 py-4">
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="one" value="Male" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-blue-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-blue-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-blue-600 font-medium transition-colors duration-200">Мужчина</span>
          </label>
          
          <label class="flex items-center space-x-3 cursor-pointer group">
            <input type="radio" id="two" value="Female" v-model="gender" class="hidden" />
            <span class="w-6 h-6 border-2 border-gray-300 rounded-full flex items-center justify-center group-hover:border-pink-500 transition-all duration-200">
              <span class="w-3 h-3 rounded-full bg-transparent group-has-[:checked]:bg-pink-500 transition-all duration-200"></span>
            </span>
            <span class="text-gray-700 group-has-[:checked]:text-pink-600 font-medium transition-colors duration-200">Женщина</span>
          </label>
        </div>
        
        <Transition name="fade" mode="out-in">
          <p v-if="errorMessage" class="text-red-500 self-center px-5 text-pretty" key="error-message">
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
import { ref, inject, reactive } from 'vue';
import { useRouter } from 'vue-router';
import AppButton from '@/components/ui/AppButton.vue';
import apiClient from '@/api';
import AppInput from '../ui/AppInput.vue';

const emit = defineEmits(['switchToLogin']);
const router = useRouter();
const auth = inject('auth') as { 
  setLoggedIn: () => void;
  checkAuthStatus: () => Promise<void>; // Убедись, что checkAuthStatus прокинут через provide
};
const buttonLoading = ref(false);

const user = reactive({
  firstName: '',
  secondName: '',
  lastName: '',
  email: '',
  password: '',
});
const firstName = ref('');
const secondName = ref('');
const lastName = ref('');

const email = ref('');

const password = ref('');
const password_check = ref('');

const gender = ref('');

const errorMessage = ref('')

const tryRegister = async () => {
    errorMessage.value = '';
    
    if (password.value == password_check.value) {
      try {
          buttonLoading.value = true; 
          await apiClient.post('/Auth/register', {
            firstName: firstName.value,
            secondName: secondName.value,
            lastName: lastName.value,
            gender: gender.value,
            email: email.value,
            password: password.value
          });
          await apiClient.post('/Auth/login', {
            email: email.value,
            password: password.value
          });
          
          if (auth) {
            auth.setLoggedIn();
            await auth.checkAuthStatus();
            await router.push('/');
          }
          
          await router.push('/'); 
      } catch (error: any) {

        if (error.response && error.response.data && Array.isArray(error.response.data)) {
          errorMessage.value = error.response.data.map((e: any) => e.description).join(' ');
        }
        else{
          errorMessage.value = "Не все поля заполненны, либо не соответсвуют требованиям."
        }
        console.error('Ошибка при регистрации:', error);
      } finally {
        buttonLoading.value = false; 
      }
    }
    else
    {
      errorMessage.value = 'Пароли несовпадают!';
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