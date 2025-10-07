<template>
    <tr class="group transition-colors">
        <td class="bg-gray-100 group-hover:bg-gray-200 px-4 py-3 rounded-l-xl border-y-2 border-l-2 border-gray-200">
            <AppIcon icon_name="miniuser" class="mx-auto text-gray-700"/>
        </td>

        <template v-for="(value, key) in props.element">
            <TableElement v-if="key !== 'id'">{{ value || '—' }}</TableElement>
        </template>
    </tr>
</template>

<script setup lang="ts">
import AppIcon from '@/components/ui/AppIcon.vue';
import TableElement from './TableElement.vue';
import type { LineData } from '@/types/tables';

const props = defineProps<{
    element: LineData 
}>();
</script>