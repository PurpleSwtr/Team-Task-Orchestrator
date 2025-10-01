<template>
    <!-- <span>Таблица: </span> -->
    <select v-model="selected" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables">{{table}}</option>
    </select>

    <input type="number" class="border rounded-md mr-4 py-1 bg-gray-100 border-gray-400 outline-0" placeholder="Количество генераций">


    <AppButton message='Сгенерировать' @click="GenStart"></AppButton>

</template>

<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import {ref} from 'vue'
let selected = ref('')
const tables = ref(['Задачи', 'Пользователи', 'Команды', 'Роли'])
function GenStart() {
// Будем отправлять в апишку данные наших плейсхолдеорв
};

</script>

<style scoped>

</style>