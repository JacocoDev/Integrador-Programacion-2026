## Implementado hasta el momento (Jueves 27/8)

A partir de la segunda entrega se continuó desarrollando la estructura general del juego, incorporando el flujo entre menús y partida, la configuración previa al inicio de una partida, diferentes niveles de dificultad y nuevas opciones de navegación desde la escena de juego.

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
* Los sectores se generan visualmente alrededor del jugador.
* El sistema detecta el sector al que está apuntando el jugador.
* El sector apuntado se muestra visualmente en color amarillo.
* Al disparar, el sector seleccionado cambia temporalmente a color rojo.
* Cada sector funciona como referencia para la posición de aparición de los enemigos.

### Sistema de disparo

Se mantuvo el sistema de disparo de la segunda entrega.

Actualmente:

* Se utiliza la acción `Fire` del **Unity Input System**.
* El disparo está vinculado al botón izquierdo del mouse.
* Cada disparo consume una unidad de munición.
* Existe un tiempo de espera entre disparos.
* No se puede disparar mientras la escopeta se encuentra recargando.
* No se puede disparar cuando no queda munición.
* El disparo identifica el sector al que está apuntando el jugador.
* Si existe un enemigo en ese sector, el enemigo es eliminado.
* El disparo genera feedback visual en el sector correspondiente.
* La escopeta reproduce una animación de retroceso al disparar.

### Sistema de munición y recarga

Se mantuvo y continuó utilizando el sistema de recarga desarrollado anteriormente.

* La cantidad máxima de munición se configura mediante `GameSettings`.
* La recarga utiliza la acción `Reload`.
* Las balas se cargan individualmente.
* La recarga puede mantenerse para continuar cargando munición.
* La recarga puede ser interrumpida.
* La recarga puede retroceder durante determinadas fases.
* La escopeta no puede disparar durante la recarga.
* Las diferentes etapas de la recarga poseen animaciones independientes.

### Sistema de enemigos

Se mantuvo el sistema de enemigos tipo zombie.

* Los zombies aparecen dinámicamente durante la partida.
* Cada zombie aparece dentro de un sector.
* La posición de aparición depende del sector seleccionado.
* Cada zombie queda asociado al sector en el que apareció.
* El sistema mantiene una lista de los zombies activos.
* Al disparar hacia un sector, se busca el zombie correspondiente.
* Si existe un zombie en el sector seleccionado, es eliminado.

### Sistema de movimiento y ataque de los zombies

Se mantuvo el comportamiento básico de los zombies.

* Los zombies avanzan hacia el jugador.
* El movimiento se realiza sobre el plano horizontal.
* La velocidad depende de la configuración de dificultad.
* Los zombies se detienen al alcanzar la distancia de ataque.
* Los ataques poseen un cooldown.
* Antes de producir daño, el zombie reproduce una animación de ataque.
* El zombie se inclina hacia el jugador durante el ataque.
* Después del ataque vuelve progresivamente a su rotación original.
* El ataque provoca daño al jugador.

### Sistema de vida

Se mantuvo el sistema de vida del jugador.

* La vida máxima depende de la configuración seleccionada.
* El jugador comienza con la vida máxima.
* Los zombies pueden reducir la vida del jugador.
* La vida nunca puede ser menor que cero.
* Cuando la vida llega a cero se activa el Game Over.

La cantidad de vida inicial puede variar dependiendo de la dificultad seleccionada.

### Feedback de daño

Se mantuvo el feedback visual producido al recibir daño.

* Se detecta cuando la vida del jugador disminuye.
* La cámara realiza un pequeño movimiento horizontal.
* La intensidad, duración y velocidad del efecto son configurables.
* La cámara vuelve a su posición original después del efecto.

### Sistema de oleadas

Se mantuvo el sistema de oleadas.

* La partida comienza automáticamente con la primera oleada.
* Cada oleada posee una cantidad determinada de zombies.
* La cantidad de zombies aumenta progresivamente.
* Los zombies aparecen de manera progresiva.
* El tiempo entre apariciones es aleatorio dentro de un rango configurable.
* La oleada finaliza cuando se generaron todos los zombies y no quedan zombies vivos.
* Al completar una oleada comienza un tiempo de espera.
* Durante la espera se muestra información sobre la próxima oleada.

