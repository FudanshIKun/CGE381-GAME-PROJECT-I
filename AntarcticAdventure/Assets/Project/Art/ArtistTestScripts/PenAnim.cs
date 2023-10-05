using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PenAnim : MonoBehaviour
{
    public Rigidbody rb;
    public float jumpForce;
    public Animator anim;
    public float groundPosition;
    public bool is_ground;
    bool is_jump_right_side;
    bool is_copter;
    public float copterHeight;
    public float copterLerpSpeed;
    void Start()
    {
    }

    void Update()
    {
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && is_ground)
        {
            jump(jumpForce);
            anim.SetBool("is_jump", true);
            play_jump_animation();
            is_jump_right_side = !is_jump_right_side;
            anim.SetBool("is_jump_right", is_jump_right_side);
        }

        check_ground();

        //Left Right Control
        if (Input.GetKey(KeyCode.A))
        {
            run_blending(-1);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            run_blending(1);
        }
        else
        {
            run_blending(0);
        }

        //Stumble Control
        if (Input.GetKeyDown(KeyCode.E))
        {
            play_stumble_animation();
        }

        //Win Control
        if(Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("is_win", true);
            play_jump_animation();
            jump(stumbleJumpForce);
        }

        copter();

    }
    void copter()
    {
        if(Input.GetKey(KeyCode.C))
        {
            is_copter = true;
            float target = Mathf.Lerp(rb.position.y, copterHeight, Time.deltaTime * copterLerpSpeed);
            rb.position = new Vector3(rb.position.x, target, rb.position.z);
            rb.velocity = Vector3.zero;
        }
        else
        {
            is_copter = false;
        }

        anim.SetBool("is_copter", is_copter);


    }
    void check_ground()
    {
        //Check Ground
        if (scaleRoot.transform.position.y <= groundPosition && rb.velocity.y <= 0)
        {
            if (is_ground == false)
            {
                //Impact
                play_impact_animation();
            }
            is_ground = true;
            anim.SetBool("is_jump", false);
            anim.SetBool("is_stumble", false);
        }
        else
        {
            is_ground = false;
        }
        anim.SetBool("is_ground", is_ground);
    }

    void jump(float force)
    {
        rb.velocity = new Vector3(0, force, 0);
    }
    //Copy This Region
    #region Scale Animation
    [Header("Procedural Animation")]
    public Transform scaleRoot;
    public float xScaleWeight;

    [Header("Ground Impact ")]
    public AnimationCurve impactCurve;
    public float impactInterval;
    public float impactWeight;

    [Header("Jump")]
    public AnimationCurve jumpCurve;
    public float jumpInterval;
    public float jumpWeight;

    [Header("Stumble")]
    public float stumbleJumpForce;

    [Header("Run Blend")]
    public float runBlendLerpSpeed;
    
    void run_blending(float target)
    {
        anim.SetFloat("run_blend", Mathf.Lerp(anim.GetFloat("run_blend"), target, Time.deltaTime * runBlendLerpSpeed));
    }
    //One Shot Animation Function
    void play_jump_animation()
    {
        StartCoroutine(play_aniamtion_event(jumpCurve, jumpInterval, jumpWeight));
    }
    void play_impact_animation()
    {
        StartCoroutine(play_aniamtion_event(impactCurve, impactInterval, impactWeight));
    }
    void play_stumble_animation()
    {
        play_jump_animation();
        jump(stumbleJumpForce);
        anim.SetBool("is_stumble", true);
    }
    IEnumerator play_aniamtion_event(AnimationCurve curve, float interval, float weight)
    {
        float elasTime = 0;
        while(elasTime < impactInterval)
        {
            elasTime += Time.deltaTime;
            float value = curve.Evaluate(elasTime / interval);
            float weightedValue = lerp_unclamp(1, value, weight);
            set_scale(weightedValue);
            yield return null;
        }
        scaleRoot.transform.localScale = new Vector3(1, 1, 1);
    }

    //Set Scale with X Offset
    void set_scale(float yScale)
    {
        float xScale = lerp_unclamp(1, (1 / yScale), xScaleWeight);
        scaleRoot.transform.localScale = new Vector3(xScale, yScale, 1);
    }
    float lerp_unclamp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
    #endregion
}
