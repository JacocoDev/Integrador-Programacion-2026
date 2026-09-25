## Implementado hasta el momento (Jueves 24/9)

A partir de la cuarta entrega se continuó desarrollando el sistema principal del juego, incorporando nuevos tipos de enemigos, un sistema de generación basado en probabilidades dinámicas, una diferenciación más completa entre las dificultades, nuevos sistemas de configuración y la incorporación de los menús de Settings y Credits.

### Player FPS

Se mantuvo el sistema de jugador implementado en las entregas anteriores.

Actualmente:

* El jugador utiliza una `Capsule` como cuerpo.

* La cámara funciona en primera persona mediante:

  * `CameraPivot`
  * `Main Camera`

* La cámara permite únicamente la orientación horizontal.

* La orientación utiliza el **Unity Input System**.

* La acción `Look` utiliza el movimiento del mouse.

* La sensibilidad del movimiento puede configurarse desde el menú de Settings.

* La sensibilidad seleccionada se almacena mediante `PlayerPrefs`.

* El cursor se bloquea mientras la partida está activa.

* Al finalizar la partida, el cursor vuelve a estar visible y desbloqueado.

### Sistema de sectores

Se mantuvo el sistema de sectores desarrollado anteriormente.

Actualmente:

* Los sectores se generan automáticamente mediante `SectorSystem.cs`.

* El número de sectores puede configurarse antes de comenzar la partida.

* Se pueden seleccionar:

  * 4 direcciones.
  * 6 direcciones.
  * 8 direcciones.

* Cada sector ocupa una parte equivalente de los 360°.

* La orientación inicial del jugador funciona como referencia para los sectores.

* El sistema detecta el sector al que está apuntando el jugador.

* El sector apuntado se muestra visualmente en color amarillo.

* Al disparar, el sector seleccionado cambia temporalmente a color rojo.

* Cada sector funciona como referencia para la posición de aparición de los enemigos.

* Los enemigos quedan asociados al sector en el que fueron generados.

### Sistema de disparo

Se mantuvo el sistema de disparo desarrollado anteriormente y se continúa utilizando como principal interacción con los enemigos.

Actualmente:

* Se utiliza la acción `Fire` del **Unity Input System**.

* El disparo está vinculado al botón izquierdo del mouse.

* Cada disparo consume una unidad de munición.

* Existe un tiempo de espera entre disparos.

* No se puede disparar mientras la escopeta se encuentra recargando.

* No se puede disparar cuando no queda munición.

* El disparo identifica el sector al que está apuntando el jugador.

* Si existe un enemigo en ese sector, se busca el enemigo correspondiente mediante `EnemySystem`.

* El disparo genera feedback visual en el sector correspondiente.

* La escopeta reproduce una animación de retroceso.

* Se reproduce un sonido de disparo al realizar correctamente el disparo.

* El daño del disparo se aplica mediante el sistema de vida de los enemigos.

### Sistema de munición y recarga

Se mantuvo el sistema de recarga de la escopeta y su integración con los sistemas de animación y audio.

Actualmente:

* La cantidad máxima de munición se configura mediante `GameSettings`.

* La recarga utiliza la acción `Reload`.

* Las balas se cargan individualmente.

* La recarga puede mantenerse para continuar cargando munición.

* La recarga puede ser interrumpida.

* La recarga puede retroceder durante determinadas fases.

* La escopeta no puede disparar durante la recarga.

* Se reproduce un sonido al comenzar la secuencia de recarga.

* Se reproduce un sonido cada vez que se carga una bala.

* Se reproduce un sonido de error cuando se intenta disparar sin munición.

* Si se intenta recargar con la escopeta completamente cargada, se reproduce el sonido de error dos veces con un intervalo configurable.

* Después de un disparo se reproduce un sonido de recámara luego de un tiempo configurable.

### Sistema de enemigos

El sistema anterior basado en un único tipo de zombie fue ampliado para permitir trabajar con diferentes tipos de enemigos mediante `EnemyData`.

Actualmente:

* Los enemigos aparecen dinámicamente durante la partida.

* Cada enemigo aparece dentro de un sector.

* La posición de aparición depende del sector seleccionado.

* Cada enemigo queda asociado al sector en el que apareció.

* `EnemySystem` mantiene una lista de los enemigos existentes.

* Al disparar hacia un sector, se busca el enemigo correspondiente.

* El enemigo recibe daño mediante `Enemy.TakeDamage()`.

* Los enemigos pueden tener diferentes cantidades de vida.