Los tiempos de aparición de zombies pueden variar según la dificultad seleccionada.

### Sistema de Game Over

Se mantuvo el sistema centralizado de Game Over.

* `GameManager` controla si la partida continúa activa.
* Cuando el jugador pierde toda su vida se activa el Game Over.
* El tiempo del juego se detiene.
* Los sistemas que dependen del estado de la partida dejan de procesar sus acciones.
* El cursor se desbloquea.
* Se muestra el mensaje `Game Over`.
* Se muestra un botón `Main Menu`.
* El botón permite abandonar la partida y regresar al menú principal.
* Antes de cargar nuevamente la escena se restablece `Time.timeScale`.

### Sistema de interfaz

La interfaz de la partida fue actualizada y se pasó completamente al inglés.

Actualmente se muestran:

* Munición actual y máxima.
* Vida actual.
* Número de oleada actual.
* Cantidad de zombies eliminados y cantidad total de zombies.
* Mensaje `Wave Completed!`.
* Tiempo restante para la próxima oleada.
* Mensaje `Game Over`.
* Botón `Main Menu` al finalizar la partida.

Ejemplos de textos utilizados:

* `Ammo`
* `HP`
* `Wave`
* `Zombies`
* `Wave Completed!`
* `Next Wave in`
* `Game Over`
* `Main Menu`

### Sistema de menús

Se implementó el flujo principal de navegación entre las distintas escenas del juego.

Actualmente el flujo es:

`Main Menu → Game Configuration Menu → Game`

Desde la escena de juego, al finalizar la partida:

`Game Over → Main Menu`

El sistema utiliza `SceneManager` para realizar el cambio entre escenas.

### Menú principal

Se implementó el manejo de las opciones principales del menú.

Actualmente:

* `Play Game` lleva al menú de configuración de partida.
* `Open Leaderboards` se encuentra preparado para su futura implementación.
* `Open Settings` se encuentra preparado para su futura implementación.
* `Open Credits` se encuentra preparado para su futura implementación.
* `Quit Game` permite cerrar la aplicación.

### Menú de configuración de partida

Se incorporó un menú previo al inicio de la partida.

El jugador puede configurar:

#### Cantidad de direcciones

Se pueden seleccionar:

* `4 Directions`
* `6 Directions`
* `8 Directions`

La selección modifica la cantidad de sectores utilizados durante la partida.

#### Dificultad

Se pueden seleccionar:

* `Easy`
* `Hard`

La dificultad modifica diferentes parámetros del juego.

### Sistema de configuración de partida

Se creó `GameSettings` como sistema centralizado para almacenar las configuraciones seleccionadas antes de iniciar la partida.

Actualmente controla:

* Cantidad de sectores.
* Dificultad seleccionada.
* Vida del jugador.
* Munición máxima de la escopeta.
* Velocidad de los zombies.
* Tiempo mínimo entre apariciones.
* Tiempo máximo entre apariciones.

La configuración seleccionada en el menú se mantiene al cambiar a la escena `Game`.

### Dificultad

Se incorporó una primera diferenciación entre las dificultades `Easy` y `Hard`.

Actualmente:

#### Easy

* 3 HP.
* 6 unidades de munición.
* Velocidad de zombie de `1`.
* Tiempo entre apariciones de `4` a `8` segundos.

#### Hard

* 1 HP.
* 6 unidades de munición.
* Velocidad de zombie de `1.5`.
* Tiempo entre apariciones de `3` a `6` segundos.

La estructura de configuración permite agregar nuevos parámetros de dificultad posteriormente.

## Scripts desarrollados

### `CursorManager.cs`

Se encarga de:

- Bloquear el cursor al comenzar la partida.
- Ocultar el cursor durante el juego.
- Detectar el Game Over.
- Desbloquear y mostrar el cursor al finalizar la partida.

### `EnemySystem.cs`

Se encarga de:

