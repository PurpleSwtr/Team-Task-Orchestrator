<template>

    <select v-model="selected" class="border rounded mr-4 py-1">
        <option disabled value="">Выберите таблицу</option>
        <option v-for="table in tables">{{table}}</option>
    </select>

    <input type="number" class="border rounded mr-4 py-1" placeholder="Количество генераций">


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