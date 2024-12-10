using UnityEngine;

public class SlowRotate:MonoBehaviour {
    public bool startAtDay = true; // Legt fest, wo die Rotation beginnt
    public AudioSource forestSound;
    public AudioSource windSound;
    public AudioSource horrorSound;
    public AudioSource wolfSound;

    [Range(0f, 1f)] public float forestMasterVolume = 1f;
    [Range(0f, 1f)] public float windMasterVolume = 1f;
    [Range(0f, 1f)] public float horrorMasterVolume = 1f;
    [Range(0f, 1f)] public float wolfMasterVolume = 1f;

    public float rotationDuration = 300f; // Zeit in Sekunden für eine vollständige 360-Grad-Drehung

    private float rotationSpeed;
    private float nextHorrorSoundTime;
    private float nextWolfSoundTime;

    void Start() {
        // Initiale Rotation setzen
        if (startAtDay) {
            transform.rotation = Quaternion.Euler(90f,0f,0f);
        } else {
            transform.rotation = Quaternion.Euler(-90f,0f,0f);
        }

        // Rotation in Grad pro Sekunde berechnen
        rotationSpeed = 360f / rotationDuration;

        // Setze Audioquellen auf Loop oder nicht
        forestSound.loop = true;
        windSound.loop = true;

        // Forest Sound und Wind starten
        forestSound.Play();
        windSound.Play();

        // Horror und Wolf starten nicht auf Loop
        ScheduleNextNightSounds();
    }

    void Update() {
        // Rotation durchführen
        transform.Rotate(rotationSpeed * Time.deltaTime,0f,0f);

        // Begrenzung auf 0–360 Grad und Anpassung der Wrap-Around-Logik
        float currentRotation = transform.eulerAngles.x;
        if (currentRotation > 180f) currentRotation -= 360f;

        // Forest-Sound Lautstärke (Tag)
        if (currentRotation >= 0f && currentRotation <= 90f) {
            // Lauter werden von 0° bis 90°
            forestSound.volume = Mathf.Lerp(0f,forestMasterVolume,currentRotation / 90f);
        } else if (currentRotation > 90f && currentRotation <= 180f) {
            // Leiser werden von 90° bis 180°
            forestSound.volume = Mathf.Lerp(forestMasterVolume,0f,(currentRotation - 90f) / 90f);
        } else {
            // Nacht (180° bis -90°)
            forestSound.volume = 0f;
        }

        // Wind-Sound Lautstärke (Nacht)
        if (currentRotation >= -90f && currentRotation <= 0f) {
            // Lauter werden von -90° bis 0°
            windSound.volume = Mathf.Lerp(0f,windMasterVolume,(currentRotation + 90f) / 90f);
        } else if (currentRotation > 0f && currentRotation <= 90f) {
            // Leiser werden von 0° bis 90°
            windSound.volume = Mathf.Lerp(windMasterVolume,0f,currentRotation / 90f);
        } else {
            // Tag (90° bis 180°)
            windSound.volume = 0f;
        }

        // Horror- und Wolfs-Sounds nur nachts abspielen (-90° bis 0°)
        if (currentRotation >= -90f && currentRotation <= 0f) {
            if (Time.time >= nextHorrorSoundTime) {
                horrorSound.volume = horrorMasterVolume;
                horrorSound.PlayOneShot(horrorSound.clip);
                ScheduleNextHorrorSound();
            }

            if (Time.time >= nextWolfSoundTime) {
                wolfSound.volume = wolfMasterVolume;
                wolfSound.PlayOneShot(wolfSound.clip);
                ScheduleNextWolfSound();
            }
        }
    }

    private void ScheduleNextNightSounds() {
        ScheduleNextHorrorSound();
        ScheduleNextWolfSound();
    }

    private void ScheduleNextHorrorSound() {
        nextHorrorSoundTime = Time.time + Random.Range(10f,60f); // Horror 1x pro Minute
    }

    private void ScheduleNextWolfSound() {
        nextWolfSoundTime = Time.time + Random.Range(30f,120f); // Wolf 2x pro Nacht
    }
}