- Generar zombies.
- Determinar aleatoriamente el sector en el que aparece cada zombie.
- Colocar los zombies dentro de los sectores.
- Mantener una lista de los zombies activos.
- Eliminar zombies al disparar al sector correspondiente.
- Obtener la cantidad de zombies vivos.

### `GameManager.cs`

Se encarga de:

- Controlar si la partida está activa.
- Activar el estado de Game Over.
- Detener el tiempo del juego al finalizar la partida.
- Proporcionar información sobre el estado actual de la partida.

### `PlayerDamageAnimation.cs`

Se encarga de:

- Detectar cuándo el jugador recibe daño.
- Generar el movimiento de la cámara.
- Controlar la duración y velocidad del efecto.
- Restaurar la posición original de la cámara.

### `PlayerHealth.cs`

Se encarga de:

- Controlar la vida actual.
- Configurar la vida máxima.
- Recibir daño.
- Evitar que la vida sea menor a 0.
- Activar el Game Over cuando la vida llega a 0.

### `PlayerLook.cs`

Controla la rotación horizontal del jugador utilizando el **Input System**.

### `SectorSystem.cs`

Se encarga de:

- Generar los sectores automáticamente.
- Determinar la orientación de cada sector.
- Detectar el sector al que apunta el jugador.
- Resaltar visualmente el sector apuntado.
- Cambiar temporalmente el color del sector al disparar.
- Proporcionar información sobre los sectores a otros sistemas.

### `Shotgun.cs`

Se encarga de:

- Controlar la munición.
- Gestionar el disparo.
- Controlar el cooldown entre disparos.
- Gestionar la recarga.
- Controlar las diferentes fases de recarga.
- Permitir la interrupción y reversión de la recarga.
- Registrar los disparos realizados.
- Registrar los intentos de acciones inválidas.
- Proporcionar información sobre el estado actual de la escopeta.

### `ShotgunAnimation.cs`

Se encarga de:

- Reproducir el retroceso al disparar.
- Reproducir las diferentes etapas visuales de la recarga.
- Restaurar la posición y rotación original.
- Reproducir feedback cuando se realiza una acción inválida.

### `UIManager.cs`

Se encarga de:

- Actualizar la UI de munición.
- Actualizar la UI de vida.
- Actualizar la UI de oleadas.
- Actualizar el mensaje de Game Over.
- Mostrar el botón `Main Menu`.
- Volver a la escena `Main Menu` al presionar el botón correspondiente.
- Restablecer `Time.timeScale` antes de abandonar la partida.

### `WaveSystem.cs`

Se encarga de:

- Crear y controlar las oleadas.
- Determinar la cantidad de zombies.
- Controlar la aparición progresiva de zombies.
- Determinar el tiempo entre apariciones.
- Detectar cuándo se completa una oleada.
- Controlar el tiempo de espera entre oleadas.

### `Zombie.cs`

Se encarga de:

- Mover los zombies hacia el jugador.
- Detectar cuándo están a distancia de ataque.
- Controlar el cooldown de ataque.
- Reproducir la animación de ataque.
- Aplicar daño al jugador.
- Restaurar la rotación original después del ataque.

### `GameConfigurationMenu.cs`

Se encarga de:

- Controlar las opciones del menú de configuración.
- Seleccionar entre 4, 6 u 8 direcciones.
- Seleccionar la dificultad `Easy` o `Hard`.
- Actualizar el estado visual de los botones seleccionados.
- Guardar la configuración seleccionada en `GameSettings`.
- Iniciar la escena `Game`.

### `GameSettings.cs`

Se encarga de:

- Almacenar la configuración seleccionada para la partida.
- Controlar la cantidad de sectores.
- Controlar la dificultad.
- Configurar la vida del jugador.
- Configurar la munición de la escopeta.
- Configurar la velocidad de los zombies.
- Configurar los tiempos de aparición de enemigos.

### `MainMenuManager.cs`

Se encarga de:

- Controlar las opciones del menú principal.
- Iniciar el menú de configuración.
- Preparar las opciones de Leaderboards, Settings y Credits.
- Cerrar la aplicación mediante `Quit Game`.