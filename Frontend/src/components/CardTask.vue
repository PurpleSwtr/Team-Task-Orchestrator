<template>
  <div class="p-4 bg-yellow-200 text-black rounded-lg">
    <h1>{{props.tittle}}</h1>
    <p class="font-light">{{props.task}}</p>
<input class="font-light" type="checkbox"> Выполнено<input/>


</div>
</template> 

<script setup>
const props = defineProps({
  tittle: String,
  task: String
})

// name = "Задача"
// text = "Текст"
// list["task_1"] = {"name": name,"text": text}
// list["task_2"] = {"name": name,"text": text}
// list["task_3"] = {"name": name,"text": text}
// console.log(list)
</script>

<style>

</style>