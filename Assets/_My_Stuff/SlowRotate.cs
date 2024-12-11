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
    }

    void Update() {
        // Berechne die Rotation anhand der Zeit und Geschwindigkeit
        currentRotation += rotationSpeed * Time.deltaTime;

        // Sicherstellen, dass der Rotation-Wert im Bereich von 0° bis 360° bleibt
        currentRotation %= 360f;
        print(currentRotation);

        // Aktualisiere die Rotation des Objekts direkt, um das Objekt zu drehen
        transform.rotation = Quaternion.Euler(currentRotation,0f,0f);

        // Berechnung der Lautstärken für Forest-Sound
        if (currentRotation >= 0f && currentRotation <= 90f) {
            // Forest wird lauter von 0° bis 90°
            forestSound.volume = Mathf.Lerp(0f,forestMasterVolume,currentRotation / 90f);
        } else if (currentRotation > 90f && currentRotation <= 180f) {
            // Forest wird leiser von 90° bis 180°
            forestSound.volume = Mathf.Lerp(forestMasterVolume,0f,(currentRotation - 90f) / 90f);
        } else {
            // Nacht: Forest ist aus
            forestSound.volume = 0f;
        }

        // Berechnung der Lautstärken für Wind-Sound
        if (currentRotation >= 180f && currentRotation <= 270f) {
            // Wind wird lauter von 180° bis 270°
            windSound.volume = Mathf.Lerp(0f,windMasterVolume,(currentRotation - 180f) / 90f);
        } else if (currentRotation > 270f && currentRotation <= 360f) {
            // Wind wird leiser von 270° bis 360°
            windSound.volume = Mathf.Lerp(windMasterVolume,0f,(currentRotation - 270f) / 90f);
        } else {
            // Tag: Wind ist aus
            windSound.volume = 0f;
        }

        // Horror- und Wolfs-Sounds nur nach einem gewissen Zeitpunkt abspielen (200° und 320°)
        if (currentRotation >= 220f && !horrorPlayed) // Horror bei 200° (nach 200° Tag/Nacht)
        {
            horrorSound.volume = horrorMasterVolume;
            horrorSound.PlayOneShot(horrorSound.clip);
            horrorPlayed = true; // Horror-Sound nur einmal
        }

        if (currentRotation >= 300f && !wolfPlayed) // Wolf bei 320° (nach 320° Tag/Nacht)
        {
            wolfSound.volume = wolfMasterVolume;
            wolfSound.PlayOneShot(wolfSound.clip);
            wolfPlayed = true; // Wolf-Sound nur einmal
        }

        // Reset der Flags, wenn die Rotation zum Tag zurückkehrt (0° bis 180°)
        if (currentRotation >= 0f && currentRotation <= 180f) {
            // Tagsüber: Horror und Wolf können zurückgesetzt werden, um bei Bedarf erneut zu spielen
            horrorPlayed = false;
            wolfPlayed = false;
        }
    }
}