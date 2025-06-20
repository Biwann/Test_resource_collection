## Краткое описание того, что получилось, а что нет.

### Игровое поле:
- открытая 2d сцена
- 2 базы на противоположных сторонах:
    один префаб, цвет устанавливается через компонент SelfInitializableUnit
- 2-10 дронов, разделенных поровну между фракциями (не реализовано, но понимаю как сделать: создать TestBaseController в который через инспектор положить SO DebugSettings и 2 базы + реализовать логику удаления и создания через FactionBase MovableUnitType.DroneCollector)
- ресурсы, спавнящиеся случайно через заданный интервал (не реализовано, но понимаю как сделать: создать ResourceSpawner, в котором поместить через инспектор ground и в границах которого вызывать Instantiate в заданный интервал времени в SO GameSettings)

### Поведение дронов:
реализовано через CollectingResourcesBehavior.cs
- найти свободный ресурс (SearchingResourceStrategy.cs)
- долететь до него (состояние такое есть, проблемы с NavMesh из-за чего не удалось реализовать) (CollectingResourceStrategy.cs)
- собрать ресурс (там же в CollectingResourceStrategy.cs, хотя можно было разделить полет и сбор)
- вернуться на базу (DeliverResourceToBaseStrategy.cs)
- выгрузить ресурс с частицой (не реализовано, но понимаю как, свойство для этого уже добавлено, но в коде нет Particles.Play())
- повторить цикл заново

! дроны не должны сталкиваться (не реализовано, тк возникли проблемы с NavMesh, хотя в интернете почитал, что можно вычислять в заданном круге приближающихся агентов и через векторную логику применять силу в противоположную сторону)

### Интерфейс:
не реализован вообще, но есть четкое понимание как это сделать

- Ползунок: кол-во дронов на фракцию (менять значение в SO DebugSettings и на событие DebugSettingsChanged будет применяться логика изменения кол-ва дронов)
- Ползунок: скорость дронов (изменить значение в SO DroneMovableParameters.asset и скорость поменяется)
- Ввод: изменить частоту генерации ресурсов (также через SO настроек GameSettings)
- Чекбокс: включение\откючение отрисовки пути дрона (есть настройки DebugSettingsChanged)
- Отображение счетчика собранных ресурсов (есть SO RadiantData и DireData, которые можно было бы поместить в какой нибудь FactionResourcesView, изменение на событии)

За счет того, что разделена логика от данных, легко можно подкрутить UI

### Опционально:
- выбор дрона для слежения (не реализовано)
- цветовое различие дронов по фракциям (реализовано см. DroneCollector.cs, цвет наследуется от базы, которая создала дрона)
- миникарта (не реализована, но под это продумана архитектура игровых элементов (все так или иначе наследуются от GameUnitBase.cs, можно использовать этот класс общий для того, чтобы отрисовывать все дочерние элементы на карте)
- управление скоростью симуляции (не реализовано)
- показ текущего состояния дрона при помощи цвета/иконки (не реализовано, но можно сделать завязку на конкретное состояние через компонент CollectingResourcesBehavior.cs)

### Архитектура:
Старался придерживаться классического подхода с разделением данных в ScriptableObject, логики компонентов в Monobehavior и отображения в GameObject
(MVC)

какие компоненты?
игровые элементы:
- GameUnitBase
  - MovableUnitBase
      - DroneCollector
  - StaticUnitBase
      - FactionBase
      - Resource
   
поведение MovableUnitBase реализовано следующим образом:
MovableUnitBase содержит свойство BehaviorController = GetComponent<BehaviorControllerBase>(), которая после вызова InitializeUnit приступает к работе

- BehaviorControllerBase (стейт-машина)
  - CollectingResourcesBehavior (сюда помещаются через инспектор стратегии BehaviorStrategyBase для состояний _searchingStrategy, _collectingStrategy, _returningStrategy)
- BehaviorStrategyBase (состояние стейт-машины, состояния могут общаться между собой через IBehaviorStrategyParameter, реализован только ResourceParameter для передачи информации о ресурсе)
  - SearchingResourceStrategy (каждые 0.5 сек ищет ближайший не зарезервированный ресурс и при успехе идет дальше)
  - CollectingResourceStrategy (перемещается в ресурсу и 2 секунды ждет, чтобы его собрать)
  - DeliverResourceToBaseStrategy (перемещается к базе, добавляет ресурс и снова ищет)

логика FactionBase устроена таким образом, что через инспектор подается SO TypeToPrefabData, в котором определено отображение MovableUnitType в префаб, и есть список для хранения всех MovableObjects фракции и метод для создания в указанной точке юнита (точка задается на сцене), также получает через инспектор FactionBaseData - информацию о фракции (в данном случае только кол-во ресурсов)

логика Resource: есть свойство IsReserved и метод Collect, который вызывает DestroyImmediate(gameObject)
резервирование устроено самым простым способом, но можно было бы сделать метод Reserve, который бы возвращал ResourceReservation с методом Release, чтобы только тот, кто забронировал мог освободить (вместо публичного сеттера)


![image](https://github.com/user-attachments/assets/e04b3825-4c0d-4b20-800c-2ab1361307a7)

Также одной из проблем скорости являлось не лучшие характеристики ноутбука. Довольно долго приходилось ждать компиляцию юнити (один раз даже полчаса ждал) и возникали задержки при работе в Visual Studio на самых элементарных этапах

![5346296907363906210](https://github.com/user-attachments/assets/3c6bbcb2-8233-48ee-96a6-d50ad48373e0)


