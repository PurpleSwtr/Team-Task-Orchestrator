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
        <AppButton @click="tryLogin":statusLoading="buttonLoading" message="Войти" class="mx-40 mb-10"></AppButton>
        <div class="text-center">
        <p class="inline">У вас нет аккаунта? </p>
        <RouterLink 
        to="/register">
        <button class="inline text-blue-500 cursor-pointer underline-offset-2 hover:underline hover:text-blue-700">Зарегистрироваться</button>
        
        </RouterLink>
      </div>
        <div class="pb-15"></div>
      </div>
    </div>
</template>