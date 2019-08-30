using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject hookHead;
    public LayerMask attachable;
    public float maxHookTravelTime;
    public float hookSpeed;
    public GameObject player;
    public Movement movementScript;

    private Vector3 playerDestination;
    private Vector3 hookDestination;
    private enum HookPhase { still, expanding, rollingBack, pullingUp, hold};
    private HookPhase phase = HookPhase.still;
    private float startOfHookTrvel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (phase == HookPhase.still || phase == HookPhase.pullingUp || phase == HookPhase.hold))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            hookDestination = mousePosition;
            startOfHookTrvel = Time.time;
            phase = HookPhase.expanding;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (phase == HookPhase.hold|| phase == HookPhase.pullingUp))
        {
            phase = HookPhase.still;
        }
        switch (phase)
        {
            case HookPhase.still:
                player.GetComponent<Rigidbody2D>().gravityScale = 5;
                movementScript.canMove = true;
                movementScript.hooked = false;
                HookStill();
                break;
            case HookPhase.expanding:
                player.GetComponent<Rigidbody2D>().gravityScale = 5;
                movementScript.canMove = true;
                movementScript.hooked = false;
                HookExpanding();
                break;
            case HookPhase.rollingBack:
                player.GetComponent<Rigidbody2D>().gravityScale = 5;
                movementScript.canMove = true;
                movementScript.hooked = false;
                HookRollingBack();
                break;
            case HookPhase.pullingUp:
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
                movementScript.canMove = false;
                movementScript.hooked = false;
                HookPullingUp();
                break;
            case HookPhase.hold:
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
                movementScript.canMove = false;
                movementScript.hooked = true;
                break;
            default:
                break;
        }

        HookUpdateVisuals();
    }

    void HookStill()
    {
        transform.position = player.transform.position;
        hookHead.transform.position = transform.position;
    }

    void HookExpanding() {
        if (Time.time-startOfHookTrvel>= maxHookTravelTime)
        {
            phase = HookPhase.rollingBack;
        }

        if (Vector2.Distance(hookHead.transform.position, hookDestination) < 0.1f)
        {
            phase = HookPhase.rollingBack;
        }

        hookHead.transform.position = Vector3.MoveTowards(hookHead.transform.position, hookDestination, hookSpeed*Time.deltaTime);
    }

    void HookRollingBack() {
        hookHead.transform.position = Vector3.MoveTowards(hookHead.transform.position, transform.position, hookSpeed * Time.deltaTime);
        if (Vector2.Distance(hookHead.transform.position, transform.position)<0.1f)
        {
            phase = HookPhase.still;
        }
    }

    void HookPullingUp()
    {
        if (Vector2.Distance(playerDestination, player.transform.position) < 1f)
        {
            phase = HookPhase.hold;
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, playerDestination, hookSpeed * Time.deltaTime);
        hookHead.transform.position = playerDestination;
    }

    void HookUpdateVisuals()
    {
        lineRenderer.SetPosition(0, hookHead.transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (phase != HookPhase.expanding)
        {
            return;
        }
        if (attachable == (attachable | (1 << collision.gameObject.layer)))
        {
            playerDestination = hookHead.transform.position;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            phase = HookPhase.pullingUp;
        }
    }

    public void Detach()
    {
        phase = HookPhase.still;
    }

}