* Los enemigos pueden otorgar diferentes cantidades de puntos.

* Cada tipo de enemigo posee una configuración propia mediante un `EnemyData`.

* Las características específicas de cada enemigo pueden variar según la dificultad.

* El sistema permite utilizar el mismo comportamiento general para distintos tipos de enemigos.

* La lógica de comportamiento, animación y audio se mantiene separada en componentes diferentes.

### Tipos de enemigos

Actualmente existen cuatro tipos de enemigos:

* **Normal Zombie**

  * Posee 1 punto de vida.
  * No posee comportamiento especial de aparición.
  * No posee stun al recibir daño.

* **Brute Zombie**

  * Utiliza una configuración diferente de movimiento y ataque.
  * Su aparición aumenta progresivamente durante las primeras oleadas.

* **Fast Zombie**

  * Posee una velocidad de movimiento superior.
  * Reproduce un grito antes de comenzar a desplazarse.
  * La duración de preparación puede variar según la dificultad.

* **Tank Zombie**

  * Posee 2 puntos de vida.
  * Requiere dos disparos para ser eliminado.
  * El primer disparo puede producir un estado de stun.
  * Durante el stun deja temporalmente de avanzar.

Todos los tipos utilizan el mismo sistema general de movimiento, ataque, daño, muerte, animación y audio, mientras que `EnemyData` permite configurar sus diferencias.

### Sistema de vida y daño de los enemigos

Se reemplazó el funcionamiento anterior de eliminación inmediata por un sistema de vida común para los distintos tipos de enemigos.

Actualmente:

* Cada enemigo posee una cantidad de vida configurada en `EnemyData`.

* Al recibir un disparo se reduce su vida.

* Si la vida llega a cero, comienza el proceso de muerte.

* El enemigo no es destruido inmediatamente al recibir el último disparo.

* Primero se reproduce la animación de muerte.

* Durante la muerte el enemigo deja de ejecutar su comportamiento normal.

* Una vez finalizada la animación y el tiempo configurado, `EnemySystem` destruye el GameObject.

* Los puntos se otorgan únicamente cuando el enemigo muere realmente.

* Los enemigos con más de un punto de vida pueden recibir varios disparos antes de morir.

### Sistema de movimiento y ataque de los enemigos

Se mantuvo el comportamiento general de persecución y ataque, adaptándolo al nuevo sistema genérico de enemigos.

Actualmente:

* Los enemigos avanzan hacia el jugador.

* El movimiento se realiza sobre el plano horizontal.

* La velocidad depende del `EnemyData` correspondiente.

* La velocidad puede ser diferente para cada tipo de enemigo.

* La velocidad también puede variar según la dificultad.

* Los enemigos se detienen al alcanzar la distancia de ataque.

* La distancia de ataque utilizada actualmente es de `2` unidades.

* Los ataques poseen un cooldown.

* El cooldown depende de la configuración del enemigo y de la dificultad.

* Antes de producir daño, el enemigo reproduce una animación de ataque.

* El enemigo se inclina hacia el jugador durante el ataque.

* El daño se aplica mediante la animación en el momento correspondiente.

* Después del ataque vuelve progresivamente a su estado normal.

* El comportamiento de ataque se mantiene separado de la lógica de animación.

### Sistema de estados de los enemigos

El nuevo `Enemy` utiliza estados internos para controlar su comportamiento.

Actualmente existen:

* `Preparing`

  * Utilizado por enemigos que poseen una preparación antes de comenzar a moverse.

* `Moving`

  * Estado normal de desplazamiento hacia el jugador.

* `Attacking`

  * Estado utilizado durante el ataque.

* `Stunned`

  * Estado temporal utilizado por enemigos que pueden quedar aturdidos al recibir daño.

* `Dying`

  * Estado utilizado durante la secuencia de muerte.

Estos estados permiten controlar de manera independiente el movimiento, ataque, preparación, stun y muerte.

### Sistema de animaciones de enemigos

Se mantiene la separación entre el control de las animaciones y la lógica principal de los enemigos.

El nuevo `EnemyAnimation.cs` se encarga de controlar las animaciones visuales del enemigo.

Actualmente:

* Controla la animación de ataque.

* Inclina al enemigo en dirección al jugador durante el ataque.

* Controla el tiempo de inclinación.

* Aplica el daño al jugador en el momento correspondiente.

* Devuelve progresivamente al enemigo a su rotación original.

* Controla la animación de muerte.

* El enemigo cae durante un tiempo configurable.

* La posición y rotación cambian simultáneamente durante la caída.

