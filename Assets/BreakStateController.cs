using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakStateController : MonoBehaviour
{
    public Sprite[] states;
    public SpriteRenderer target_sprite;
    private int current_state=0;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        Color new_color=target_sprite.color;
        new_color.a = 0;
        target_sprite.color=new_color;
        active=false;
    }

    public void NextState(){
        if (active==false)
        {
        Color new_color=target_sprite.color;
        new_color.a = 0.9f;
        target_sprite.color=new_color;
        active=true;         
        }
        target_sprite.sprite=states[current_state];
        current_state++;
    }


}
