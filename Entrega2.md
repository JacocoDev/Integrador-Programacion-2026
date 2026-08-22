## Implementado hasta el momento (Jueves 20/8)

A partir de la primera entrega se continuó desarrollando el sistema principal del juego, incorporando enemigos, oleadas, sistema de vida, munición, recarga, animaciones, feedback visual y una interfaz de usuario básica.

### Player FPS

Se mantuvo y continuó desarrollando el sistema de jugador implementado en la primera entrega.

* El jugador utiliza una `Capsule` como cuerpo.
* La cámara funciona en primera persona mediante:
  * `CameraPivot`
  * `Main Camera`
* La cámara permite únicamente la orientación horizontal.
* El movimiento de cámara continúa utilizando el **Unity Input System**.
* La acción `Look` utiliza el movimiento del mouse.
* El cursor se bloquea mientras el juego está activo.
* Al finalizar la partida, el cursor vuelve a estar visible y desbloqueado.

### Sistema de sectores

Se mantuvo el sistema de sectores desarrollado en la primera entrega y se amplió su funcionamiento para integrarlo con los enemigos y el disparo.

Actualmente:

* Los sectores se generan automáticamente mediante `SectorSystem.cs`.
* El número de sectores es configurable.
* Cada sector ocupa una parte equivalente de los 360°.
* Los sectores se generan visualmente como porciones de una pizza alrededor del jugador.
* Cada sector posee un color base alternado.
* El sistema detecta el sector al que está apuntando el jugador según su rotación horizontal.
* La orientación inicial del jugador se utiliza como referencia para determinar los sectores.
* La posición de la cámara y de la escopeta no afecta la detección del sector.
* El sector apuntado se muestra visualmente en color amarillo.
* Al disparar, el sector seleccionado cambia temporalmente a color rojo.
* Cada sector funciona también como referencia para la posición de aparición de los enemigos.

### Sistema de disparo

El sistema de disparo desarrollado en la primera entrega fue ampliado para incorporar munición, enfriamiento y eliminación de enemigos.

* Se utiliza la acción `Fire` del **Unity Input System**.
* El disparo está vinculado al botón izquierdo del mouse.
* Cada disparo consume una unidad de munición.
* La cantidad máxima de munición es configurable.
* Existe un tiempo de espera entre disparos.
* No se puede disparar mientras la escopeta se encuentra recargando.
* No se puede disparar cuando no queda munición.
* Si se intenta disparar sin munición, se registra un error y se reproduce un feedback visual en la escopeta.
* El disparo identifica el sector al que está apuntando el jugador.
* Si existe un enemigo en ese sector, el enemigo es eliminado.
* El disparo genera feedback visual en el sector correspondiente.
* Se incorporó una animación de retroceso de la escopeta al disparar.

### Sistema de munición y recarga

Se implementó un sistema completo de recarga para la escopeta.

* La cantidad de munición actual y máxima es configurable.
* La recarga utiliza la acción `Reload` del **Unity Input System**.
* La recarga se divide en distintas fases:
  * `Lowering`
  * `RotatingDown`
  * `Loading`
  * `RotatingUp`
  * `Raising`
* La escopeta baja y rota durante la recarga.
* Las balas se cargan individualmente.
* La recarga puede mantenerse para continuar cargando munición.
* La recarga puede ser interrumpida antes de finalizar.
* Si se solicita nuevamente la recarga durante determinadas fases, la animación puede retroceder para volver al estado de carga.
* La escopeta no puede disparar mientras se encuentra en una fase de recarga.
* La recarga posee distintas duraciones configurables para cada etapa.

### Sistema de enemigos

Se implementó un sistema básico de enemigos tipo zombie.

* Los enemigos se crean dinámicamente durante la partida.
* Los zombies aparecen a una distancia configurable del jugador.
* Cada zombie aparece dentro de uno de los sectores disponibles.
* La posición de aparición se determina utilizando el centro angular del sector seleccionado.
* Cada zombie queda asociado al sector en el que apareció.
* El sistema mantiene una lista de los zombies existentes.
* Al disparar hacia un sector, se busca el zombie correspondiente a ese sector.
* Si existe un zombie en el sector seleccionado, es eliminado.
* El sistema permite conocer la cantidad de zombies que continúan vivos.

### Sistema de movimiento y ataque de los zombies

Los zombies poseen comportamiento básico de persecución y ataque.

* Los zombies avanzan hacia la posición del jugador.
* El movimiento se realiza únicamente sobre el plano horizontal.
* La velocidad de movimiento es configurable.
* Los zombies se detienen cuando alcanzan una distancia determinada del jugador.
* Al encontrarse dentro de la distancia de ataque, intentan atacar.
* Los ataques poseen un tiempo de espera configurable.
* Antes de realizar el daño, el zombie reproduce una animación de ataque.
* El zombie se inclina hacia el jugador durante el ataque.
* Una vez realizado el ataque, vuelve progresivamente a su rotación original.
* El ataque provoca daño al jugador.

