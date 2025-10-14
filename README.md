![C#](https://img.shields.io/badge/C%23-239120.svg?logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?logo=dotnet&logoColor=fff)
![VueJS](https://img.shields.io/badge/Vue.js-35495e.svg?logo=vue.js&logoColor=4FC08D)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC.svg?logo=typescript&logoColor=white)
![Tailwind CSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?logo=tailwindcss&logoColor=white)
![Microsoft SQL Server](https://custom-icon-badges.demolab.com/badge/Microsoft%20SQL%20Server-CC2927?logo=mssqlserver-white&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)

В будущем будут добавлены:

![Redis](https://img.shields.io/badge/Redis-DC382D?logo=redis&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white)

# Team Task Orchestrator 
## Корпоративная система управления задачами
### Приложение было разработано для управления проектами и задачами предприятия.

Когда-нибудь тут нужно написать крутое ридми...

В описании можно указать чёто типо почему я выбрал именно эти технологии, что конкретно применялось. Ещё вот эти красивые гитхабовские иконки и всякое такое.

Потом обязательно заскриншотим или вставим гифки (если можно) Корочееее, дел много...

Система ролей:

Пользователь -> Модератор -> Тимлид -> Админ

**Что может Пользователь:**
- Смотреть задачи
- Отправлять задачи на проверку (request)
- Задавать вопросы по задаче
- Перемещать статус задачи
- Отправлять задачи на рассмотрение модератору

**Что может Модератор:**
- Всё, что может Пользователь
- Назначать задачи пользователю
- Редактировать задачи пользователя
- Писать сообщениями ответы на вопросы
- Принимать задачи из статуса на рассмотрение

**Что может Тимлид:**
- Всё, что может Модератор
- Создать команду
- Создать проект
- Назначить модератора
- Назначить крупные проектные задачи 

**Что может Админ:**
- Всё, что может Админ
- Назначить тимлида
- Имеет доступ к админской панели
- Может просматривать статистику
