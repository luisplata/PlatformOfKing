using UnityEngine;

public class BellsebossCharacter : PlayerFather
{
    protected override void Move()
    {
        base.Move();
        if(inputFacade.ActionButton)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.gravityScale = 0f;
        }
    }
}