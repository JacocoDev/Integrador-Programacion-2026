## Implementado hasta el momento (Jueves 10/9)

A partir de la tercera entrega se continuó desarrollando el sistema principal del juego, incorporando el sistema de audio espacial, una separación más clara entre la lógica y las animaciones de los enemigos, un nuevo sistema de puntuación, Leaderboards y nuevas conexiones entre los distintos sistemas del juego.

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

### Sistema de disparo

Se mantuvo el sistema de disparo desarrollado anteriormente y se incorporó su integración con el sistema de audio.

Actualmente:

* Se utiliza la acción `Fire` del **Unity Input System**.
* El disparo está vinculado al botón izquierdo del mouse.
* Cada disparo consume una unidad de munición.
* Existe un tiempo de espera entre disparos.
* No se puede disparar mientras la escopeta se encuentra recargando.
* No se puede disparar cuando no queda munición.
* El disparo identifica el sector al que está apuntando el jugador.
* Si existe un enemigo en ese sector, se inicia su proceso de muerte.
* El disparo genera feedback visual en el sector correspondiente.
* La escopeta reproduce una animación de retroceso.
* Se incorporó un sonido de disparo que se reproduce al realizar correctamente el disparo.

### Sistema de munición y recarga

Se mantuvo el sistema de recarga de la escopeta y se incorporaron sonidos específicos para sus distintas acciones.

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

Se mantuvo el sistema de enemigos tipo zombie y se modificó su ciclo de eliminación.

Actualmente:

* Los zombies aparecen dinámicamente durante la partida.
* Cada zombie aparece dentro de un sector.
* La posición de aparición depende del sector seleccionado.
* Cada zombie queda asociado al sector en el que apareció.
* `EnemySystem` mantiene una lista de los zombies existentes.
* Al disparar hacia un sector, se busca el zombie correspondiente.
* Si existe un zombie en el sector seleccionado, se inicia su animación de muerte.
* El zombie no es destruido inmediatamente al recibir el disparo.
* El zombie permanece en la escena mientras se reproduce su animación de muerte.
* Una vez finalizada la animación y el tiempo configurado sobre el suelo, `EnemySystem` elimina el zombie de su lista y destruye su GameObject.

### Sistema de movimiento y ataque de los zombies

Se mantuvo el comportamiento básico de persecución y ataque.

* Los zombies avanzan hacia el jugador.
* El movimiento se realiza sobre el plano horizontal.
* La velocidad depende de la configuración de dificultad.
* Los zombies se detienen al alcanzar la distancia de ataque.
* Los ataques poseen un cooldown.
* Antes de producir daño, el zombie reproduce una animación de ataque.
* El zombie se inclina hacia el jugador durante el ataque.
* El daño se aplica al finalizar la fase inicial de la animación.
* Después del ataque vuelve progresivamente a su rotación original.
* El comportamiento de ataque se mantiene separado de la lógica de animación.

### Sistema de animaciones de zombies

Se separó el control de las animaciones de los zombies de su lógica de comportamiento.

El nuevo `ZombieAnimation.cs` se encarga de las animaciones sin formar parte de la lógica principal de `Zombie.cs`.

Actualmente:

* Controla la animación de ataque.
* Inclina al zombie en dirección al jugador durante el ataque.
* Controla el tiempo de inclinación.
* Aplica el daño mediante `Zombie` al finalizar la fase correspondiente.
* Devuelve progresivamente al zombie a su rotación original.
* Controla la animación de muerte.
* El zombie cae durante un tiempo configurable.
* La posición y rotación cambian simultáneamente durante la caída.
* La dirección de la caída se calcula tomando como referencia la posición del jugador.
* El zombie termina sobre el suelo en una altura configurable.
* Después de caer permanece durante un tiempo configurable antes de ser destruido.
* Al finalizar el proceso informa a `Zombie` que ya puede ser eliminado.

### Sistema de audio

Se incorporó un sistema de audio modular para acompañar las acciones principales del juego.

El sistema se divide en diferentes scripts especializados en lugar de utilizar un único administrador de audio.

Actualmente se implementaron sonidos para:

* Jugador.
* Escopeta.
* Zombies.
* Oleadas.

Los sonidos de los zombies utilizan audio espacial 3D para permitir identificar su dirección y distancia mediante el sonido.

Los sonidos del jugador, escopeta y oleadas utilizan audio 2D.

### Audio del jugador

Se incorporó `PlayerAudio.cs` para reproducir sonidos relacionados con el estado de vida del jugador.

Actualmente:

* Detecta automáticamente cuándo disminuye la vida.
* Si el jugador recibe daño y continúa con vida, reproduce el sonido de daño.
* Si la vida llega a cero, reproduce únicamente el sonido de muerte.
* El sonido de daño no se reproduce cuando el jugador muere.

### Audio de la escopeta

