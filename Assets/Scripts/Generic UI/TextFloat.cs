using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFloat : MonoBehaviour
{

    public float xSpeed;
    private float startTime;
    private string display;
    public int value;
    public int type;
    private Text textComp;
    private Outline outline;

    [SerializeField] private Color[] typeColors = new Color[4];

    // Use this for initialization
    void Start()
    {
        textComp = gameObject.GetComponent<Text>();
        outline = gameObject.GetComponent<Outline>();

        AdjustTextForValue();
        AdjustTextForType();
        InitializeXSpeed();

        textComp.text = display;
        startTime = Time.time;
        StartCoroutine(DestroyText());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    IEnumerator DestroyText()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    private void AdjustTextForValue()
    {
        if (value >= 0)
        {
            outline.effectColor = Color.white;
            display = '+' + value.ToString();
        }
        else
        {
            outline.effectColor = Color.black;
            display = value.ToString();
        }

        if (Mathf.Abs(value) >= 20)
        {
            this.transform.localScale *= 1.8f;
        }
        else if (Mathf.Abs(value) >= 10)
        {
            this.transform.localScale *= 1.4f;
        }
    }

    private void AdjustTextForType()
    {
        textComp.color = typeColors[type];
    }

    private void InitializeXSpeed()
    {
        while (Mathf.Abs(xSpeed) < 1f)
        {
            xSpeed = Random.Range(-2f, 2f);
        }
    }

    private void Move()
    {
        float adjustedTime = Time.time - startTime;
        float ySpeed = (Mathf.Sqrt(adjustedTime) - 1.1f * adjustedTime) * 50f;
        this.transform.Translate(new Vector3(xSpeed, ySpeed, 0f) * Time.deltaTime);
        this.transform.localScale *= (1 - .2f * Time.deltaTime);
    }


}
