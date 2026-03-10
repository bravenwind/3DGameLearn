using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField]
    private float walkBobbingSpeed = 13.0f;

    [SerializeField]
    private float bobbingAmount = 0.05f;

    [SerializeField]
    private FPSMovement playerMovement;

    private float timer = 0.0f;
    private float defaultPosY = 0.0f;

    void Start()
    {
        defaultPosY = transform.localPosition.y;    
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.isMoving)
        {
            timer += Time.deltaTime * walkBobbingSpeed;

            float waveSlice = Mathf.Sin(timer);

            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + waveSlice * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            timer = 0.0f;
            transform.localPosition = new Vector3(transform.localPosition.x, 
                Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkBobbingSpeed), 
                transform.localPosition.z);
        }
    }
}
