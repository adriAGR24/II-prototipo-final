# 🎲 VR Blackjack: Experiencia Inmersiva con Control por Voz

> **Proyecto de Realidad Virtual para Oculus Quest 2 desarrollado en Unity con XR Interaction Toolkit.**

Este proyecto implementa una simulación de Blackjack inmersiva donde la interacción natural es la protagonista. Se han sustituido los controles tradicionales por **comandos de voz (Reconocimiento del Habla)** y **gestos físicos**, creando una interfaz multimodal que simula la experiencia real de sentarse frente a un crupier en un casino.

![Demo del Proyecto](ruta/a/tu_gif_animado.gif)
*(Sustituye esta línea por la ruta de tu GIF en la carpeta del proyecto)*

---

## ⚠️ Cuestiones Importantes para el Uso

Para disfrutar de la experiencia tal y como fue diseñada, por favor ten en cuenta lo siguiente antes de ejecutar la aplicación:

1.  **Permisos de Micrófono:** Al iniciar la aplicación en las gafas, es **imperativo aceptar los permisos de grabación de audio**. El núcleo del juego utiliza el modelo **OpenAI Whisper** localmente; si no se concede permiso, el juego no podrá recibir comandos.
2.  **Experiencia de Sentado:** El juego está diseñado como una *Seated Experience*.
    * Al inicio, verás una silla virtual.
    * Apunta a ella y pulsa el botón *Grip* (o el botón asignado en `SillaInteractuable.cs`) para "sentarte". Esto alineará tu cámara y altura con la mesa de juego.
3.  **Comandos de Voz:** Habla con claridad y naturalidad. Los comandos principales son:
    * **Pedir:** *"Dame carta", "Pedir", "Otra", "Hit".*
    * **Plantarse:** *"Me planto", "Basta", "Me quedo", "Listo".*
    * **Reiniciar:** *"Jugar", "Empezar partida", "Nuevo juego".*
4.  **Entorno:** Se recomienda configurar el guardián en modo **Estacionario**.

---

## 🚀 Hitos de Programación y Contenidos Impartidos

El desarrollo ha integrado competencias avanzadas de la asignatura de **Interfaces Inteligentes** y programación en Unity:

### 1. Programación Orientada a Eventos (Observer Pattern)
Hemos desacoplado totalmente la lógica del juego de la interfaz y los efectos audiovisuales.
* **Logro:** El script `EventManager.cs` funciona como una centralita. Cuando ocurre algo (ej. `OnPlayerHit`), múltiples sistemas reaccionan sin conocerse entre sí: el `DeckManager` suelta una carta, el `AudioManager` reproduce un sonido y el `CroupierVisuals` inicia la animación.

### 2. Reconocimiento del Habla (Speech-to-Intent)
Integración de Inteligencia Artificial para procesamiento de lenguaje natural.
* **Logro:** Implementación de **Whisper AI** (`VoiceControlBlackjack.cs`). No solo transcribe audio a texto, sino que usamos **Expresiones Regulares (Regex)** para detectar la *intención* del usuario, permitiendo que diga "dame otra" o "quiero pedir" y el sistema entienda ambas como el mismo comando.

### 3. Matemáticas y Físicas Aplicadas
Uso de vectores y curvas para movimiento procedural.
* **Logro:** En lugar de animaciones pregrabadas para las cartas, utilizamos **Curvas de Bézier** en tiempo real (`CroupierVisuals.cs`). Calculamos una parábola matemática (`Lerp` con altura de arco) para que la carta vuele físicamente desde la mano del crupier hasta su posición exacta en la mesa.

### 4. Uso de Sensores Físicos (Multimodalidad)
Lectura directa de los sensores del hardware para mecánicas ocultas.
* **Logro:** Acceso a la velocidad lineal de los mandos mediante `InputDevice` y `CommonUsages.deviceVelocity` en el script `HandSeepdDetection.cs`.

---

## ✨ Aspectos Destacados de la Aplicación

* **🖐️ Inmersión "Manos Libres":** Se han eliminado los modelos 3D de los mandos de Oculus. En su lugar, utilizamos **modelos de manos completas** que siguen la posición de los controladores, aumentando la sensación de presencia (Embodiment).
* **🎭 NPC Reactivo (Crupier con "Vida"):** El crupier no es estático. Gracias al script `HandSeepdDetection.cs`, si el jugador agita las manos violentamente o intenta golpear la mesa, el crupier reacciona verbalmente pidiendo calma, dotando de personalidad a la IA.
* **🎨 UI Diegética y Minimalista:** Hemos evitado menús flotantes intrusivos. La información (puntuación, mensajes) está integrada en el espacio 3D de la mesa (`GameManager.cs`), y las transiciones se manejan con fundidos suaves (`ScreenFader.cs`) para evitar mareos.
* **🔊 Audio Espacializado:** Los sonidos no son estéreo simple. El sonido de barajar (`ShuffleSound.cs`) o repartir (`CardDealSound.cs`) proviene espacialmente de la ubicación exacta de la baraja o la mano del crupier.

