using UnityEngine;

public class RoundLight : MonoBehaviour
{
    private void Update()//Updateは毎フレーム呼ばれる（対策が必要）
    {
        //y軸に対して1秒間に-12度回転
        transform.Rotate(new Vector3(0, -12) * Time.deltaTime); //Time.deltaTimeは前のフレームからの経過時間（秒）を取得する
    }
}
