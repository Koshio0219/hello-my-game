using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifeSpan = 5.0f;
    private Vector3 direction;
    private bool AttackTrigger;
    // Start is called before the first frame update
    void Start()
    {
        AttackTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (AttackTrigger)
        {
            BulletMove();
            lifeSpan -= Time.deltaTime;
            if (lifeSpan <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (this.transform.localScale.x < 1.0)
            {
                this.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f) * Time.deltaTime;
            }
        }

    }
    public void setAttackTrigger()
    {
        AttackTrigger = true;
    }

    public void setDirection(Vector3 Direction)
    {
        direction = Direction;
    }

    private void BulletMove() {
        this.transform.position = this.transform.position + direction * Time.deltaTime;
    }
    
}