---

## 📡 Sensores Incluidos (Interfaces Multimodales)

| Sensor / Input | Hardware | Implementación en el Proyecto |
| :--- | :--- | :--- |
| **Acelerómetro / Giróscopo** | IMU en Touch Controllers | **Detección de Gestos Bruscos:** Se monitorea la magnitud del vector de velocidad de los mandos. Si supera `1.5 m/s` (`velocidadUmbral`), se dispara el evento `OnPlayerMoveHandsFast`, activando una queja del crupier. |
| **Micrófono** | Array de micrófonos HMD | **Control por Voz:** Captura de audio en tiempo real, gestión de buffers y detección de silencio (`VAD`) para enviar los paquetes de voz a la IA de reconocimiento. |
| **Posicionamiento 6DOF** | Cámaras de Tracking Quest 2 | **Tracking de Cabeza y Manos:** Usado para la interacción física con la silla (`SillaInteractuable.cs`) y para calcular la posición de lanzamiento de las cartas hacia el jugador. |

---

## 👥 Acta de Acuerdos del Grupo

**Metodología:** Trabajo modular con integración continua mediante Unity Version Control.

### Reparto de Tareas Individuales

* **Adrián García Rodríguez**
    * *Responsabilidad:* Mecánicas de interacción física y configuración del entorno XR.
    * *Desarrollo:* Script de `Sentarse_silla.cs`, lógica de `SillaInteractuable.cs` y ajuste de colisionadores y físicas de la mesa.
* **Roberto Padrón Castañeda**
    * *Responsabilidad:* Arte técnico, Animación y Avatar.
    * *Desarrollo:* Integración del modelo 3D del Croupier, gestión de `CroupierVisuals.cs` (movimiento de cartas), y sustitución de prefabs de controladores por manos.
* **Cristóbal Jesús Sarmiento Rodríguez**
    * *Responsabilidad:* Diseño Sonoro y Sensores.
    * *Desarrollo:* Arquitectura de audio (`AudioManager.cs`, `CardDealSound.cs`, `ShuffleSound.cs`) y programación de la detección de movimiento rápido (`HandSeepdDetection.cs`).
* **Kyliam Gabriel Chinea Salcedo**
    * *Responsabilidad:* Interfaz de Usuario (UI) y Feedback visual.
    * *Desarrollo:* Diseño de Canvas en espacio mundial, sistema de puntuación en `GameManager.cs` y efectos de transición (`ScreenFader.cs`).

### Tareas Desarrolladas en Grupo
* **Arquitectura del Core:** Diseño conjunto de la lógica del Blackjack (`GameManager.cs` y `DeckManager.cs`).
* **Integración de Whisper:** La configuración del reconocimiento de voz (`VoiceControlBlackjack.cs`) y el ajuste de parámetros para evitar alucinaciones de la IA se realizó en equipo.
* **Gestión de Eventos:** Definición de la estructura de `EventManager.cs` para asegurar que el trabajo de todos los miembros se conectara correctamente.

---

## ✅ Check-list de Recomendaciones de Diseño RV

| Recomendación de Diseño | Estado | Observaciones |
| :--- | :---: | :--- |
| **Mantener tasa de frames estable (72+ FPS)** | ✅ Se contempla | Escena optimizada con iluminación baked y polígonos reducidos. |
| **Evitar Cinetosis (Motion Sickness)** | ✅ Se contempla | Diseño de "Experiencia Sentada" sin locomoción artificial (teletransporte/joystick) para evitar disonancia vestibular. Uso de `ScreenFader`. |
| **UI en Espacio Mundial (World Space)** | ✅ Se contempla | Los textos son parte de la mesa, no están pegados a la cámara ("Screen Space"). |
| **Feedback Auditivo Espacial** | ✅ Se contempla | Sonidos ubicados en coordenadas 3D correctas. |
| **Feedback Visual (Affordance)** | ✅ Se contempla | La silla cambia de material (`materialResaltado`) cuando el jugador la apunta, indicando que es interactuable. |
| **Altura y Ergonomía** | ✅ Se contempla | Mecánica de recalibración automática al sentarse para evitar jugar en posturas incómodas. |
| **Feedback Háptico (Vibración)** | ❌ No se contempla | Actualmente no hay respuesta vibratoria al tocar cartas (área de mejora futura). |
| **Prevención de "Gorilla Arm"** | ✅ Se contempla | La mesa está a una altura baja y los elementos interactivos están al alcance de la mano descansada. |

---
*Proyecto realizado para la Universidad de La Laguna - Curso 2025/2026*
