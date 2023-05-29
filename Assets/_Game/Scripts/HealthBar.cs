using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset; //độ lệch giữa UI với Target

    float hp;
    float maxHp;

    private Transform target;

    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount, hp / maxHp, Time.deltaTime * 5f); //tính hàm tuyến tính fillamout tới lượng máu còn lại
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp, Transform target)
    {
        this.target = target;   
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1; //lấy thông số hình ảnh
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;

        imageFill.fillAmount = hp / maxHp;
    }
}
