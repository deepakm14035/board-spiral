using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    PlayerController _player;
    Vector3 _offset;

    public void resetCamera() {
        transform.position = _player.curpos.transform.position + _offset;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindObjectOfType<PlayerController>();
        _offset = -_player.curpos.transform.position + gameObject.transform.position;
        _offset.x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Slerp(gameObject.transform.position, _player.curpos.transform.position + _offset,Time.deltaTime*3f);
    }
}
