using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraShaking : MonoBehaviour
{

    [SerializeField] GameObject cameraObject;
    [SerializeField] AnimationCurve curve = new();

    public float duraction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Position = cameraObject.transform.position;
    }

    Vector3 Position;
    float time = 0;

    IEnumerator Move(AnimationCurve curve, float duraction) {

        Vector3 origin = cameraObject.transform.localPosition;

        float elapsed = 0;

        while(elapsed < duraction) {

            elapsed += Time.deltaTime;

            float power = curve.Evaluate(elapsed / duraction);

            Vector3 randomVector = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), 0);

            cameraObject.transform.localPosition = Vector3.Lerp(cameraObject.transform.localPosition, origin + randomVector * power, 0.5f);

            yield return null;
        }

        cameraObject.transform.localPosition = origin;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) {
            StartCoroutine(Move(curve, duraction));
        }
    }
}