Se incorporó `ShotgunAudio.cs` para controlar los sonidos relacionados con la escopeta.

Actualmente reproduce:

* Sonido de disparo.
* Sonido de inicio de recarga.
* Sonido de carga de cada bala.
* Sonido de error.
* Sonido de recámara después del disparo.

Los sonidos se sincronizan con los estados internos de `Shotgun.cs`, evitando depender directamente de las animaciones visuales.

### Audio de los zombies

Se incorporó `ZombieAudio.cs` para controlar de manera independiente los sonidos de cada zombie.

Actualmente:

* Los pasos se reproducen mientras el zombie se encuentra en movimiento.
* Los pasos utilizan intervalos aleatorios.
* Los gemidos se reproducen de manera independiente a los pasos.
* Los gemidos utilizan intervalos aleatorios.
* Los gemidos y pasos pueden producirse de manera independiente.
* Mientras el zombie está atacando no se reproducen pasos ni gemidos.
* El ataque reproduce un sonido una vez al comenzar.
* La muerte reproduce un sonido específico.
* Al comenzar la muerte se detienen los sonidos normales del zombie.
* Los sonidos utilizan audio espacial 3D.

Esto permite utilizar el sonido como una fuente de información sobre la posición de los enemigos dentro del entorno oscuro.

### Audio de las oleadas

Se incorporó `WaveAudio.cs` para controlar los sonidos relacionados con el comienzo y final de las oleadas.

Actualmente:

* Se reproduce un sonido cuando una oleada comienza.
* Se reproduce un sonido cuando una oleada termina y comienza el tiempo de espera.
* Los sonidos de las oleadas utilizan audio 2D.
* Los sonidos se reproducen una única vez por transición.

### Sistema de oleadas

Se modificó el funcionamiento de aparición de zombies para controlar de manera más precisa el ritmo de cada oleada.

Actualmente:

* La partida comienza automáticamente con la primera oleada.
* Cada oleada posee una cantidad determinada de zombies.
* La cantidad de zombies aumenta progresivamente.
* El primer zombie de cada oleada aparece inmediatamente.
* Solo puede existir un zombie activo al mismo tiempo durante la secuencia de aparición de una oleada.
* El siguiente zombie no aparece hasta que el anterior haya sido eliminado.
* Después de eliminar un zombie se espera un tiempo aleatorio antes de generar el siguiente.
* El tiempo entre apariciones depende de un rango configurable.
* La oleada finaliza cuando se generaron todos los zombies y no quedan zombies vivos.
* Al completar una oleada comienza un tiempo de espera.
* Durante la espera se muestra información sobre la próxima oleada.
* Al finalizar el tiempo de espera comienza automáticamente la siguiente oleada.

La lógica anterior de aparición aleatoria se conserva comentada dentro de `WaveSystem.cs` como referencia y no se elimina.

### Sistema de puntuación

Se incorporó un sistema de puntuación mediante `ScoreManager.cs`.

Actualmente:

* El jugador obtiene puntos al eliminar zombies.
* Los puntos se registran según el tipo de zombie.
* Se registra la cantidad de zombies eliminados de cada tipo.
* Se registra la cantidad de puntos obtenidos por cada tipo de zombie.
* Al completar una oleada se obtienen puntos adicionales.
* Cada oleada completada otorga `25` puntos base.
* Un zombie normal otorga `1` punto base.
* La puntuación se mantiene durante toda la partida.
* La puntuación final puede modificarse mediante el multiplicador de dificultad.
* El sistema genera notificaciones de puntuación para mostrarlas en la interfaz.

### Multiplicador de puntuación

La dificultad también afecta la puntuación final.

Actualmente:

#### Easy

* Multiplicador de puntuación de `1x`.

#### Hard

* Multiplicador de puntuación de `1.5x`.

El multiplicador se aplica sobre la puntuación base obtenida durante la partida.

### Sistema de interfaz

La interfaz fue ampliada para mostrar información relacionada con la puntuación y los resultados de la partida.

Actualmente se muestran:

* Munición actual y máxima.
* Vida actual.
* Número de oleada actual.
* Cantidad de zombies eliminados y cantidad total de zombies.
* Mensaje `Wave Completed!`.
* Tiempo restante para la próxima oleada.
* Puntuación actual.
* Notificaciones de puntos obtenidos.
* Mensaje `Game Over`.
* Puntuación final.
* Resumen de puntuación.
* Botón para acceder a los Leaderboards.

Las notificaciones de puntuación muestran los puntos obtenidos y el tipo de acción correspondiente.

### Sistema de Game Over

Se mantuvo el sistema centralizado de Game Over y se amplió para integrar el sistema de puntuación.

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

Se implementó un sistema local de Leaderboards para almacenar y consultar los resultados obtenidos por los jugadores.

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