### Sistema de vida

Se implementó un sistema de vida para el jugador.

* La vida máxima es configurable.
* El jugador comienza la partida con la vida máxima.
* Los zombies pueden reducir la vida del jugador al atacar.
* La vida nunca puede ser menor que cero.
* Cuando la vida llega a cero, la partida termina.
* El sistema de vida se comunica con `GameManager` para producir el Game Over.

### Feedback de daño

Se agregó feedback visual al recibir daño.

* Se detecta automáticamente cuando la vida del jugador disminuye.
* Al recibir daño, la cámara realiza un pequeño movimiento horizontal.
* La intensidad, duración y velocidad del movimiento son configurables.
* Una vez finalizado el efecto, la cámara vuelve a su posición original.

### Sistema de oleadas

Se implementó un sistema de oleadas para controlar la aparición progresiva de enemigos.

* La partida comienza automáticamente con la primera oleada.
* Cada oleada posee una cantidad determinada de zombies.
* La cantidad de zombies aumenta progresivamente con cada nueva oleada.
* Los zombies de una misma oleada aparecen de manera progresiva y no todos al mismo tiempo.
* El tiempo entre apariciones es aleatorio dentro de un rango configurable.
* La oleada finaliza cuando:
  * Se generaron todos los zombies correspondientes.
  * No quedan zombies vivos.
* Al completar una oleada comienza un tiempo de espera antes de iniciar la siguiente.
* El tiempo de espera entre oleadas es configurable.
* Durante la espera se muestra información sobre la próxima oleada.

### Sistema de Game Over

Se implementó un sistema centralizado para controlar el estado de la partida.

* Mientras el juego está activo, los sistemas principales pueden continuar funcionando.
* Cuando el jugador pierde toda su vida, se activa el Game Over.
* Al producirse el Game Over, el tiempo del juego se detiene.
* Los sistemas que dependen del estado de la partida dejan de procesar sus acciones.
* El cursor se desbloquea al finalizar la partida.
* Se muestra un mensaje de Game Over en la interfaz.

### Sistema de interfaz

Se implementó una interfaz básica para mostrar información importante durante la partida.

Actualmente se muestran:

* Munición actual y máxima de la escopeta.
* Vida actual del jugador.
* Número de oleada actual.
* Cantidad de zombies eliminados y cantidad total de zombies de la oleada.
* Mensaje de oleada completada.
* Tiempo restante para la próxima oleada.
* Mensaje de Game Over.

La interfaz se actualiza durante la partida para reflejar el estado actual de los distintos sistemas.

### Feedback visual de la escopeta

Se incorporaron diferentes animaciones para comunicar las acciones de la escopeta.

* Retroceso al disparar.
* Movimiento de la escopeta durante la recarga.
* Rotación durante las distintas fases de recarga.
* Movimiento de la escopeta cuando el jugador intenta realizar una acción no válida, como disparar sin munición o intentar recargar cuando no corresponde.
* Las animaciones utilizan diferentes duraciones y ángulos configurables.

## Scripts desarrollados

### `CursorManager.cs`

Controla el estado del cursor dependiendo de si la partida está activa o terminó.

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

### `PlayerDamageAnimation.cs`

Se encarga de:

- Detectar cuándo el jugador recibe daño.
- Generar el movimiento de la cámara al recibir daño.
- Controlar la duración y velocidad del efecto.
- Restaurar la posición original de la cámara.

### `PlayerHealth.cs`

Se encarga de:

- Controlar la vida actual del jugador.
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
- Resaltar visualmente el sector al que apunta el jugador.
- Cambiar temporalmente el color del sector al disparar.
- Proporcionar información sobre los sectores a otros sistemas.

### `Shotgun.cs`

Se encarga de:

- Controlar la munición de la escopeta.
- Gestionar el disparo.
- Controlar el cooldown entre disparos.
- Gestionar el sistema de recarga.
- Controlar las diferentes fases de la recarga.
- Permitir la interrupción y reversión de la recarga.
- Registrar los disparos realizados.
- Registrar los intentos de acciones inválidas.
- Proporcionar información sobre el estado actual de la escopeta.

### `ShotgunAnimation.cs`

Se encarga de:

- Reproducir el retroceso de la escopeta al disparar.
- Reproducir las diferentes etapas visuales de la recarga.
- Restaurar la posición y rotación original de la escopeta.
- Reproducir un movimiento lateral cuando se intenta realizar una acción inválida.

### `UIManager.cs`

Se encarga de:

- Actualizar la UI de munición.
- Actualizar la UI de vida.
- Actualizar la UI de oleadas.
- Actualizar la UI de Game Over.

### `WaveSystem.cs`

Se encarga de:

- Crear y controlar las oleadas.
- Determinar la cantidad de zombies de cada oleada.
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
- Aplicar daño al jugador al finalizar el ataque.
- Restaurar la rotación original después del ataque.