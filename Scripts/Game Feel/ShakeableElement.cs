using System.Collections;
using UnityEngine;

public class ShakeableElement : MonoBehaviour
{
    private const float TruenoSfxDuration = 4f;
    [SerializeField]
    [Range(0f, 6f)]
    private float shakeDurationInSeconds;

    [Header("Rotational")]
    [SerializeField]
    [Range(0f, 1f)]
    private float rotationalMaxMagnitude;
    [SerializeField]
    [Range(0f, 300f)]
    private float rotationalPower;

    [Header("Translational")]
    [SerializeField]
    [Range(0f, 1f)]
    private float translationalMaxMagnitude;
    [SerializeField]
    [Range(0f, 300f)]
    private float translationalPower;

    [Header("Trauma")]
    [SerializeField]
    [Range(0f, 1f)]
    private float trauma;
    public float Trauma { get => trauma; set => trauma = Mathf.Clamp01(value); }
    [SerializeField]
    [Range(0f, 5f)]
    private float traumaDecay;

    private Vector3 initialPosition;

    private Coroutine activeCoroutine;

    protected void Awake() => initialPosition = transform.position;

    [ContextMenu("Shake")]
    public void ShakeInEditor() => Shake(false);

    public void Shake(bool isTrueno, float intensity = 1f)
    {
        if (IsATruenoPlaying())
            return;

        Trauma = intensity;

        StartCoroutine(ShakeOverTime());

        bool IsATruenoPlaying() => Trauma == 1f;
        IEnumerator ShakeOverTime()
        {
            float elapsedTime = 0f;

            while (Trauma > 0)
            {
                elapsedTime += Time.deltaTime;

                AddRotationalShake();
                AddTranslationalShake();

                if (isTrueno)
                {
                    if (elapsedTime >= TruenoSfxDuration)
                        DecreaseTraumaOvertime();
                }
                else
                    DecreaseTraumaOvertime();

                yield return null;

                transform.position = initialPosition;
                transform.rotation = Quaternion.identity;
            }

            void AddRotationalShake()
            {
                float newAngle = rotationalMaxMagnitude * Trauma * GetPerlinNoise(0, rotationalPower);

                Vector3 rotation = transform.rotation.eulerAngles;
                rotation.z = newAngle;

                transform.rotation = Quaternion.Euler(rotation * rotationalPower);
            }
            void AddTranslationalShake()
            {
                Vector3 newPosition = translationalMaxMagnitude * Trauma * GetTranslationalVector();
                transform.position += newPosition * translationalPower;

                Vector3 GetTranslationalVector()
                {
                    return new Vector3(GetPerlinNoise(1, translationalPower),
                                       GetPerlinNoise(2, translationalPower),
                                       0);
                }
            }
            void DecreaseTraumaOvertime() => Trauma -= Time.deltaTime * traumaDecay * (Trauma + 0.3f);
            float GetPerlinNoise(int seed, float power)
            {
                float requestedValue = elapsedTime * Mathf.Pow(Trauma, 0.3f) * power;

                return -1 + Mathf.PerlinNoise(seed, requestedValue) * 2;
            }
        }
    }

}