Se mantuvo `GameSettings` como sistema centralizado para almacenar la configuración seleccionada antes de iniciar una partida.

Actualmente controla:

* Cantidad de sectores.
* Dificultad seleccionada.
* Vida del jugador.
* Munición máxima de la escopeta.
* Velocidad de los zombies.
* Tiempo mínimo entre apariciones.
* Tiempo máximo entre apariciones.

La configuración se mantiene al cambiar de la escena de configuración a la escena de juego.

### Dificultad

Se mantuvo la diferenciación entre las dificultades `Easy` y `Hard`.

Actualmente:

#### Easy

* 3 HP.
* 6 unidades de munición.
* Velocidad de zombie de `0.75`.
* Tiempo entre apariciones de `3` a `5` segundos.
* Multiplicador de puntuación de `1x`.

#### Hard

* 1 HP.
* 6 unidades de munición.
* Velocidad de zombie de `1`.
* Tiempo entre apariciones de `1.5` a `3` segundos.
* Multiplicador de puntuación de `1.5x`.

La dificultad afecta tanto al comportamiento de los enemigos como al resultado final de la partida.

### Menú principal

Se amplió el funcionamiento del menú principal.

Actualmente:

* `Play Game` lleva al menú de configuración de partida.
* `Open Leaderboards` permite acceder a la escena de Leaderboards.
* `Open Settings` permanece preparado para una futura implementación.
* `Open Credits` permanece preparado para una futura implementación.
* `Quit Game` permite cerrar la aplicación.

### Menú de configuración de partida

Se mantuvo el menú previo al inicio de la partida.

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

### Sistema de navegación de Leaderboards

Se incorporó una interfaz específica para consultar los resultados almacenados.

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

* Generar zombies.
* Determinar aleatoriamente el sector en el que aparece cada zombie.
* Colocar los zombies dentro de los sectores.
* Mantener una lista de los zombies activos.
* Iniciar el proceso de muerte de un zombie al disparar al sector correspondiente.
* Esperar a que finalice la animación de muerte antes de destruir el zombie.
* Obtener la cantidad de zombies vivos.

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
* Mantener preparadas las opciones de Settings y Credits.
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
* Evitar que la vida sea menor a 0.
* Activar el Game Over cuando la vida llega a 0.

### `PlayerLook.cs`

Controla la rotación horizontal del jugador utilizando el **Input System**.

### `ScoreManager.cs`

Se encarga de:

* Controlar la puntuación de la partida.
* Registrar los zombies eliminados.
* Registrar los puntos obtenidos por cada tipo de zombie.
* Otorgar puntos al completar oleadas.
* Aplicar el multiplicador de dificultad.
* Generar notificaciones de puntuación.
* Proporcionar la puntuación final.
* Generar el resumen de puntuación de la partida.

### `SectorSystem.cs`

Se encarga de:

* Generar los sectores automáticamente.
* Determinar la orientación de cada sector.
* Detectar el sector al que apunta el jugador.
* Resaltar visualmente el sector apuntado.
* Cambiar temporalmente el color del sector al disparar.
* Proporcionar información sobre los sectores a otros sistemas.

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
* Determinar la cantidad de zombies.
* Controlar la aparición progresiva de zombies.
* Permitir únicamente un zombie activo durante la secuencia de aparición.
* Controlar el tiempo entre apariciones.
* Detectar cuándo se completa una oleada.
* Controlar el tiempo de espera entre oleadas.
* Mantener la lógica anterior de aparición comentada como referencia.

### `Zombie.cs`

Se encarga de:

* Mover los zombies hacia el jugador.
* Detectar cuándo están a distancia de ataque.
* Controlar el cooldown de ataque.
* Controlar el estado de ataque.
* Aplicar daño al jugador.
* Controlar el estado de muerte.
* Indicar cuándo el zombie está listo para ser destruido.
* Proporcionar la referencia del jugador a otros componentes del zombie.

### `ZombieAnimation.cs`

Se encarga de:

* Controlar la animación de ataque.
* Inclinar al zombie hacia el jugador durante el ataque.
* Aplicar el daño en el momento correspondiente de la animación.
* Restaurar la rotación después del ataque.
* Controlar la animación de muerte.
* Calcular la dirección de caída respecto al jugador.
* Mover y rotar el zombie durante la caída.
* Mantener al zombie en el suelo durante el tiempo configurado.
* Indicar cuándo terminó completamente la secuencia de muerte.

### `ZombieAudio.cs`

Se encarga de:

* Reproducir pasos.
* Reproducir gemidos.
* Reproducir sonidos de ataque.
* Reproducir sonidos de muerte.
* Utilizar audio espacial 3D.
* Variar aleatoriamente los intervalos de pasos y gemidos.
* Evitar pasos y gemidos durante el ataque.
* Detener los sonidos normales cuando comienza la muerte.
