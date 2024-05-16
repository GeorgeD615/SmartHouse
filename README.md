# SmartHouse

# Отчет о выполнении проекта по дисциплине "Программная инженерия кибериммунных систем" по теме "Cистема управления умным домом"

- [Постановка задачи](#постановка-задачи)
- [Определение ценностей продукта и негативных событий](#определение-ценностей-продукта-и-негативных-сценариев)
- [Роли и пользователи](#роли-и-пользователи)
- [Цели и предположения безопасности](#цели-и-предположения-безопасности)
- [Контекстная диаграмма](#контекстная-диаграмма)
- [Базовые сценарии](#базовые-сценарии)
- [Оновные блоки](#основные-блоки)
- [Архитектура](#архитектура)
- [Базовый сценарий и HLA](#базовый-сценарий-и-hla)
- [Негативные сценарии](#негативные-сценарии)
- [Политика архитектуры](#политика-архитектуры)
- [Тесты](#тесты)
- [Выводы](#выводы)


## Постановка задачи
Задача: создать прототип автоматизированной системы управления всеми приборами в доме, которые объединены в единую экосистему.

Система может сама принимать решения и выполнять определенные задачи, без участия человека. Владельцу остается лишь управлять дистанционно путем ввода команд в консоль.

## Определение ценностей продукта и негативных сценариев
| Ценность | Негативные события | Величина ущерба | Комментарий |
|:----------:|:----------:|:----------:|:----------:|
| Люди | Причинение вреда здоровью пользователя | Высокий | Возможны судебные иски |
| Имущество дома | Причинение ущерба критической инфраструктуре или имуществу третьих лиц | Высокий | Судебные иски, возможно требование возмещения ущерба |
| Компоненты системы управления | Выход из строя одного/нескольких компонентов системы | Средний | Система и её компоненты застрахованы |

## Роли и пользователи
| Роль | Описание |
|:----------:|:----------:|
| Владелец дома (жильцы) | могут задавать сценарии поведения системы
| |имеют возможеость обращения к конкретным элементам системы |
| Гости дома | могут обращаться к элементам системы|
||могут пользоваться готовыми сценариями|
||не имеют полного доступа к элементами системы |

## Цели и предположения безопасности
### Цели безопасности
1. При любых обстоятельствах выполняются только подходящие для жизни системы сценарии

2. При любых обстоятельствах доступ к компонентам системы, которые оказывают влияние на самочувствие человека, есть только у авторизованных пользователей.

### Предположения безопасности
1. при любых обстоятельствах система получает только допустимый объём электричества.

2. авторизованные пользователи ознакомлены с набором рекомендованых команд*

 *команды, которые не наносят прямой ущерб здоровью пользователя 

## Контекстная диаграмма
![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/7cbba2d4-4c5f-416b-b401-abd392c96106)


## Базовые сценарии
![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/93712598-9187-4619-b05c-46e1d49d897d)


## Основные блоки
![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/f3cd1567-e51b-4fdf-b875-a9eefec2734e)


## Архитектура
![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/6561a236-65c9-42d4-87e9-3459087f99b5)


## Базовый сценарий и HLA
Основные этапы работы системы

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/5be415bb-384c-46b3-9def-4446e0162334)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/7487af0b-e12c-47f2-815f-f22fa0645bc8)


## Негативные сценарии
![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/f3bd289f-ba95-4437-8061-15aca7966b69)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/cb37220d-cc86-41fe-93d5-47c04b2c820f)


## Политика архитектуры
### Итерации разработки
Начальный этап разработки

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/6ec0ab25-3972-4e18-8df9-d438f98631b5)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/ee6e081c-0aa5-473e-8da6-202ca83c81e6)


Декомпозиция контроллера(хаба)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/b94479df-f285-4c6d-8a5a-5c735b2a0aae)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/8320328b-fa14-4b8b-b4e0-8bc8174aa130)


Декомпозиция внешних элеменов на критичные и некритичные

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/d57c1463-0839-428c-b7b7-d720dfea1969)

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/d2773038-0706-425e-8ab1-49d90d53564c)

## Тесты

## Выводы

В результате работы над прототипированием системы управления умным домом команде удалось познакомиться и в теории, и на практике с кибериммунным подходом к разработке, появилось понимание о том, из каких этапов состоит построение нового it-продукта. 

Интересным показался опыт разделения ролей в команде. Так в результате обсуждений архитектуры системы, когда часть команды защищала интересы бизнеса, а другая часть отвечала за возможности разработки, был найден компромисс, который лёг в основу текущей реализации проекта.

После окончания работ по реализации мы решили оценить насколько хорошо представляли объёмы и сложность работ до их начала.

Оценка объемов и сложности разработки до начала реализации:

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/e150c094-b054-4770-8b56-2f8e218267d9)

Обновление оценок после реализации:

![image](https://github.com/GeorgeD615/SmartHouse/assets/91796705/c5aaf3a3-96ad-4b85-8c4e-24694f663345)




