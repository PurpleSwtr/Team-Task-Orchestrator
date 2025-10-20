<template>
  <div class="group p-15 pl-6 ml-5 mt-10 bg-gray-100 text-gray-700 relative overflow-hidden duration-500 rounded-2xl w-90 max-w-full hover:-translate-y-2 shadow-gray-300 hover:shadow-xl shadow-md hover:cursor-pointer hover:scale-103"
  >
    <AppIcon :icon_name="statusTask[status].icon_name"
    class="absolute right-5 bottom-7 scale-500 z-1
    group-hover:-translate-x-10
    group-hover:-translate-y-2
    duration-700
    group-hover:scale-300"

    :class="statusTask[status].icon_color"></AppIcon>
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
      statusTask[status].bg_color,
      statusTask[status].shadow_color,
    ]">
      <p class="font-semibold text-gray-600 text-center pt-5 text-sm">{{statusTask[status].title}}</p>
    </div>
    <h1 class="text-xl font-semibold truncate inline text-gray-700 relative z-10">{{ props.title }}</h1>
    <p class="font-normal line-clamp-3 text-gray-700 relative z-10">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2 relative z-10">
    <AppButton message="Статус" class="scale-75 absolute top-2" @click.stop="onChangeStatusChange"/>
      <!-- <input type="checkbox" /> -->
      <!-- <span class="text-sm text-gray-700">Выполнено</span> -->
    </div>
  </div>
</template>

<script setup lang="ts">
import AppButton from '@/components/ui/AppButton.vue';
import AppIcon from '@/components/ui/AppIcon.vue';
import { reactive, ref } from 'vue';

interface StatusDetail {
  title: string;
  bg_color: string;
  shadow_color: string;
  icon_color: string;
  icon_name: string;
}
const currentStatus = ref(0)

type StatusKey = keyof typeof statusTask;

// Типизируем ref
const status = ref<StatusKey>('appointed');

const statuses = ['appointed', 'process', 'waiting', 'updated', 'finished'] as const;

const onChangeStatusChange = () => {
  currentStatus.value = currentStatus.value < 4 ? currentStatus.value + 1 : 0;
  status.value = statuses[currentStatus.value]
  console.log(props.task_id)
};

// Статусы задач:

// Не начата / appointed
// В процессе / process
// Ожидает проверки / waiting
// Обновлена модератором / updated
// Завершена / finished


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
    title: "Ожидает проверки",
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

const props = defineProps<{
  task_id?: string,
  title?: string,
  task?: string,
}>();
</script>

<style></style>
