<script setup lang="ts">
import type { UserData, UserDataForAdmin } from '@/types/tables';
import AppButton from '@/components/ui/AppButton.vue'; 
import AppIcon from '../ui/AppIcon.vue';
import AppWarnButton from '../ui/AppWarnButton.vue';
import { ref, watch } from 'vue';
import { useApiAsyncPatch } from '@/composables/useApi';

const props = defineProps<{
  userData: UserDataForAdmin | null 
  isOpen: boolean
}>();

const emit = defineEmits(['close', 'update']);

const onClose = () => {
  emit('close'); 
};

const onChange = async () => {
  try
  {
    if(props.userData && Object.keys(selectedRoles.value).length !== 0) {
      const payload = {
        userId: props.userData.id,
        roles: selectedRoles.value
      }
      
      await useApiAsyncPatch('/Users/ChangeRole', payload);
      
      console.log("Роли успешно обновлены!");
      emit('update');
      onClose();
    }
    else {
      throw new Error("Данные пользователя отсутствуют или роль не выбрана!")
    }
  }
  catch(error)
  {
    console.error("Ошибка при обновлении ролей:", error)
  }
};

const userRoles = ref([
    { key: 'User', label: 'Пользователь' },
    { key: 'Teamlead', label: 'Тимлид' },
    { key: 'Moderator', label: 'Модератор' },
    { key: 'Admin', label: 'Админ' },
]);

const selectedRoles = ref<string[]>([]);


watch(() => props.userData, (newUserData) => {
  if (newUserData && newUserData.roles) {

    selectedRoles.value = [...newUserData.roles].flat();
  } else {

    selectedRoles.value = [];
  }
});

</script>

<template>
  <Transition name="fade">
    <div v-if="isOpen" class="fixed inset-0 backdrop-saturate-100 backdrop-opacity-100 backdrop-blur-sm backdrop-brightness-75 flex items-center justify-center" @click.self="onClose">
      <div class="bg-gray-50 rounded-2xl shadow-2xl max-w-2xl w-full p-16 mx-4 relative overflow-hidden hover:scale-105 hover:-translate-y-5 duration-300">
        <template v-if="userData">
                <div class="flex justify-center -mt-5">
                  <div class="flex items-center justify-center bg-gray-300 w-32 h-32 rounded-full border-0 relative overflow-hidden">
                    <AppIcon icon_name="miniuser" class="absolute scale-600 text-white mt-10"></AppIcon>
                  </div>
                </div>
                <div class="text-center mt-2">
                  <h1 class="text-3xl font-bold text-gray-700 mb-2 inline">{{ userData.firstName + ' ' + userData.secondName + ' ' + userData.lastName}} </h1>
                </div>
                <p class="text-gray-600 font-bold mt-4">Email:</p>
                <p class="text-gray-600 mb-4">{{userData.email }}</p>
                <p class="text-gray-600 font-bold mt-4">Пол:</p>
                <p class="text-gray-600 mb-4">{{userData.gender === 'Male' ? 'Мужчина' : userData.gender === 'Female' ? 'Женщина' : ''}}</p>
                <p class="text-gray-600 font-bold mt-4 mb-1">Права доступа:</p>
                <div id="roles" v-for="value in userRoles">
                  <input 
                    type="checkbox" 
                    class="mr-2" 
                    :value="value.key"
                    :id="value.key"
                    v-model="selectedRoles"
                  ></input>
                  <p class="text-gray-600 mb-4  inline">{{value.label}}</p>
                </div>
                <p class="text-gray-600 font-bold mt-4">Дата регистрации:</p>
                <p class="text-gray-600 mb-4">{{userData.registrationTime.slice(0,10).replace(/-/g, ".") }}</p>
              </template>
            <template v-else>
                 <p class="text-gray-600">Загрузка данных...</p>
            </template>
            
            <div class="flex justify-start mt-10">
              <AppButton message="Закрыть" @click="onClose" />
            </div>
            <AppWarnButton message="Применить изменения" class="mt-3" @click="onChange" />
            <AppIcon icon_name="miniuser" 
              class="absolute right-25 bottom-32 scale-1800 text-gray-200">
            </AppIcon>
        </div>
    </div>
  </Transition>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>