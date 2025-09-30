/// <reference types="vite/client" />

// 👇 Вот эту часть нужно добавить
declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export default component
}