* La dirección de la caída se calcula tomando como referencia la posición del jugador.

* El enemigo termina sobre el suelo en una altura configurable.

* Después de caer permanece durante un tiempo configurable antes de ser destruido.

* Al finalizar el proceso informa a `Enemy` que ya puede ser eliminado.

### Sistema de audio

Se mantiene el sistema de audio modular incorporado en la entrega anterior.

El sistema se divide en diferentes scripts especializados en lugar de utilizar un único administrador de audio.

Actualmente se implementaron sonidos para:

* Jugador.

* Escopeta.

* Enemigos.

* Oleadas.

* Interfaz y otros elementos del juego mediante el sistema de mezcla de audio.

Los sonidos de los enemigos utilizan audio espacial 3D para permitir identificar su dirección y distancia mediante el sonido.

Los sonidos del jugador, escopeta y oleadas utilizan audio 2D.

Además, se incorporó un `AudioMixer` para controlar los diferentes grupos de audio.

Actualmente existen los grupos:

* `Master`
* `Player`
* `Zombies`
* `Shotgun`
* `Waves`
* `UI`

El volumen general del juego se controla mediante el parámetro expuesto `MasterVolume`.

### Audio del jugador

Se mantiene `PlayerAudio.cs` para reproducir sonidos relacionados con el estado de vida del jugador.

Actualmente:

* Detecta automáticamente cuándo disminuye la vida.

* Si el jugador recibe daño y continúa con vida, reproduce el sonido de daño.

* Si la vida llega a cero, reproduce únicamente el sonido de muerte.

* El sonido de daño no se reproduce cuando el jugador muere.

* Los sonidos utilizan el grupo de audio correspondiente del `AudioMixer`.

### Audio de la escopeta

Se mantiene `ShotgunAudio.cs` para controlar los sonidos relacionados con la escopeta.

Actualmente reproduce:

* Sonido de disparo.

* Sonido de inicio de recarga.

* Sonido de carga de cada bala.

* Sonido de error.

* Sonido de recámara después del disparo.

Los sonidos se sincronizan con los estados internos de `Shotgun.cs`, evitando depender directamente de las animaciones visuales.

### Audio de los enemigos

El anterior sistema específico de zombies fue reemplazado por `EnemyAudio.cs`, permitiendo que todos los tipos de enemigos utilicen el mismo sistema de audio.

Actualmente:

* Los pasos se reproducen mientras el enemigo se encuentra en movimiento.

* Los pasos utilizan intervalos aleatorios.

* Los gemidos se reproducen de manera independiente a los pasos.

* Los gemidos utilizan intervalos aleatorios.

* Los gemidos y pasos pueden producirse de manera independiente.

* Mientras el enemigo está atacando no se reproducen pasos ni gemidos.

* El ataque reproduce un sonido una vez al comenzar.

* La muerte reproduce un sonido específico.

* Al comenzar la muerte se detienen los sonidos normales del enemigo.

* Los enemigos que poseen un grito de aparición pueden reproducirlo antes de comenzar a moverse.

* Los enemigos que poseen un sonido de stun pueden reproducirlo al recibir daño sin morir.

* Los sonidos utilizan audio espacial 3D.

* Cada `EnemyData` puede tener sus propios clips de audio.

Esto permite utilizar el sonido como una fuente de información sobre la posición, comportamiento y tipo de enemigo dentro del entorno oscuro.

### Audio de las oleadas

Se mantiene `WaveAudio.cs` para controlar los sonidos relacionados con las transiciones de las oleadas.

Actualmente:

* Se reproduce un sonido cuando una oleada comienza.

* Se reproduce un sonido cuando una oleada termina y comienza el tiempo de espera.

* Los sonidos de las oleadas utilizan audio 2D.

* Los sonidos se reproducen una única vez por transición.

### Sistema de generación probabilística de enemigos

Se incorporó `EnemySpawnSystem.cs` para controlar qué tipo de enemigo aparece en cada generación.

En lugar de seleccionar siempre los enemigos mediante una secuencia fija, el sistema calcula una probabilidad para cada tipo de enemigo en función de la oleada actual.

Actualmente:

* Cada tipo de enemigo posee una oleada central o `peak wave`.

* Cada tipo posee un valor de `spread`.

* La probabilidad se calcula mediante una distribución gaussiana.

* Las probabilidades de todos los enemigos se normalizan para que sumen el 100%.

* En cada generación se realiza una selección aleatoria utilizando las probabilidades calculadas.

* Los enemigos no quedan bloqueados a partir de una determinada oleada.

