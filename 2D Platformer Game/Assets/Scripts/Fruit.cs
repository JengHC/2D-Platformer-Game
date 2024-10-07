using UnityEngine;

public class Fruit : MonoBehaviour
{

    public float TimeAdd = 2;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameMangerController.Instance.AddTime(TimeAdd);
            GetComponent<Animator>().SetTrigger("Eaten");
            GetComponent<Collider2D>().enabled = false;
            

            //Invoke("OnDestroy", 0.6f);

            Debug.Log("Eat item");
        }
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
