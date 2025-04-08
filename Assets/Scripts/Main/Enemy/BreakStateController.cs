using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakStateController : MonoBehaviour
{
    public Sprite[] states;
    public SpriteRenderer target_sprite;
    private int current_state = 0;
    private bool active;
    // Start is called before the first frame update
    void Start()
    {
        Color new_color = target_sprite.color;
        new_color.a = 0;
        target_sprite.color = new_color;
        active = false;
    }

    public void NextState()
    {
        ShowBreakState();
        UpdateState();
        current_state++;
        LimitExcess();
    }
    public void NextState(int damage)
    {
        ShowBreakState();
        current_state += damage;
        LimitExcess();
        UpdateState();
    }
    public void RevertDamage(int heal_value)
    {
        current_state -= heal_value;
        if (current_state < 0)
        {
            current_state = 0;
        }
        UpdateState();

    }
    private void UpdateState()
    {
        target_sprite.sprite = states[current_state];
    }
    private void ShowBreakState()
    {
        if (active == false)
        {
            Color new_color = target_sprite.color;
            new_color.a = 0.9f;
            target_sprite.color = new_color;
            active = true;
        }
    }
    private void LimitExcess(){
        if (current_state > states.Length-1){
            current_state = states.Length-1;
        }
    }



}
