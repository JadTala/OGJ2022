using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemyMovement : Enemy
{
    private bool canAttack = true;
    
    // Start is called before the first frame update
    void Start()
    {
        rarity = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SuperUpdate();
        if (GetComponent<Rigidbody2D>().velocity.magnitude == 0) GetComponent<Rigidbody2D>().angularVelocity = 0;
        
        Vector2 playerPosition = cam.WorldToViewportPoint(player.position);
        Vector2 transformPosition = (Vector2)cam.WorldToViewportPoint(transform.position);
        float angle = AngleBetweenTwoPoints(playerPosition, transformPosition);
        transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,angle + 10));
        if (canAttack)
        {
            StartCoroutine(Attack());
        }     
    }
    
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    IEnumerator Attack()
    {
        canAttack = false;
        anim.SetBool("Dash", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("Dash", false);
        GetComponent<Rigidbody2D>().AddForce(transform.right * 4000);
        yield return new WaitForSeconds(Random.Range(3, 6));
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {
            col.transform.GetComponent<PlayerMovement>().TakeDamage(2);
        }
    }
}