* Un enemigo puede aparecer antes de su oleada principal, aunque su probabilidad sea muy baja.

* Las probabilidades cambian a medida que avanzan las oleadas.

### Probabilidades según dificultad

La dificultad modifica las oleadas centrales utilizadas para calcular las probabilidades.

En `Easy`:

* Normal Zombie tiene su máximo alrededor de la oleada `3`.

* Brute Zombie tiene su máximo alrededor de la oleada `6`.

* Fast Zombie tiene su máximo alrededor de la oleada `9`.

* Tank Zombie tiene su máximo alrededor de la oleada `12`.

En `Hard`:

* Normal Zombie tiene su máximo alrededor de la oleada `2`.

* Brute Zombie tiene su máximo alrededor de la oleada `4`.

* Fast Zombie tiene su máximo alrededor de la oleada `6`.

* Tank Zombie tiene su máximo alrededor de la oleada `8`.

De esta manera, en la dificultad Hard los tipos de enemigos más avanzados pasan a tener una probabilidad significativa durante oleadas más tempranas.

### Sistema de aparición de enemigos

`EnemySystem.cs` se encarga de generar físicamente los enemigos seleccionados por `EnemySpawnSystem`.

Actualmente:

* Solicita a `EnemySpawnSystem` el tipo de enemigo correspondiente a la oleada.

* Selecciona aleatoriamente un sector.

* Calcula la posición de aparición utilizando el centro angular del sector.

* Genera el enemigo en esa posición.

* Asocia el enemigo al sector correspondiente.

* Inicializa el enemigo con:

  * Jugador.
  * Vida del jugador.
  * `GameManager`.
  * `EnemyData`.

* Mantiene una lista de enemigos activos.

* Busca enemigos por sector al disparar.

* Elimina de la lista los enemigos cuya secuencia de muerte terminó.

* Destruye el GameObject del enemigo después de completar su animación de muerte.

### Sistema de oleadas

Se modificó el funcionamiento de aparición de enemigos para integrarlo con el nuevo sistema de tipos y probabilidades.

Actualmente:

* La partida comienza automáticamente con la primera oleada.

* Cada oleada posee una cantidad determinada de enemigos.

* La cantidad de enemigos aumenta progresivamente.

* El primer enemigo de cada oleada aparece inmediatamente.

* Solo puede existir un enemigo activo al mismo tiempo durante la secuencia de aparición de una oleada.

* El siguiente enemigo no aparece hasta que el anterior haya sido eliminado.

* Después de eliminar un enemigo se espera un tiempo aleatorio antes de generar el siguiente.

* El tiempo entre apariciones depende de un rango configurable.

* El rango de aparición depende de la dificultad seleccionada.

* La oleada finaliza cuando se generaron todos los enemigos y no quedan enemigos vivos.

* Al completar una oleada comienza un tiempo de espera.

* Durante la espera se muestra información sobre la próxima oleada.

* Al finalizar el tiempo de espera comienza automáticamente la siguiente oleada.

### Sistema de puntuación

Se mantiene el sistema de puntuación mediante `ScoreManager.cs` y se amplió para trabajar con los diferentes tipos de enemigos.

Actualmente:

* El jugador obtiene puntos al eliminar enemigos.

* Los puntos se registran según el tipo de enemigo.

* Se registra la cantidad de enemigos eliminados de cada tipo.

* Se registra la cantidad de puntos obtenidos por cada tipo de enemigo.

* Al completar una oleada se obtienen puntos adicionales.

* Cada oleada completada otorga `25` puntos base.

* Cada tipo de enemigo puede tener una cantidad diferente de puntos.

* La puntuación se mantiene durante toda la partida.

* La puntuación final puede modificarse mediante el multiplicador de dificultad.

* El sistema genera notificaciones de puntuación para mostrarlas en la interfaz.

### Multiplicador de puntuación

La dificultad también afecta la puntuación final.

#### Easy

* Multiplicador de puntuación de `1x`.

#### Hard

* Multiplicador de puntuación de `1.5x`.

El multiplicador se aplica sobre la puntuación base obtenida durante la partida.

### Sistema de interfaz

La interfaz fue ampliada para mostrar información relacionada con los diferentes sistemas de la partida.

Actualmente se muestran:

* Munición actual y máxima.

* Vida actual.

* Número de oleada actual.

* Cantidad de enemigos eliminados y cantidad total de enemigos.

* Mensaje `Wave Completed!`.

* Tiempo restante para la próxima oleada.

* Puntuación actual.

