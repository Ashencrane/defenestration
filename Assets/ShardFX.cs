using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardFX : MonoBehaviour
{
    [SerializeField]
    List<Sprite> sprites;

    [SerializeField]
    GameObject shard;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Sprite s in sprites)
        {
            GameObject newShard = Instantiate(shard, transform);
            newShard.GetComponent<SpriteRenderer>().sprite = s;

            Vector2 randomForce = new Vector2(Random.Range(-300f, 300f), Random.Range(-300f, 300f));

            newShard.GetComponent<Rigidbody2D>().AddForce(randomForce);
        }

        StartCoroutine("DeleteAfterTime");
    }

    IEnumerator DeleteAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
