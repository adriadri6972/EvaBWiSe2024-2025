using UnityEngine;

public class Fire:MonoBehaviour {
    // Referenz zur Point Light Komponente
    private Light fireLight;

    // Einstellungen für die Lichtintensität
    public float minIntensity = 0.8f;
    public float maxIntensity = 2.0f;

    // Einstellungen für die Farben des Feuers
    public Color color1 = new Color(1.0f, 0.5f, 0.0f); // Orange
    public Color color2 = new Color(1.0f, 0.8f, 0.3f); // Gelblich

    // Geschwindigkeit der Flicker-Effekte
    public float intensitySpeed = 5.0f;
    public float colorSpeed = 3.0f;

    private float randomSeed;

    void Start() {
        // Point Light Komponente holen
        fireLight = GetComponent<Light>();

        if (fireLight == null) {
            Debug.LogError("FireLightFlicker benötigt ein Light-Objekt!");
            enabled = false;
            return;
        }

        // Sicherstellen, dass es ein Point Light ist
        if (fireLight.type != LightType.Point && fireLight.type != LightType.Spot) {
            Debug.LogError("FireLightFlicker funktioniert nur mit Point oder Spot Lights!");
            enabled = false;
            return;
        }

        // Zufälligen Seed für Perlin Noise generieren
        randomSeed = Random.Range(0,1000);
    }

    void Update() {
        // Lichtintensität mit Perlin Noise zufällig ändern
        float noise = Mathf.PerlinNoise(randomSeed, Time.time * intensitySpeed);
        fireLight.intensity = Mathf.Lerp(minIntensity,maxIntensity,noise);

        // Farbe des Lichts mit einem sanften Übergang wechseln
        float colorLerp = Mathf.PerlinNoise(randomSeed + 1, Time.time * colorSpeed);
        fireLight.color = Color.Lerp(color1,color2,colorLerp);
    }
}
