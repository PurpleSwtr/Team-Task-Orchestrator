<template>
    <div class="loader"></div>
</template>

<style scoped>
.loader {
  /* 
    Мы привязываем размер лоадера к размеру шрифта родителя (кнопки).
    "width: 1em" означает, что ширина контентной части лоадера будет равна
    размеру шрифта. Например, для font-size: 16px ширина будет 16px.
  */
  width: 1em;
  aspect-ratio: 1;
  border-radius: 50%;
  
  /*
    Теперь самое главное: мы восстанавливаем оригинальные пропорции.
    В вашем коде соотношение рамки к ширине было 8px / 50px = 0.16.
    Мы сохраняем это соотношение, используя единицы em.
    `currentColor` автоматически возьмет цвет текста у родителя (в вашем случае - белый).
  */
  border: 0.16em solid currentColor;

  animation:
    l20-1 0.8s infinite linear alternate,
    l20-2 1.6s infinite linear;
}

/* Анимации не меняются, так как они работают с процентами от размера элемента */
@keyframes l20-1 {
   0%    {clip-path: polygon(50% 50%,0       0,  50%   0%,  50%    0%, 50%    0%, 50%    0%, 50%    0% )}
   12.5% {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100%   0%, 100%   0%, 100%   0% )}
   25%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 100% 100%, 100% 100% )}
   50%   {clip-path: polygon(50% 50%,0       0,  50%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   62.5% {clip-path: polygon(50% 50%,100%    0, 100%   0%,  100%   0%, 100% 100%, 50%  100%, 0%   100% )}
   75%   {clip-path: polygon(50% 50%,100% 100%, 100% 100%,  100% 100%, 100% 100%, 50%  100%, 0%   100% )}
   100%  {clip-path: polygon(50% 50%,50%  100%,  50% 100%,   50% 100%,  50% 100%, 50%  100%, 0%   100% )}
}
@keyframes l20-2 { 
  0%    {transform:scaleY(1)  rotate(0deg)}
  49.99%{transform:scaleY(1)  rotate(135deg)}
  50%   {transform:scaleY(-1) rotate(0deg)}
  100%  {transform:scaleY(-1) rotate(-135deg)}
}
</style>