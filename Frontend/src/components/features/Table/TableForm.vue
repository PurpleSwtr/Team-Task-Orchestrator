<template>
    <div class="bg-white border-2 border-gray-300 rounded-3xl shadow-xl p-5 overflow-auto">
        <table class="w-full border-separate border-spacing-y-3">
            <thead>
                <tr>
                    <th class="text-left font-bold text-gray-500 p-3"></th>
                    <th v-for="column in columns" :key="column.key" class="text-left font-bold text-gray-500 p-3">
                        {{ column.label }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <TableLine v-for="item in items" :key="item.id" :element="item"/>
            </tbody>
        </table>
    </div>
</template>

<script setup lang="ts">
import TableLine from './TableLine.vue';
import type { UserData } from '@/types/tables';

const props = defineProps<{
    items: UserData[], 
    columns: { key: string, label: string }[] 
}>();
</script>