<template>
  <div class="p-4 pl-7 ml-5 mt-10 bg-yellow-200 text-black rounded-lg w-80 max-w-full">
    <h1 class="text-lg font-semibold truncate">{{ props.tittle }}</h1>
    <p class="font-light line-clamp-3">{{ props.task }}</p>
    <div class="flex items-center gap-2 mt-2">
      <input type="checkbox" />
      <span class="text-sm">Выполнено</span>
    </div>
  </div>
</template>
<script setup>
const props = defineProps({
  tittle: String,
  task: String,
})

// name = "Задача"
// text = "Текст"
// list["task_1"] = {"name": name,"text": text}
// list["task_2"] = {"name": name,"text": text}
// list["task_3"] = {"name": name,"text": text}
// console.log(list)
</script>

<style></style>
