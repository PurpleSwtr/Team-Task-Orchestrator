<template>
  <div class="group p-15 pl-6 ml-5 mt-10 bg-gray-100 text-gray-700 relative overflow-hidden duration-500 rounded-2xl w-90 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103"
  >
    <!-- Используем localStatus вместо status -->
    <AppIcon :icon_name="statusTask[localStatus].icon_name"
    class="absolute right-5 bottom-7 scale-500 z-1
    group-hover:-translate-x-10
    group-hover:-translate-y-2
    duration-700
    group-hover:scale-300"

    :class="statusTask[localStatus].icon_color"></AppIcon>
    <div :class="[
      'w-25',
      'h-50',
      'absolute',
      'top-6',
      'right-6',
      'rounded-xl',
      'z-0',
      'duration-400',
      'group-hover:shadow-xl',
      statusTask[localStatus].bg_color,
      statusTask[localStatus].shadow_color,
    ]">
      <p class="font-semibold text-gray-600 text-center pt-5 text-sm">{{statusTask[localStatus].title}}</p>
    </div>
    <h1 class="text-xl font-semibold truncate inline text-gray-700 relative z-10">{{ props.title }}</h1>
    <p class="font-normal line-clamp-3 text-gray-700 relative z-10">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2 relative z-10">
    <AppButton message="Статус" class="scale-75 absolute top-2" @click.stop="onChangeStatusChange"/>
    </div>
  </div>
</template>

<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import { reactive, ref, watch } from 'vue';

interface StatusDetail {
  title: string;
  bg_color: string;
  shadow_color: string;
  icon_color: string;
  icon_name: string;
}

// Определяем ключи для типизации
type StatusKey = keyof typeof statusTask;
const statuses = ['appointed', 'process', 'waiting', 'updated', 'finished'] as const;

const props = defineProps<{
  task_id?: string,
  title?: string,
  task?: string,
  status?: string // Приходит как строка из API
}>();

// Создаем локальную переменную, инициализируем пропсом или дефолтным значением
// as StatusKey нужен, чтобы TS не ругался, что string не подходит под ключи объекта
const localStatus = ref<StatusKey>((props.status as StatusKey) || 'appointed');

// Если пропс обновится (например, перезагрузили данные), обновляем локальную переменную
watch(() => props.status, (newStatus) => {
  if (newStatus) {
    localStatus.value = newStatus as StatusKey;
  }
});

const onChangeStatusChange = () => {
  // Находим текущий индекс в массиве статусов
  const currentIndex = statuses.indexOf(localStatus.value);
  // Вычисляем следующий (зацикливаем через остаток от деления)
  const nextIndex = (currentIndex + 1) % statuses.length;

  localStatus.value = statuses[nextIndex];

  // В будущем тут можно отправить запрос на сервер:
  // emit('status-changed', localStatus.value);
  console.log(`Task ${props.task_id} status changed to: ${localStatus.value}`);
};

const statusTask = reactive({
  "appointed": {
    title: "Не начата",
    bg_color: "bg-amber-300",
    shadow_color: "shadow-amber-400",
    icon_color: "text-orange-400",
    icon_name: "task",
  },
    "process": {
    title: "В процессе",
    bg_color: "bg-gray-300",
    shadow_color: "shadow-gray-400",
    icon_color: "text-gray-400",
    icon_name: "processing",
  },
    "waiting": {
    title: "Проверяется",
    bg_color: "bg-blue-300",
    shadow_color: "shadow-blue-400",
    icon_color: "text-blue-400",
    icon_name: "audit",
  },
    "updated": {
    title: "Обновлена",
    bg_color: "bg-orange-400",
    shadow_color: "shadow-orange-400",
    icon_color: "text-red-700",
    icon_name: "reject",
  },
    "finished": {
    title: "Завершена",
    bg_color: "bg-green-600",
    shadow_color: "shadow-green-400",
    icon_color: "text-emerald-700",
    icon_name: "success",
  },
})
</script>

<style></style>
