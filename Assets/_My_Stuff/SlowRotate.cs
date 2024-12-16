using UnityEngine;

public class SlowRotate:MonoBehaviour {
    public bool startAtDay = true; // Legt fest, wo die Rotation beginnt
    public AudioSource forestSound;
    public AudioSource windSound;
    public AudioSource horrorSound;
    public AudioSource wolfSound;

    [Range(0f, 1f)] public float forestMasterVolume = 0.5f;
    [Range(0f, 1f)] public float windMasterVolume = 0.5f;
    [Range(0f, 1f)] public float horrorMasterVolume = 0.5f;
    [Range(0f, 1f)] public float wolfMasterVolume = 0.5f;

    public float rotationDuration = 60f; // Zeit in Sekunden für eine vollständige 360-Grad-Drehung

    [Header("Fog Settings")]
    public float fogStartDensity = 0.0f; // Anfangsdichte des Nebels
    public float fogMaxDensity = 0.05f; // Maximale Dichte des Nebels

    private float rotationSpeed;
    private bool horrorPlayed;
    private bool wolfPlayed;
    private float currentRotation = 0f; // Initialer Rotationswert

    void Start() {
        // Initiale Rotation setzen
        if (startAtDay) {
            currentRotation = 0f; // Start bei Tag (0°)
        } else {
            currentRotation = 180f; // Start bei Nacht (180°)
        }

        // Rotation in Grad pro Sekunde berechnen
        rotationSpeed = 360f / rotationDuration;

        // Setze Audioquellen auf Loop oder nicht
        forestSound.loop = true;
        windSound.loop = true;

        // Falls "Play On Awake" deaktiviert ist, müssen wir die Sounds manuell starten
        forestSound.Play();
        windSound.Play();

        // Nebel initialisieren
        RenderSettings.fog = false;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = fogStartDensity;
        RenderSettings.fogColor = Color.black; // Setze die Nebelfarbe auf Schwarz
    }

    void Update() {
        // Berechne die Rotation anhand der Zeit und Geschwindigkeit
        currentRotation += rotationSpeed * Time.deltaTime;

        // Sicherstellen, dass der Rotation-Wert im Bereich von 0° bis 360° bleibt
        currentRotation %= 360f;
        print(currentRotation);

        // Aktualisiere die Rotation des Objekts direkt, um das Objekt zu drehen
        transform.rotation = Quaternion.Euler(currentRotation,0f,0f);

        // Lautstärke und Nebel anpassen
        UpdateForestSound();
        UpdateWindSound();
        UpdateFog();

        // Horror- und Wolfs-Sounds
        if (currentRotation >= 220f && !horrorPlayed) {
            horrorSound.volume = horrorMasterVolume;
            horrorSound.PlayOneShot(horrorSound.clip);
            horrorPlayed = true;
        }

        if (currentRotation >= 300f && !wolfPlayed) {
            wolfSound.volume = wolfMasterVolume;
            wolfSound.PlayOneShot(wolfSound.clip);
            wolfPlayed = true;
        }

        // Reset der Flags
        if (currentRotation >= 0f && currentRotation <= 180f) {
            horrorPlayed = false;
            wolfPlayed = false;
        }
    }

    void UpdateForestSound() {
        if (currentRotation >= 0f && currentRotation <= 90f) {
            forestSound.volume = Mathf.Lerp(0f,forestMasterVolume,currentRotation / 90f);
        } else if (currentRotation > 90f && currentRotation <= 180f) {
            forestSound.volume = Mathf.Lerp(forestMasterVolume,0f,(currentRotation - 90f) / 90f);
        } else {
            forestSound.volume = 0f;
        }
    }

    void UpdateWindSound() {
        if (currentRotation >= 180f && currentRotation <= 270f) {
            windSound.volume = Mathf.Lerp(0f,windMasterVolume,(currentRotation - 180f) / 90f);
        } else if (currentRotation > 270f && currentRotation <= 360f) {
            windSound.volume = Mathf.Lerp(windMasterVolume,0f,(currentRotation - 270f) / 90f);
        } else {
            windSound.volume = 0f;
        }
    }

    void UpdateFog() {
        if (currentRotation >= 180f && currentRotation <= 270f) {
            // Nebel wird dichter von 180° bis 270°
            float fogDensity = Mathf.Lerp(fogStartDensity, fogMaxDensity, (currentRotation - 180f) / 90f);
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fog = true; // Nebel aktivieren
        } else if (currentRotation > 270f && currentRotation <= 360f) {
            // Nebel wird dünner von 270° bis 360°
            float fogDensity = Mathf.Lerp(fogMaxDensity, fogStartDensity, (currentRotation - 270f) / 90f);
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fog = true; // Nebel aktiv halten
        } else {
            // Tag: Nebel aus
            RenderSettings.fog = false;
        }
    }
}
