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
import apiClient from '@/api';
import AppButton from '@/components/ui/AppButton.vue';
import {onMounted, ref} from 'vue'
let selected = ref('')
const tables = ref<string[]>([])

const getTables = async () => {
    
    try {
    tables.value = (await apiClient.get('/Generator')).data
    }   
    catch {

    }

};

onMounted(getTables);

function GenStart() {
// Будем отправлять в апишку данные наших плейсхолдеорв
};

</script>

<style scoped>

</style>