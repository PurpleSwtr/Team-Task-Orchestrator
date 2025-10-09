<template>
    <div class="bg-white border-2 border-gray-300 rounded-3xl shadow-xl p-5 overflow-auto">
        <table class="w-full border-separate border-spacing-y-3">
            <thead>
                <tr>
                    <!-- Заголовки рендерятся как и раньше -->
                    <th v-for="column in columns" :key="column.key" class="text-left font-bold text-gray-500 p-3">
                        {{ column.label }}
                    </th>
                </tr>
            </thead>
            <tbody>
                <!-- 
                    Вместо нашего старого компонента TableLine, мы итерируемся по items 
                    и для каждой строки предоставляем именованный слот 'row'.
                    В этот слот мы "прокидываем" текущий элемент `item`.
                -->
                <tr v-for="item in items" :key="item.id" class="group transition-colors">
                    <slot name="row" :item="item"></slot>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup lang="ts">
// Мы делаем пропс 'items' универсальным, принимающим любой массив.
// 'any' здесь допустим, так как компонент не работает с внутренней структурой объектов.
// В более строгом варианте можно использовать дженерики, но для начала и так отлично.
defineProps<{
    items: Array<any>, 
    columns: { key: string, label: string }[] 
}>();
</script>