* Notificaciones de puntos obtenidos.

* Mensaje `Game Over`.

* Puntuación final.

* Resumen de puntuación.

* Botón para acceder a los Leaderboards.

Las notificaciones de puntuación muestran los puntos obtenidos y el tipo de enemigo eliminado.

### Sistema de Game Over

Se mantiene el sistema centralizado de Game Over y su integración con el sistema de puntuación.

Cuando finaliza la partida:

* Se detiene el tiempo del juego.

* Los sistemas que dependen del estado de la partida dejan de procesar sus acciones.

* El cursor se desbloquea.

* Se muestra el mensaje `Game Over`.

* Se muestra la puntuación final.

* Se muestra un resumen de la puntuación obtenida.

* Se habilita el acceso al sistema de Leaderboards.

Antes de acceder a los Leaderboards se almacena temporalmente la información necesaria para registrar el resultado de la partida.

### Sistema de Leaderboards

Se mantiene el sistema local de Leaderboards implementado anteriormente.

Actualmente:

* Los resultados se almacenan localmente.

* Los datos se guardan en formato JSON.

* El archivo se almacena utilizando `Application.persistentDataPath`.

* Cada resultado contiene:

  * Nombre del jugador.
  * Puntuación.
  * Dificultad.
  * Cantidad de direcciones.
  * Fecha de la partida.

* Los resultados se ordenan de mayor a menor puntuación.

* En caso de empate, se prioriza el resultado obtenido anteriormente.

* Se muestran hasta 25 resultados por categoría.

* Los Leaderboards pueden filtrarse según la cantidad de direcciones.

* Se pueden consultar los resultados correspondientes a 4, 6 u 8 direcciones.

* El sistema puede buscar resultados mediante el nombre del jugador.

* Se puede obtener la posición de un resultado dentro del ranking correspondiente.

El sistema funciona de manera local y no utiliza servidores externos ni almacenamiento en línea.

### Registro de resultados

Al finalizar una partida se puede registrar el resultado obtenido en los Leaderboards.

El sistema:

* Recupera la puntuación final.

* Recupera la dificultad utilizada.

* Recupera la cantidad de direcciones seleccionada.

* Recupera la fecha de inicio de la partida.

* Solicita un nombre al jugador.

* Permite confirmar el resultado.

* Permite omitir el registro.

* Actualiza automáticamente el Leaderboard después de registrar un resultado.

### Sistema de configuración de partida

Se mantiene `GameSettings` como sistema centralizado para almacenar la configuración seleccionada antes de iniciar una partida.

Actualmente controla:

* Cantidad de sectores.

* Dificultad seleccionada.

* Vida del jugador.

* Munición máxima de la escopeta.

* Tiempo mínimo entre apariciones.

* Tiempo máximo entre apariciones.

La configuración se mantiene al cambiar de la escena de configuración a la escena de juego.

### Dificultad

La diferenciación entre `Easy` y `Hard` fue ampliada para afectar más aspectos del juego.

Actualmente la dificultad modifica:

* Vida del jugador.

* Tiempo entre apariciones.

* Multiplicador de puntuación.

* Velocidad de movimiento de los enemigos.

* Tiempo de ataque de los enemigos.

* Comportamientos especiales de los enemigos.

* Preparación de aparición de determinados enemigos.

* Duración del stun de determinados enemigos.

* Probabilidades de aparición de los diferentes tipos de enemigos.

#### Easy

* `3` HP para el jugador.

* `6` unidades de munición.

* Tiempo entre apariciones de `3` a `5` segundos.

* Multiplicador de puntuación de `1x`.

* Utiliza las configuraciones Easy de cada `EnemyData`.

* Los picos de aparición de enemigos se encuentran más separados entre sí.

#### Hard

* `1` HP para el jugador.

* `6` unidades de munición.

* Tiempo entre apariciones de `1.5` a `3` segundos.

* Multiplicador de puntuación de `1.5x`.

* Utiliza las configuraciones Hard de cada `EnemyData`.

* Los picos de aparición de enemigos se producen durante oleadas más tempranas.

### Menú principal

Se amplió el funcionamiento del menú principal y ahora las opciones de Settings y Credits se encuentran implementadas.

Actualmente:

* `Play Game` lleva al menú de configuración de partida.

* `Open Leaderboards` permite acceder a la escena de Leaderboards.

* `Open Settings` permite acceder al menú de configuración general.

* `Open Credits` permite acceder a la escena de créditos.

* `Quit Game` permite cerrar la aplicación.

### Menú de configuración de partida

