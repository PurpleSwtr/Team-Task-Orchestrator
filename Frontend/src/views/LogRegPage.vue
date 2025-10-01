<template>
  <div class="min-h-screen flex items-center justify-center">
    <div class="flex flex-col items-center w-full max-w-md">
      <h1 class="text-3xl font-bold mb-6">Страница входа</h1>
      <div class="flex flex-col gap-4 w-full">
        <input 
          type="email" 
          placeholder="Email" 
          class="p-2 border rounded"
        >
        <input 
          type="password" 
          placeholder="Пароль" 
          class="p-2 border rounded"
        >
        <button class="p-2 bg-blue-500 text-white rounded hover:bg-blue-600">
          Войти
        </button>
      </div>
    </div>
  </div>
</template>

<style>
.placeholder-email{
  padding: 2;
  border: rounded;
}
</style>