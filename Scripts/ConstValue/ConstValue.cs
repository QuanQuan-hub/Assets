
using UnityEngine;

public static class ConstValue
{
    //视野的高度限制，-60是仰角60度，60是俯角60度
    public static float ViewMinimumVert = -60;
    public static float ViewMaximumVert = 60;
    //左手IK需要乘的参数
    public static float leftHandIKIndexX = -10;
    public static float leftHandIKIndexY = 35;
    public static float leftHandIKIndexZ = 175;
    //一些变异的单位向量
    public static Vector3 NoneY = new Vector3(1, 0, 1);
}