Se mantiene el menú previo al inicio de la partida.

El jugador puede configurar:

#### Cantidad de direcciones

* `4 Directions`

* `6 Directions`

* `8 Directions`

#### Dificultad

* `Easy`

* `Hard`

Los botones seleccionados actualizan visualmente su estado y modifican la configuración almacenada en `GameSettings`.

La opción `Customize Parameters` continúa sin estar implementada.

### Sistema de Settings

Se incorporó un menú de Settings para configurar parámetros generales antes de comenzar una partida.

Actualmente se pueden modificar:

#### Sensibilidad

* Se utiliza un slider con valores enteros de `0` a `10`.

* El valor `5` corresponde a la sensibilidad predeterminada.

* El valor seleccionado se guarda mediante `PlayerPrefs`.

* La sensibilidad se aplica directamente al sistema de orientación del jugador.

* La sensibilidad utilizada por `PlayerLook` se encuentra en un rango interno de `0.05` a `0.45`.

#### Volumen

* Se utiliza un slider con valores enteros de `0` a `10`.

* El valor `10` corresponde al volumen máximo.

* El valor `0` silencia completamente el audio.

* El volumen seleccionado se guarda mediante `PlayerPrefs`.

* El volumen se aplica al parámetro `MasterVolume` del `AudioMixer`.

#### Restauración de configuración

El menú incluye una opción para restaurar los valores predeterminados.

Los valores predeterminados son:

* Sensibilidad: `5`.

* Volumen: `10`.

La configuración se guarda para que permanezca disponible entre diferentes ejecuciones del juego.

### Sistema de Credits

Se incorporó una escena independiente de Credits.

Actualmente contiene:

* Nombre del director.

* Nombre de los responsables de programación.

* Créditos de efectos de sonido.

* Crédito relacionado con la construcción del controlador físico con Arduino.

* Información sobre el desarrollo del proyecto para la Feria de Ciencias 2026.

* Descripción general del concepto del juego.

La escena dispone de un botón para regresar al menú principal.

### Sistema de navegación de Leaderboards

Se mantiene la interfaz específica para consultar los resultados almacenados.

Actualmente permite:

* Mostrar los resultados de 4 direcciones.

* Mostrar los resultados de 6 direcciones.

* Mostrar los resultados de 8 direcciones.

* Cambiar entre las distintas categorías.

* Buscar jugadores mediante su nombre.

* Mostrar la cantidad total de resultados.

* Mostrar la cantidad de resultados obtenidos mediante la búsqueda.

* Utilizar scroll para recorrer los resultados.

* Regresar al menú principal.

## Scripts desarrollados

### `CursorManager.cs`

Se encarga de:

* Bloquear y ocultar el cursor durante la partida.

* Detectar el Game Over.

* Mostrar y desbloquear el cursor al finalizar la partida.

### `EnemySystem.cs`

Se encarga de:

* Generar enemigos.

* Solicitar el tipo de enemigo a `EnemySpawnSystem`.

* Determinar aleatoriamente el sector en el que aparece cada enemigo.

* Colocar los enemigos dentro de los sectores.

* Mantener una lista de los enemigos activos.

* Buscar enemigos según el sector seleccionado.

* Aplicar el daño producido por el disparo.

* Registrar los puntos cuando un enemigo muere.

* Esperar a que finalice la animación de muerte antes de destruir el enemigo.

* Obtener la cantidad de enemigos vivos.

### `Enemy.cs`

Se encarga de:

* Controlar el comportamiento general de cada enemigo.

* Mantener los estados `Preparing`, `Moving`, `Attacking`, `Stunned` y `Dying`.

* Controlar la vida actual.

* Recibir daño.

* Determinar cuándo el enemigo muere.

* Controlar el movimiento hacia el jugador.

* Detectar la distancia de ataque.

* Controlar el cooldown de ataque.

* Aplicar el daño al jugador mediante la animación.

* Controlar el estado de preparación de enemigos especiales.

* Controlar el estado de stun.

* Iniciar la secuencia de muerte.

* Informar cuándo el enemigo está listo para ser destruido.

### `EnemyAnimation.cs`

Se encarga de:

* Controlar la animación de ataque.

* Inclinar al enemigo hacia el jugador durante el ataque.

* Aplicar el daño en el momento correspondiente de la animación.

* Restaurar la rotación después del ataque.

* Controlar la animación de muerte.

* Calcular la dirección de caída respecto al jugador.

* Mover y rotar el enemigo durante la caída.

* Mantener al enemigo en el suelo durante el tiempo configurado.

