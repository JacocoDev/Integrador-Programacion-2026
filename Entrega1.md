## Implementado hasta el momento (Jueves 13/8)

### Player FPS

* Creación del jugador utilizando una **Capsule** como cuerpo.
* Configuración de una cámara en primera persona mediante:

  * `CameraPivot`
  * `Main Camera`
* La cámara permite únicamente la orientación horizontal.
* Implementación del movimiento de cámara utilizando el **Unity Input System**.
* Configuración de una acción `Look` utilizando el movimiento del mouse.

### Sistema de sectores

Se implementó el sistema principal de división de direcciones del juego.

Actualmente se utilizan **4 direcciones/sectores**, representados visualmente como porciones de una pizza alrededor del jugador.

* Los sectores se generan automáticamente mediante `SectorSystem.cs`.
* Cada sector ocupa 90°.
* Los sectores se denominan:

  * `Sector 0`
  * `Sector 1`
  * `Sector 2`
  * `Sector 3`
* Los colores de los sectores se alternan entre dos tonos de gris para diferenciarlos visualmente.
* El jugador comienza mirando hacia el centro del `Sector 0`.
* El sistema detecta correctamente en qué sector está apuntando el jugador según su rotación horizontal.
* La posición de la cámara y de la escopeta no afecta la detección del sector.

### Sistema de disparo básico

Se implementó una primera versión del disparo utilizando el **Input System**.

* Se creó la acción `Fire`.
* El disparo está vinculado actualmente al botón izquierdo del mouse.
* Al disparar, se identifica el sector al que está apuntando el jugador.
* El sector seleccionado cambia temporalmente a color rojo como feedback visual.
* El sector vuelve posteriormente a su color correspondiente.

### Estructura actual

```text
Scene
│
├── Player
│   ├── CameraPivot
│   │   └── Main Camera
│   ├── Shotgun
│   ├── Player Input
│   └── Player Look
│
└── Arena
    └── SectorSystem
```

## Scripts desarrollados

### `PlayerLook.cs`

Controla la rotación horizontal del jugador utilizando el Input System.

### `SectorSystem.cs`

Se encarga de:

* Generar los sectores.
* Determinar la orientación de cada sector.
* Detectar el sector al que apunta el jugador.
* Cambiar los colores de los sectores.
* Gestionar el feedback visual del disparo.