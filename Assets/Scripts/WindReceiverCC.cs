using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WindReceiverCC : MonoBehaviour
{
    CharacterController cc;
    float externalY = 0f;

    void Awake() { cc = GetComponent<CharacterController>(); }

    // Called by WindZone3D while inside trigger
    public void AddWind(float y)
    {
        externalY = Mathf.Max(externalY, y);
    }

    void Update()
    {
        Vector3 move = Vector3.zero;
        if (externalY > 0f)
        {
            move.y += externalY;
            externalY = Mathf.MoveTowards(externalY, 0f, 9.81f * Time.deltaTime);
        }
        cc.Move(move * Time.deltaTime);
    }
}