* Indicar cuándo terminó completamente la secuencia de muerte.

### `EnemyAudio.cs`

Se encarga de:

* Reproducir pasos.

* Reproducir gemidos.

* Reproducir sonidos de ataque.

* Reproducir sonidos de muerte.

* Reproducir gritos de aparición.

* Reproducir sonidos de stun.

* Utilizar audio espacial 3D.

* Variar aleatoriamente los intervalos de pasos y gemidos.

* Evitar pasos y gemidos durante el ataque.

* Detener los sonidos normales cuando comienza la muerte.

### `EnemyData.cs`

Se encarga de almacenar la configuración de cada tipo de enemigo.

Contiene:

* Nombre del enemigo.

* Color del enemigo.

* Cantidad de vida.

* Puntos otorgados.

* Configuración de comportamientos especiales.

* Valores de movimiento para Easy y Hard.

* Valores de ataque para Easy y Hard.

* Duración de preparación para Easy y Hard.

* Duración del stun para Easy y Hard.

* Clips de audio correspondientes al enemigo.

* Intervalos de pasos.

* Intervalos de gemidos.

### `EnemySpawnSystem.cs`

Se encarga de:

* Determinar qué tipo de enemigo debe aparecer en una oleada.

* Calcular las probabilidades de cada tipo de enemigo.

* Utilizar una distribución gaussiana para calcular la influencia de cada enemigo.

* Normalizar las probabilidades.

* Seleccionar aleatoriamente el tipo de enemigo.

* Utilizar diferentes oleadas centrales según la dificultad.

* Aplicar los valores de `spread` configurados para cada enemigo.

### `GameManager.cs`

Se encarga de:

* Controlar si la partida está activa.

* Activar el estado de Game Over.

* Detener el tiempo del juego al finalizar la partida.

* Proporcionar información sobre el estado actual de la partida.

### `LeaderboardManager.cs`

Se encarga de:

* Crear y administrar los datos de los Leaderboards.

* Guardar los resultados en formato JSON.

* Cargar los resultados almacenados.

* Agregar nuevos resultados.

* Ordenar los resultados por puntuación.

* Resolver empates mediante la fecha de la partida.

* Filtrar resultados por cantidad de direcciones.

* Buscar resultados por nombre.

* Obtener la posición de un resultado dentro del ranking.

* Mantener un máximo de 25 resultados mostrados por categoría.

### `LeaderboardRow.cs`

Se encarga de:

* Representar visualmente un resultado individual del Leaderboard.

* Mostrar posición.

* Mostrar nombre.

* Mostrar puntuación.

* Mostrar dificultad.

* Mostrar fecha.

### `LeaderboardUIManager.cs`

Se encarga de:

* Controlar la interfaz de los Leaderboards.

* Mostrar el panel de registro de resultados.

* Mostrar la puntuación obtenida.

* Mostrar la dificultad utilizada.

* Mostrar la cantidad de direcciones.

* Registrar el nombre del jugador.

* Permitir confirmar o cancelar el registro.

* Mostrar los resultados de 4, 6 y 8 direcciones.

* Crear las filas correspondientes a cada resultado.

* Filtrar resultados mediante búsqueda.

* Actualizar los contadores de resultados.

* Controlar los scrollbars.

* Regresar al menú principal.

### `MainMenuManager.cs`

Se encarga de:

* Controlar las opciones del menú principal.

* Iniciar el menú de configuración.

* Abrir los Leaderboards.

* Abrir Settings.

* Abrir Credits.

* Cerrar la aplicación mediante `Quit Game`.

### `PlayerAudio.cs`

Se encarga de:

* Detectar cambios en la vida del jugador.

* Reproducir el sonido de daño.

* Reproducir el sonido de muerte.

* Evitar reproducir el sonido de daño cuando el jugador muere.

### `PlayerDamageAnimation.cs`

Se encarga de:

* Detectar cuándo el jugador recibe daño.

* Generar el movimiento de la cámara.

* Controlar la duración y velocidad del efecto.

* Restaurar la posición original de la cámara.

### `PlayerHealth.cs`

Se encarga de:

* Controlar la vida actual.

* Configurar la vida máxima.

* Recibir daño.

* Evitar que la vida sea menor a `0`.

* Activar el Game Over cuando la vida llega a `0`.

### `PlayerLook.cs`

Se encarga de:

* Controlar la rotación horizontal del jugador.

* Utilizar el **Unity Input System**.

* Leer la acción `Look`.

* Aplicar la sensibilidad configurada en Settings.

