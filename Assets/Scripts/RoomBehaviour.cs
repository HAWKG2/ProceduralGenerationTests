using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    // array pattern will be as follows
    // 0 - up | 1 - down | 2 - right | 3 - left
    public GameObject[] walls;
    public GameObject[] doors;
    public void UpdateRoom(bool[] status)
    {
        for(int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(!status[i]);
            doors[i].SetActive(status[i]);
        }
    }

}
