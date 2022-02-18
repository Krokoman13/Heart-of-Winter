using UnityEngine;
using System.Collections;

using HeartOfWinter.Arena;
using HeartOfWinter.Characters;


public class ClickManager : MonoBehaviour
{
    [SerializeField] Playfield playfield;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            Collider2D collider = hit.collider;

            if (collider == null) return;

            Character collided = collider.gameObject.GetComponent<Character>();

            if (collided != null)
            {
                if (!collided.outLine) return;

                playfield.ClickedOn(collided);
            }
        }
    }

}