* Obtener la sensibilidad almacenada mediante `Settings`.

### `ScoreManager.cs`

Se encarga de:

* Controlar la puntuación de la partida.

* Registrar los enemigos eliminados.

* Registrar los puntos obtenidos por cada tipo de enemigo.

* Otorgar puntos al completar oleadas.

* Aplicar el multiplicador de dificultad.

* Generar notificaciones de puntuación.

* Proporcionar la puntuación final.

* Generar el resumen de puntuación de la partida.

* Registrar la fecha de inicio de la partida.

### `SectorSystem.cs`

Se encarga de:

* Generar los sectores automáticamente.

* Determinar la orientación de cada sector.

* Detectar el sector al que apunta el jugador.

* Resaltar visualmente el sector apuntado.

* Cambiar temporalmente el color del sector al disparar.

* Proporcionar información sobre los sectores a otros sistemas.

### `Settings.cs`

Se encarga de:

* Guardar la sensibilidad seleccionada.

* Guardar el volumen seleccionado.

* Recuperar la sensibilidad almacenada.

* Recuperar el volumen almacenado.

* Restaurar los valores predeterminados.

* Utilizar `PlayerPrefs` para conservar la configuración entre ejecuciones.

### `SettingsMenuManager.cs`

Se encarga de:

* Controlar el menú de Settings.

* Configurar el slider de sensibilidad.

* Configurar el slider de volumen.

* Cargar la configuración almacenada.

* Actualizar los textos de los sliders.

* Guardar los cambios de sensibilidad.

* Guardar los cambios de volumen.

* Aplicar el volumen al `AudioMixer`.

* Restaurar la configuración predeterminada.

* Regresar al menú principal.

### `Shotgun.cs`

Se encarga de:

* Controlar la munición.

* Gestionar el disparo.

* Controlar el cooldown entre disparos.

* Gestionar la recarga.

* Controlar las diferentes fases de recarga.

* Permitir la interrupción y reversión de la recarga.

* Registrar los disparos realizados.

* Registrar los intentos de acciones inválidas.

* Proporcionar información sobre el estado actual de la escopeta.

### `ShotgunAnimation.cs`

Se encarga de:

* Reproducir el retroceso al disparar.

* Reproducir las diferentes etapas visuales de la recarga.

* Restaurar la posición y rotación original.

* Reproducir feedback cuando se realiza una acción inválida.

### `ShotgunAudio.cs`

Se encarga de:

* Reproducir el sonido de disparo.

* Reproducir el sonido de inicio de recarga.

* Reproducir el sonido de carga de cada bala.

* Reproducir sonidos de error.

* Reproducir el sonido de recámara después de disparar.

* Controlar los tiempos de reproducción de los sonidos.

* Sincronizar los sonidos con los estados de `Shotgun`.

### `UIManager.cs`

Se encarga de:

* Actualizar la UI de munición.

* Actualizar la UI de vida.

* Actualizar la UI de oleadas.

* Actualizar la puntuación.

* Mostrar las notificaciones de puntuación.

* Mostrar el mensaje de Game Over.

* Mostrar la puntuación final.

* Mostrar el resumen de puntuación.

* Mostrar el acceso a los Leaderboards.

* Preparar los datos del resultado final antes de acceder a los Leaderboards.

### `WaveAudio.cs`

Se encarga de:

* Reproducir el sonido de inicio de oleada.

* Reproducir el sonido de finalización de oleada.

* Detectar los cambios de estado del sistema de oleadas.

### `WaveSystem.cs`

Se encarga de:

* Crear y controlar las oleadas.

* Determinar la cantidad de enemigos.

* Controlar la aparición progresiva de enemigos.

* Permitir únicamente un enemigo activo durante la secuencia de aparición.

* Controlar el tiempo entre apariciones.

* Obtener los tiempos de aparición configurados para la dificultad seleccionada.

* Detectar cuándo se completa una oleada.

* Controlar el tiempo de espera entre oleadas.

### `GameSettings.cs`

Se encarga de:

* Almacenar la cantidad de sectores seleccionada.

* Almacenar la dificultad seleccionada.

* Configurar la vida del jugador.

* Configurar la munición máxima.

* Configurar el tiempo mínimo entre apariciones.

* Configurar el tiempo máximo entre apariciones.

* Aplicar los valores correspondientes a Easy.

* Aplicar los valores correspondientes a Hard.

### `CreditsMenuManager.cs`

Se encarga de:

* Controlar la navegación de la escena de Credits.

* Regresar al menú principal.
